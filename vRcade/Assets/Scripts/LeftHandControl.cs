using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandControl : MonoBehaviour
{
    public LayerMask grabable;
    
    GameObject grabbableParent;
    GameObject grabbedObject = null;

    Vector3 offset;
    //float angleBetween = 0.0f;

    float itemWeight;

    void Start()
    {
        grabbableParent = GameObject.FindGameObjectWithTag("GrabbableParent");
    }

    void Update()
    {
        if (Input.GetButton("LeftGrab") && grabbedObject != null)
        {
            //grabbedObject.transform.position = transform.position;
            //grabbedObject.transform.rotation = transform.rotation;
            //grabbedObject.transform.position = transform.position + offset;
            //grabbedObject.transform.position = transform.position + (transform.position + offset);
            //grabbedObject.transform.forward += offset;

            //grabbedObject.transform.RotateAround(transform.position, transform.forward, angleBetween);

            //grabbedObject.transform.position = RotatePointAroundPivot(grabbedObject.transform.position, transform.position, Vector3.one);

            grabbedObject.transform.parent = transform;

            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
        }

        if (Input.GetButtonUp("LeftGrab") && grabbedObject != null)
        {
            Debug.LogWarning("THE OBJECT HAS BEEN DROPPED");
            //grabbedObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity;
            grabbedObject.transform.parent = grabbableParent.transform;
            grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            grabbedObject = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (grabbedObject == null)
            if ((grabable.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            {
                grabbedObject = collision.gameObject;
                //offset = grabbedObject.transform.forward - transform.position;
                //angleBetween = Vector3.Angle(transform.forward, offset);
            }
    }

    void OnCollisionExit()
    {
        if (!Input.GetButton("LeftGrab"))
            grabbedObject = null;
    }


    // Got from Guenter123987 on - https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
        //return angles * (point - pivot) + pivot;
    }

}
