using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class Program : MonoBehaviour
{
    public Transform sphere;
    public ModbusManager modbusManager;

    int direction_command;

    public Dropdown dropDown_PortNames;
    public Button button_Connect;

    private float MOVING_RATIO = 1.0f;
    private float UPDATE_PERIOD = 0.1f;
    private float duration;

    public void OnButtonLeft()
    {
        direction_command = -1;
    }

    public void OnButtonStop()
    {
        direction_command = 0;
    }

    public void OnButtonRight()
    {
        direction_command = 1;
    }

    public void OnHitLeft(bool hit)
    {
        Debug.Log("OnHitLeft: " + hit.ToString());
        modbusManager.WriteCoil(0, hit);
    }

    public void OnHitRight(bool hit)
    {
        Debug.Log("OnHitRight: " + hit.ToString());
        modbusManager.WriteCoil(1, hit);
    }

    public void OnConnect()
    {
        if (!modbusManager.IsConnected())
        {
            if (modbusManager.Connect(dropDown_PortNames.options[dropDown_PortNames.value].text))
            {
                button_Connect.GetComponentInChildren<Text>().text = "Disconnect";
            }
        }
        else
        {
            modbusManager.Disconnect();
            button_Connect.GetComponentInChildren<Text>().text = "Connect";
        }
    }

    public void OnRead()
    {
        modbusManager.ReadState();
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] portNames = SerialPort.GetPortNames();
        dropDown_PortNames.ClearOptions();
        foreach(string portName in portNames)
        {
            dropDown_PortNames.options.Add(new Dropdown.OptionData() { text = portName });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (direction_command == -1)
        {
            sphere.Translate(-Time.deltaTime* MOVING_RATIO, 0, 0);
        }
        else if (direction_command == 1)
        {
            sphere.Translate(Time.deltaTime* MOVING_RATIO, 0, 0);
        }

        duration += Time.deltaTime;
        if (duration > UPDATE_PERIOD)
        {
            duration -= UPDATE_PERIOD;

            ushort position = Convert.ToUInt16((sphere.transform.localPosition.x + 5.0f) * 1000);
            modbusManager.WriteRegister(0, position);
            //Debug.Log("position: " + position.ToString());
        }
    }
}
