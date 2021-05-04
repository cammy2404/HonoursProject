using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public float rotationSpeed;
    public Transform centre;

    Transform rotationCentre;

    void Start()
    {
        if (centre != null)
            rotationCentre = centre;
        else
            rotationCentre = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(rotationCentre.position, transform.up, rotationSpeed * Time.deltaTime);
    }
}
