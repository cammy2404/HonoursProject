using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{
    public GameObject[] Cameras;
    private Transform[] CameraParents;

    public int startingCamera = 0;

    public float cameraPanSpeed = 0.5f;
    public float cameraTiltSpeed = 0.5f;
    public float cameraZoomSpeed = 1f;

    private int currentCamera = 0;

    SerialPort stream;
    public string comPort = "COM3";
    public int baudRate = 9600;
    
    string input;

    bool isOpen = false;


    void Start()
    {
        stream = new SerialPort(comPort, baudRate);
        stream.ReadTimeout = 50;
        try
        {
            stream.Open();
            isOpen = true;
        }
        catch (Exception)
        {
            Debug.LogWarning(comPort + " is not open/available");
        }

        CameraParents = new Transform[Cameras.Length];
        for (int i = 0; i < CameraParents.Length; i++)
        {
            CameraParents[i] = Cameras[i].transform.parent.parent.parent.transform;
        }

        changeCamera(true);
    }


    void Update()
    {
        if (isOpen)
            try
            {
                if (stream.BytesToRead > 0)
                {
                    input = stream.ReadLine().ToString().Trim();

                    string[] inputSplit = input.Split('-');

                    foreach (string s in inputSplit)
                    {

                        int i = int.Parse(s.Substring(0, 1).ToString().Trim());
                        float v = float.Parse(s.Substring(1, s.Length - 1).ToString().Trim());

                        switch (i)
                        {
                            case 1:
                                if (v == 1)
                                    changeCamera();
                                break;

                            case 2:
                                TiltCamera(v);
                                break;

                            case 3:
                                PanCamera(v);
                                break;

                            case 4:
                                ajustZoomLevel(v);
                                break;

                            case 5:
                                ajustZoomLevel(v);
                                break;

                            default:
                                Debug.LogError("Value passed is not recognised");
                                break;
                        }

                    }

                    stream.DiscardInBuffer();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
    }

    void changeCamera(bool start = false)
    {
        currentCamera = (currentCamera + 1) % Cameras.Length;

        int cam = (start) ? startingCamera : currentCamera;

        foreach (GameObject c in Cameras)
        {
            c.SetActive(false);
        }
        Cameras[cam].SetActive(true);
    }

    void ajustZoomLevel(float value)
    {
        Camera cam = Cameras[currentCamera].GetComponent<Camera>();

        float zoomLevel = (value == 0) ? 0f : ((value == 1) ? -cameraZoomSpeed : cameraZoomSpeed);

        cam.fieldOfView += zoomLevel;
    }

    void PanCamera(float value)
    {
        Transform camParent = CameraParents[currentCamera];

        float angle = (value == 0) ? 0f : ((value == 1) ? cameraPanSpeed : -cameraPanSpeed);

        camParent.RotateAround(camParent.position, camParent.up, angle);
    }

    void TiltCamera(float value)
    {
        Transform camParent = CameraParents[currentCamera];

        float angle = (value == 0) ? 0f : ((value == 1) ? cameraTiltSpeed : -cameraTiltSpeed);

        camParent.RotateAround(camParent.position, camParent.right, angle);
    }
}
