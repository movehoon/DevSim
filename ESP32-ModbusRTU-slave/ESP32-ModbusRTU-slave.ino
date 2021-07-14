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


#define PIN_DIN1  5
#define PIN_DIN2  18
#define PIN_DOUT1 19
#define PIN_DOUT2 21

#define PIN_DAC1  25
#define PIN_DAC2  26


ModbusRTU mb;

void setup() {
  Serial.begin(115200, SERIAL_8N1);
  Serial.print("Start\n");

  Serial2.begin(115200, SERIAL_8N1);
  Serial2.print("Start2\n");

  pinMode(PIN_DIN1, INPUT);
  pinMode(PIN_DIN2, INPUT);
  pinMode(PIN_DOUT1, OUTPUT);
  pinMode(PIN_DOUT2, OUTPUT);

#if defined(ESP32) || defined(ESP8266)
  mb.begin(&Serial2);
#else
  mb.begin(&Serial2);
  mb.setBaudrate(115200);
#endif
  mb.slave(SLAVE_ID);

  mb.addCoil(COIL_ADDR, false, COIL_CONT);
//  for (int i=0; i<COIL_CONT; i++) {
//    mb.Coil(COIL_ADDR+i, (i%2==0));
//  }

  mb.addIsts(ISTS_ADDR, false, ISTS_CONT);
//  for (int i=0; i<ISTS_CONT; i++) {
//    mb.Ists(ISTS_ADDR+i, (i%2==0));
//  }

  mb.addHreg(HREG_ADDR, 0, HREG_CONT);
//  for (int i=0; i<HREG_CONT; i++) {
//    mb.Hreg(HREG_ADDR+i, i);
//  }
}

void loop() {

  if (digitalRead(PIN_DIN1))  mb.Ists(ISTS_ADDR+0, true);
  else                        mb.Ists(ISTS_ADDR+0, false);

  if (digitalRead(PIN_DIN2))  mb.Ists(ISTS_ADDR+1, true);
  else                        mb.Ists(ISTS_ADDR+1, false);

  digitalWrite(PIN_DOUT1, mb.Coil(COIL_ADDR+0));
  digitalWrite(PIN_DOUT2, mb.Coil(COIL_ADDR+1));

  dacWrite(PIN_DAC1, mb.Hreg(HREG_ADDR));
  dacWrite(PIN_DAC2, 127);


  Serial.print("D1:");
  Serial.print(digitalRead(PIN_DIN1));
  Serial.print(", D2:");
  Serial.print(digitalRead(PIN_DIN2));
  Serial.print(", DO1:");
  Serial.print(mb.Coil(COIL_ADDR));
  Serial.print(", DO2:");
  Serial.print(mb.Coil(COIL_ADDR+1));
  Serial.print(", AO1:");
  Serial.print(mb.Hreg(HREG_ADDR));
  Serial.println();

  mb.task();
  yield();

//  delay(100);
}
