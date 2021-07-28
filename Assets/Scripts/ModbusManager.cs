using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using Modbus.Device;

public class ModbusManager : MonoBehaviour
{
    SerialPort _port;
    ModbusSerialMaster _master;

    private byte SLAVE_ADDRESS = 1;
    private const int COIL_ADDRESS = 0; // DIN1~4
    private const int COIL_COUNT = 4;
    //private const int ISTS_ADDRESS = 0;
    //private const int ISTS_COUNT = 8;
    private const int HREG_ADDRESS = 0;
    private const int HREG_COUNT = 4;

    private float UPDATE_PERIOD = 0.1f;
    private float duration;

    private bool[] mb_coil = new bool[COIL_COUNT];
    //private bool[] mb_ists = new bool[ISTS_COUNT];
    private ushort[] mb_hreg = new ushort[HREG_COUNT];

    public bool Connect(string portName, byte slave_addr = 1)
    {
        if (_port == null && _master == null)
        {
            _port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
            _port.ReadTimeout = 500;
            _port.WriteTimeout = 500;
            _port.Open();

            _master = ModbusSerialMaster.CreateRtu(_port);
            SLAVE_ADDRESS = slave_addr;

            //ReadState();
            return true;
        }
        return false;
    }

    public void Disconnect()
    {
        _master.Dispose();
        _master = null;

        _port.Close();
        _port.Dispose();
        _port = null;
    }

    public bool IsConnected
    {
        get
        {
            if (_port != null)
                return _port.IsOpen;
            return false;
        }
    }
    public bool ReadCoil(ushort addr)
    {
        if (IsConnected)
        {
            return mb_coil[addr];
        }
        return false;
    }
    public bool WriteCoil(ushort addr, bool value)
    {
        if (IsConnected)
        {
            _master.WriteSingleCoil(SLAVE_ADDRESS, addr, value);
            return true;
        }
        return false;
    }

    //public bool ReadISTS(ushort addr)
    //{
    //    if (IsConnected)
    //    {
    //        return mb_ists[addr];
    //    }
    //    return false;
    //}
    public ushort ReadRegister(ushort addr)
    {
        if (IsConnected)
        {
            return mb_hreg[addr];
        }
        return 0;
    }
    public bool WriteRegister(ushort addr, ushort value)
    {
        if (IsConnected)
        {
            _master.WriteSingleRegister(SLAVE_ADDRESS, addr, value);
            return true;
        }
        return false;
    }

    public void ReadState()
    {
        try
        {
            // Read the current state of the output
            mb_coil = _master.ReadCoils(SLAVE_ADDRESS, COIL_ADDRESS, 10);
            Debug.Log(mb_coil[0].ToString());
            Debug.Log(mb_coil[1].ToString());

            //mb_ists = _master.ReadInputs(SLAVE_ADDRESS, ISTS_ADDRESS, 10);
            //Debug.Log(mb_ists[0].ToString());
            //Debug.Log(mb_ists[1].ToString());

            mb_hreg = _master.ReadHoldingRegisters(SLAVE_ADDRESS, HREG_ADDRESS, 10);
            Debug.Log(mb_hreg[0].ToString());
            Debug.Log(mb_hreg[1].ToString());
            //// Update the UI
            //if (state[0])
            //{
            //    StateLabel.Text = "On";
            //}
            //else
            //{
            //    StateLabel.Text = "Off";
            //}
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        duration += Time.deltaTime;
        if (duration > UPDATE_PERIOD)
        {
            duration -= UPDATE_PERIOD;

            if (IsConnected)
            {
                ReadState();
            }
        }
    }
}