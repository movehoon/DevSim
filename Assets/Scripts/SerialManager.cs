using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialManager : MonoBehaviour
{

    public string[] Availables()
    {
        return System.IO.Ports.SerialPort.GetPortNames();
    }

    public bool Connect(string name)
    {
        using (SerialPort port = new SerialPort(name))
        {
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.Open();
        }
        return false;
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
