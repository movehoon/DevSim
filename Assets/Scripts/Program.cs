using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Program : MonoBehaviour
{
    public Transform sphere;
    public ModbusManager modbusManager;

    int direction_command;

    public Dropdown dropDown_PortNames;
    public Button button_Connect;

    public GameObject LeftLight;
    public GameObject RightLight;

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
        LeftLight.SetActive(hit);
    }

    public void OnHitRight(bool hit)
    {
        Debug.Log("OnHitRight: " + hit.ToString());
        modbusManager.WriteCoil(1, hit);
        RightLight.SetActive(hit);
    }

    public void OnConnect()
    {
        if (!modbusManager.Connected)
        {
            modbusManager.Connect("192.168.1.2");
            //string portName = dropDown_PortNames.options[dropDown_PortNames.value].text;
            //if (modbusManager.Connect(portName))
            //{
            //    Debug.Log("Modbus Connected with " + portName);
            //    button_Connect.GetComponentInChildren<Text>().text = "Disconnect";
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

    public void OnRead()
    {
        modbusManager.ReadState();
    }

    static CountdownEvent countdown;
    static int upCount = 0;
    static object lockObj = new object();
    const bool resolveNames = true;
    static void p_PingCompleted(object sender, PingCompletedEventArgs e)
    {
        string ip = (string)e.UserState;
        if (e.Reply != null && e.Reply.Status == IPStatus.Success)
        {
            if (resolveNames)
            {
                string name;
                try
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                    name = hostEntry.HostName;
                }
                catch (SocketException ex)
                {
                    name = "?";
                }
                Debug.Log(string.Format("{0} ({1}) is up: ({2} ms)", ip, name, e.Reply.RoundtripTime));
            }
            else
            {
                Debug.Log(string.Format("{0} is up: ({1} ms)", ip, e.Reply.RoundtripTime));
            }
            lock (lockObj)
            {
                upCount++;
            }
        }
        else if (e.Reply == null)
        {
            Debug.Log(string.Format("Pinging {0} failed. (Null Reply object?)", ip));
        }
    }

    public static string getIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
            }
        }
        return localIP;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (modbusManager.modbusInterface == ModbusInterface.RTU)
        {
            string[] portNames = SerialPort.GetPortNames();
            dropDown_PortNames.ClearOptions();
            foreach (string portName in portNames)
            {
                dropDown_PortNames.options.Add(new Dropdown.OptionData() { text = portName });
            }
        }
        else if (modbusManager.modbusInterface == ModbusInterface.TCP)
        {
            dropDown_PortNames.ClearOptions();

            Debug.Log(getIPAddress());
            string ipBase = getIPAddress();
            string[] ipParts = ipBase.Split('.');
            ipBase = ipParts[0] + "." + ipParts[1] + "." + ipParts[2] + ".";
            for (int i = 1; i < 255; i++)
            {
                string ip = ipBase + i.ToString();
                StartCoroutine(StartPing(ip));
                //Ping p = new Ping();
                //p.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
                //p.SendAsync(ip, 100, ip);
            }
        }
    }

    IEnumerator StartPing(string address)
    {
        WaitForSeconds f = new WaitForSeconds(0.05f);
        UnityEngine.Ping p = new UnityEngine.Ping(address);
        while (p.isDone == false)
        {
            yield return f;
        }
        dropDown_PortNames.options.Add(new Dropdown.OptionData() { text = address });
        //PingFinished(p);
    }


    // Update is called once per frame
    void Update()
    {
        //if (modbusManager.Connected)
        //{
        //    if (modbusManager.ReadCoil(0))
        //    {
        //        direction_command = -1;
        //    }
        //    else if (modbusManager.ReadCoil(1))
        //    {
        //        direction_command = 1;
        //    }
        //    else
        //    {
        //        direction_command = 0;
        //    }
        //}

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

            if (modbusManager.Connected)
            {
                ushort position = Convert.ToUInt16((sphere.transform.localPosition.x + 5.0f) * 204.8f);
                modbusManager.WriteRegister(0, position);
                Debug.Log("position: " + position.ToString());
            }
        }
    }
}
