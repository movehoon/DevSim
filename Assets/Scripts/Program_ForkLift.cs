using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Program_ForkLift : MonoBehaviour
{
    public ModbusManager modbusManager;

    public Button button_Connect;

    private float UPDATE_PERIOD = 0.1f;

    private float duration;
    private bool d1;


    public void OnConnect()
    {
        if (!modbusManager.Connected)
        {
            modbusManager.Connect("192.168.1.3");
            //string portName = dropDown_PortNames.options[dropDown_PortNames.value].text;
            //if (modbusManager.Connect(portName))
            //{
            //    Debug.Log("Modbus Connected with " + portName);
                button_Connect.GetComponentInChildren<Text>().text = "Disconnect";
            //}
            //else
            //{
            //    Debug.Log("Modbus Connection failed with " + portName);
            //}
        }
        else
        {
            modbusManager.Disconnect();
            Debug.Log("Modbus disconnected");
            button_Connect.GetComponentInChildren<Text>().text = "Connect";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("111");
        if (modbusManager.Connected)
        {
            //Debug.Log("222");
            duration += Time.deltaTime;
            if (duration > UPDATE_PERIOD)
            {
                duration -= UPDATE_PERIOD;

                d1 = !d1;
                modbusManager.WriteCoil(1, d1);
                Debug.Log("Dout1: " + d1);
            }
        }
    }
}
