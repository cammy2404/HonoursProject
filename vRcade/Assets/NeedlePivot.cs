using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class NeedlePivot : MonoBehaviour
{
    public bool active = false;

    [Header("Rotation")]
    [Min(2)]
    public int speedFactor = 2;

    [Min(10)]
    public int smoothFactor = 100;

    [Header("Segment Values")]
    [Min(0)]
    [Tooltip("All values are positive and are negated to create the equal bounds")]
    public float greenSegment = 15.5f;
    [Min(0)]
    public float greenForce = 2;
    [Min(0)]
    [Tooltip("All values are positive and are negated to create the equal bounds")]
    public float yellowSegment = 47.5f;
    [Min(0)]
    public float yellowForce = 1.5f;
    [Min(0)]
    [Tooltip("All values are positive and are negated to create the equal bounds")]
    public float redSegment = 70.0f;
    [Min(0)]
    public float redForce = 1;

    [Header("Input")]
    [Range(0, 3)]
    public float inputPause = 0.5f;
    public BluetoothConnection bluetoothConnection;

    [Header("Output")]
    public GameObject ball;
    [Tooltip("The point from the launcher where the ball will be fired from")]
    public Transform launchPoint;

    
    int rotationValue;

    float[] x;
    int index = 0;

    bool running = true;

    RectTransform needleRect;

    void Start()
    {
        List<float> xList = new List<float>();

        // Calculate the step and stop values;
        float step = Mathf.PI / smoothFactor;
        float stop = Mathf.PI * (2 * speedFactor);
        
        // Add initial value
        xList.Add(0);

        // Loop until last value is equal to the stop value
        do
        {
            // Calculate the previous value + the step and add that new value as a new element
            xList.Add(xList[xList.Count - 1] + step);
        } while (xList[xList.Count - 1] <= stop);

        x = xList.ToArray();

        needleRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            rotationValue = (int)(70 * Mathf.Sin(x[index] / speedFactor));

            needleRect.rotation = Quaternion.Euler(0, 0, rotationValue);

            index++;
            index %= x.Length;

            if (bluetoothConnection.CheckActive() || active)
            {
                if (bluetoothConnection.butMid || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("LeftTrigger"))
                {
                    Fire(GetSegment());
                    StartCoroutine(Pause());
                }
            }
        }
    }


    public float GetSegment()
    {
        if (rotationValue < greenSegment && rotationValue > -greenSegment)
        {
            return greenForce;
        } else if (rotationValue < yellowSegment && rotationValue > -yellowSegment)
        {
            return yellowForce;
        } else
        {
            return redForce;
        }
    }


    void Fire(float fireForce)
    {
        GameObject go = Instantiate(ball);
        go.transform.position = launchPoint.position;
        go.transform.rotation = launchPoint.rotation;

        Rigidbody rb = go.GetComponent<Rigidbody>();
        Vector3 force = go.transform.forward.normalized;
        rb.AddForce(fireForce * force, ForceMode.Impulse);
    }


    IEnumerator Pause()
    {
        running = false;
        yield return new WaitForSeconds(inputPause);
        running = true;
    }
}
