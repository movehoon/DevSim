/*
  ModbusRTU ESP8266/ESP32
  Simple slave example

  (c)2019 Alexander Emelianov (a.m.emelianov@gmail.com)
  https://github.com/emelianov/modbus-esp8266

  modified 13 May 2020
  by brainelectronics

  This code is licensed under the BSD New License. See LICENSE.txt for more info.
*/

#include <ModbusRTU.h>

#define COIL_ADDR 0
#define COIL_CONT 10

#define ISTS_ADDR 100
#define ISTS_CONT 10

#define HREG_ADDR 200
#define HREG_CONT 10

#define SLAVE_ID 1

ModbusRTU mb;

void setup() {
  Serial.begin(9600, SERIAL_8N1);
  Serial.print("Start\n");
#if defined(ESP32) || defined(ESP8266)
  mb.begin(&Serial);
#else
  mb.begin(&Serial);
  mb.setBaudrate(9600);
#endif
  mb.slave(SLAVE_ID);

  mb.addCoil(COIL_ADDR, false, COIL_CONT);
  for (int i=0; i<COIL_CONT; i++) {
    mb.Coil(COIL_ADDR+i, (i%2==0));
  }

  mb.addIsts(ISTS_ADDR, false, ISTS_CONT);
  for (int i=0; i<ISTS_CONT; i++) {
    mb.Ists(ISTS_ADDR+i, (i%2==0));
  }

  mb.addHreg(HREG_ADDR, 0, HREG_CONT);
  for (int i=0; i<HREG_CONT; i++) {
    mb.Hreg(HREG_ADDR+i, i);
  }
}

void loop() {
  mb.task();
  yield();
}
