using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{
    public GameObject[] Cameras;
    private Transform[] CameraParents;

    [Tooltip("The index of the camera array to start at.")]
    public int startingCamera = 0;

    [Range(0, 1)]
    public float cameraPanSpeed = 0.1f;
    [Range(0, 1)]
    public float cameraPitchSpeed = 0.1f;
    [Range(0, 1)]
    public float cameraZoomSpeed = 0.1f;

    private int currentCamera;


    BluetoothConnection blueC;
    int joystickMax = 1024;
    int joystickNewRange = 10;

    bool currChange = false;
    bool prevChange = false;

    float pan;
    float pitch;

    int zoomMin = 10;
    int zoomMax = 100;
    float zoom;


    void Start()
    {

        CameraParents = new Transform[Cameras.Length];
        for (int i = 0; i < CameraParents.Length; i++)
        {
            CameraParents[i] = Cameras[i].transform.parent.parent.parent.transform;
        }

        blueC = gameObject.GetComponent<BluetoothConnection>();

        changeCamera(true);

        currentCamera = 0;
    }


    void Update()
    {
        if (blueC.CheckActive())
        {
            // Change Camera
            currChange = blueC.butTop;
            // Ensures that the function is only called once, even if the button is held down
            if (currChange && (currChange != prevChange))
            {
                changeCamera();
            }
            prevChange = currChange;

            // Pan Camera
            pan = Map(blueC.joyTopRightX, 0, joystickMax, -joystickNewRange, joystickNewRange);
            PanCamera(pan);

            // Pitch Camera
            pitch = Map(blueC.joyTopRightY, 0, joystickMax, -joystickNewRange, joystickNewRange);
            PitchCamera(-pitch);

            // Change Camera Zoom
            zoom = Map(blueC.joyTopLeftY, 0, joystickMax, -joystickNewRange, joystickNewRange);
            ajustZoomLevel(zoom);
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
        float zoom = cam.fieldOfView;

        zoom += -value * cameraZoomSpeed;

        if (zoom > zoomMax)
            zoom = zoomMax;

        if (zoom < zoomMin)
            zoom = zoomMin;

        cam.fieldOfView = zoom;
    }

    void PanCamera(float value)
    {
        Transform camParent = CameraParents[currentCamera];

        float angle = value * cameraPanSpeed;

        camParent.RotateAround(camParent.position, Vector3.up, angle);
    }

    void PitchCamera(float value)
    {
        Transform camParent = CameraParents[currentCamera];

        float angle = value * cameraPitchSpeed;

        camParent.RotateAround(camParent.position, camParent.right, angle);
    }

    float Map(int value, int min, int max, int newMin, int newMax, bool cap = true)
    {
        int range = max - min;

        float percentage = (float)value / range;

        if (cap && percentage > 1.0f)
            percentage = 1.0f;

        range = newMax - newMin;

        float newValue = percentage * range;

        newValue += newMin;

        return newValue;
    }
}
