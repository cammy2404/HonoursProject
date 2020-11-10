using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    [Header("Cannon")]
    public Transform cannonVerticalRotationPoint;
    public Transform cannonHorizontalRotationPoint;
    public float dampening = 250;
    public float rotationSpeed = 25;
    public float upCap = 0;
    public float downCap = 0;
    public float leftCap = 0;
    public float rightCap = 0;

    [Header("Cannon Ball")]
    public GameObject cannonBallPrefab;
    public Transform cannonFront;
    public Transform cannonRear;
    public float force;

    float desiredVertRot;
    float desiredHorRot;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.U))
        {
            desiredVertRot += rotationSpeed * Time.deltaTime;
            if (desiredVertRot > upCap) desiredVertRot = upCap;
        }

        if (Input.GetKey(KeyCode.J))
        {
            desiredVertRot -= rotationSpeed * Time.deltaTime;
            if (desiredVertRot < downCap) desiredVertRot = downCap;
        }

        if (Input.GetKey(KeyCode.H))
        {
            desiredHorRot -= rotationSpeed * Time.deltaTime;
            if (desiredHorRot < leftCap) desiredHorRot = leftCap;
        }

        if (Input.GetKey(KeyCode.K))
        {
            desiredHorRot += rotationSpeed * Time.deltaTime;
            if (desiredHorRot > rightCap) desiredHorRot = rightCap;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            CreateCannonBall();
        }


        Quaternion desiredVertRotQ = Quaternion.Euler(desiredVertRot, cannonVerticalRotationPoint.eulerAngles.y, cannonVerticalRotationPoint.eulerAngles.z);
        cannonVerticalRotationPoint.rotation = Quaternion.Lerp(transform.rotation, desiredVertRotQ, Time.deltaTime * dampening);
        
        Quaternion desiredHorRotQ = Quaternion.Euler(cannonHorizontalRotationPoint.eulerAngles.x, desiredHorRot, cannonHorizontalRotationPoint.eulerAngles.z);
        cannonHorizontalRotationPoint.rotation = Quaternion.Lerp(transform.rotation, desiredHorRotQ, Time.deltaTime * dampening);
    }

    void CreateCannonBall()
    {
        GameObject go = Instantiate(cannonBallPrefab);
        go.transform.position = cannonFront.position;

        Vector3 throwVector = cannonFront.position - cannonRear.position;

        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.AddForce(force * throwVector, ForceMode.Impulse);
    }

}