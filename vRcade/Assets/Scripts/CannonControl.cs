using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    [Header("Cannon")]
    public Transform cannonVerticalRotationPoint;
    public Transform cannonHorizontalRotationPoint;
    public float dampening = 250;
    public float rotationSpeed = 2.5f;
    public float upcap = 10;
    public float downcap = -10;
    public float leftcap = -10;
    public float rightcap = 10;

    float desiredVertRot;
    float desiredHorRot;

    [Header("Cannon Ball")]
    public GameObject[] cannonBallPrefab;
    public Transform cannonFront;
    public Transform cannonRear;
    public float force;

    int projectileIndex = 0;

    BluetoothConnection blueC;
    int joystickMax = 1024;
    int joystickNewRange = 10;

    bool butt1Curr = false;
    bool butt1Prev = false;

    bool butt2Curr = false;
    bool butt2Prev = false;

    bool butt3Curr = false;
    bool butt3Prev = false;

    float pan;
    float pitch;

    float currPan = 0;
    float currPitch = 0;

    void Start()
    {
        blueC = GameObject.FindGameObjectWithTag("GameController").GetComponent<BluetoothConnection>();
    }

    void Update()
    {
        if (blueC.CheckActive())
        {
            Buttons();

            pan = Map(blueC.joyLeftX, 0, joystickMax, -joystickNewRange, joystickNewRange);
            pitch = Map(blueC.joyLeftY, 0, joystickMax, -joystickNewRange, joystickNewRange);

            RotateCannon(pan, pitch);
        }
    }


    void Buttons()
    {
        // Ensures that the function is only called once, even if the button is held down

        butt1Curr = blueC.butLeft;
        if (butt1Curr && (butt1Curr != butt1Prev))
        {
            IncreaseProjectileIndex(1);
        }
        butt1Prev = butt1Curr;

        butt2Curr = blueC.butRight;
        if (butt2Curr && (butt2Curr != butt2Prev))
        {
            IncreaseProjectileIndex(-1);
        }
        butt2Prev = butt2Curr;

        butt3Curr = blueC.butMid;
        if (butt3Curr && (butt3Curr != butt3Prev))
        {
            CreateCannonBall();
        }
        butt3Prev = butt3Curr;

    }

    void RotateCannon(float panValue, float pitchValue)
    {
        desiredHorRot += panValue * rotationSpeed * Time.deltaTime;
        if (desiredHorRot < leftcap) desiredHorRot = leftcap;
        if (desiredHorRot > rightcap) desiredHorRot = rightcap;

        desiredVertRot += pitchValue * rotationSpeed * Time.deltaTime;
        if (desiredVertRot < downcap) desiredVertRot = downcap;
        if (desiredVertRot > upcap) desiredVertRot = upcap;


        Quaternion desiredVertRotQ = Quaternion.Euler(desiredVertRot, cannonVerticalRotationPoint.eulerAngles.y, cannonVerticalRotationPoint.eulerAngles.z);
        cannonVerticalRotationPoint.rotation = Quaternion.Lerp(transform.rotation, desiredVertRotQ, Time.deltaTime * dampening);

        Quaternion desiredHorRotQ = Quaternion.Euler(cannonHorizontalRotationPoint.eulerAngles.x, desiredHorRot, cannonHorizontalRotationPoint.eulerAngles.z);
        cannonHorizontalRotationPoint.rotation = Quaternion.Lerp(transform.rotation, desiredHorRotQ, Time.deltaTime * dampening);
    }


    void CreateCannonBall()
    {
        GameObject go = Instantiate(cannonBallPrefab[projectileIndex]);
        go.transform.position = cannonFront.position;

        Vector3 throwVector = cannonFront.position - cannonRear.position;

        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.AddForce(force * throwVector, ForceMode.Impulse);
    }

    void IncreaseProjectileIndex(int inc)
    {
        projectileIndex = (projectileIndex + 1) % cannonBallPrefab.Length;
        Debug.Log(projectileIndex);
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