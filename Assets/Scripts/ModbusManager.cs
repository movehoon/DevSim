using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using Modbus.Device;

public class ModbusManager : MonoBehaviour
{
    SerialPort _port;
    ModbusSerialMaster _master;

    private const int SLAVE_ADDRESS = 1;
    private const int COIL_ADDRESS = 0;
    private const int ISTS_ADDRESS = 100;
    private const int HREG_ADDRESS = 200;

    public bool Connect(string portName)
    {
        if (_port == null && _master == null)
        {
            _port = new SerialPort(portName, 9600);
            _port.ReadTimeout = 100;
            _port.WriteTimeout = 100;
            _port.Open();

            _master = ModbusSerialMaster.CreateRtu(_port);

            ReadState();
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

    public bool IsConnected()
    {
        if (_port != null)
            return _port.IsOpen;
        return false;
    }

    public void ReadState()
    {
        // Read the current state of the output
        var coils = _master.ReadCoils(SLAVE_ADDRESS, COIL_ADDRESS, 10);
        Debug.Log(coils[0].ToString());
        Debug.Log(coils[1].ToString());

        var istss = _master.ReadInputs(SLAVE_ADDRESS, ISTS_ADDRESS, 10);
        Debug.Log(istss[0].ToString());
        Debug.Log(istss[1].ToString());

        var hregs = _master.ReadHoldingRegisters(SLAVE_ADDRESS, HREG_ADDRESS, 10);
        Debug.Log(hregs[0].ToString());
        Debug.Log(hregs[1].ToString());
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
