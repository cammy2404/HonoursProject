using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class BluetoothConnection : MonoBehaviour
{
    private SerialPort port = null;

    public string portName = "COM3";

    //[Header("Test Data to Send")]
    //public string command;
    //public string request;
    //public string values;

    [Header("Received Data")]
    public bool receiveData = true; 
    
    //[Header("Camera")]
    //public int zoom;
    //public int pan;
    //public int pitch;

    [Header("Controller")]
    // Single Buttons
    public bool butLeft;
    public bool butMid;
    public bool butRight;
    public bool butTop;
    // Joystick Left
    public int joyLeftX = 512;
    public int joyLeftY = 512;
    public int joyLeftS;
    // Joystick Right
    public int joyRightX = 512;
    public int joyRightY = 512;
    public int joyRightS;
    // Joystick Top Left
    public int joyTopLeftX = 512;
    public int joyTopLeftY = 512;
    public int joyTopLeftS;
    // Joystick Top Right
    public int joyTopRightX = 512;
    public int joyTopRightY = 512;
    public int joyTopRightS;




    int baudRate = 9600;
    int readTimeOut = 100;
    int bufferSize = 32;
    bool programActive = true;

    Thread thread;
    System.Random rand;

    void Start()
    {
        try
        {
            port = new SerialPort();

            port.PortName = portName;
            port.BaudRate = baudRate;
            port.ReadTimeout = readTimeOut;
            port.WriteTimeout = readTimeOut;
            port.Open();
            Debug.Log("Successfully Connected");
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        if (port.IsOpen)
        {
            thread = new Thread(new ThreadStart(ProcessData));
            thread.Start();
        }
    }

    public void OnDisable()
    {
        programActive = false;
        if (port != null && port.IsOpen)
        {
            port.Close();
        }
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        //    SendData(command + "+" + request + "=" + values + ";");

    }

    //void SendData(string data)
    //{
    //    if (port.IsOpen)
    //    {
    //        try
    //        {
    //            port.WriteLine(data);
    //        }
    //        catch (TimeoutException e)
    //        {
    //            Debug.LogError(e.Message);
    //        }
    //    } else
    //    {
    //        Debug.LogWarning("Data not sent as port is not open");
    //    }
    //}


    void ProcessData()
    {
        byte[] buffer = new byte[bufferSize];

        if (receiveData)
        {
            Debug.Log("Starting to process data");
            while (programActive)
            {
                try
                {
                    string str = port.ReadLine();

                    if (str.Length > 0)
                    {
                        //Debug.Log(str);
                        int seperator = str.IndexOf('&');
                        string command = str.Substring(0, seperator);
                        string data = str.Substring(seperator + 1, str.Length - (seperator + 1));
                        switch (command)
                        {
                            case "buttons":
                                ProcessButtons(data);
                                break;

                            case "joysticks":
                                ProcessJoysticks(data);
                                break;

                            default:
                                Debug.LogError("Command --> " + command + " : not recognised");
                                break;
                        }
                    }
                }
                catch (TimeoutException e)
                {
                    Debug.LogWarning(e.Message);
                }
                catch (NullReferenceException)
                {
                    Debug.LogWarning("Null Reference Exception -- Not Sure why...");
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Exception Caught:: " + e.Message);
                }
            }
            Debug.Log("Thread Stopped");
        }
    }

    void ProcessButtons(string data)
    {
        //Debug.Log(data);
        string[] dataPoints = data.Split('&');

        foreach (string point in dataPoints)
        {
            string[] values = point.Split(':');
            switch(values[0])
            {
                case "butLeft":
                    butLeft = (int.Parse(values[1]) == 1);
                    break;

                case "butMid":
                    butMid = (int.Parse(values[1]) == 1);
                    break;

                case "butRight":
                    butRight = (int.Parse(values[1]) == 1);
                    break;

                case "butTop":
                    butTop = (int.Parse(values[1]) == 1);
                    break;

                default:
                    Debug.LogError("Data Point --> Buttons:" + values[0] + " not recognised");
                    break;
            }
        }

    }

    void ProcessJoysticks(string data)
    {
        string[] dataPoints = data.Split('&');

        foreach (string point in dataPoints)
        {
            string[] values = point.Split(':');
            switch (values[0])
            {
                case "joyLeftX":
                    joyLeftX = int.Parse(values[1]);
                    break;

                case "joyLeftY":
                    joyLeftY = int.Parse(values[1]);
                    break;

                case "joyLeftS":
                    joyLeftS = int.Parse(values[1]);
                    break;


                case "joyRightX":
                    joyRightX = int.Parse(values[1]);
                    break;

                case "joyRightY":
                    joyRightY = int.Parse(values[1]);
                    break;

                case "joyRightS":
                    joyRightS = int.Parse(values[1]);
                    break;


                case "joyTopLeftX":
                    joyTopLeftX = int.Parse(values[1]);
                    break;

                case "joyTopLeftY":
                    joyTopLeftY = int.Parse(values[1]);
                    break;

                case "joyTopLeftS":
                    joyTopLeftS = int.Parse(values[1]);
                    break;


                case "joyTopRightX":
                    joyTopRightX = int.Parse(values[1]);
                    break;

                case "joyTopRightY":
                    joyTopRightY = int.Parse(values[1]);
                    break;

                case "joyTopRightS":
                    joyTopRightS = int.Parse(values[1]);
                    break;


                default:
                    Debug.LogError("Data Point --> Joystick:" + values[0] + " not recognised");
                    break;
            }
        }
    }

    public bool CheckActive()
    {
        return port.IsOpen;
    }

}

