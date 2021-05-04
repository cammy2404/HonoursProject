using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrab : MonoBehaviour
{

    public string GrabbableTag = "Grabbable";
    public string GrabbableParentTag = "GrabbableParent";
    public int multiplier = 100;
    [Range(0, 1)]
    public int handMode = 0;
    //public Material CollisionColour;

    GameObject grippedParent;
    bool gripped = false;
    GameObject grippedObj = null;
    Vector3 currentGrabbedLocation;
    //Material grippedMat = null;

    string buttonName = "";
    readonly string _LEFTHAND_ = "LeftGrab";
    readonly string _RIGHTHAND = "RightGrab";

    void Start()
    {
        grippedParent = GameObject.FindGameObjectWithTag(GrabbableParentTag);
        buttonName = (handMode == 0) ? _LEFTHAND_ : _RIGHTHAND;
    }


    void Update()
    {
        if (gripped)
            TryLetGo();
        else
            TryGrab();

        if (grippedObj != null)
            currentGrabbedLocation = grippedObj.transform.position;
    }


    void TryGrab()
    {
        if (grippedObj != null)
        {
            if (Input.GetButtonDown(buttonName))
            {
                grippedObj.transform.parent = transform;
                grippedObj.GetComponent<Rigidbody>().isKinematic = true;
                currentGrabbedLocation = grippedObj.transform.position;

                try
                {
                    GrabPoint gp = grippedObj.GetComponentInChildren<GrabPoint>();

                    grippedObj.transform.localPosition = (handMode == 0) ? gp.grabPositionLeft : gp.grabPositionRight;
                    grippedObj.transform.localRotation = Quaternion.Euler((handMode == 0) ? gp.grabRotationLeft : gp.grabRotationRight);

                    gp.grabbed = true;

                } catch (Exception e)
                {
                    // Item grabbed does not have a specific grab position
                }

                gripped = true;
            }
        }
    }

    void TryLetGo()
    {
        if (grippedObj != null)
        {
            if(Input.GetButtonUp(buttonName))
            {
                grippedObj.transform.parent = grippedParent.transform;

                Rigidbody rb = grippedObj.GetComponent<Rigidbody>();

                rb.isKinematic = false;
                
                Vector3 throwVector = grippedObj.transform.position - currentGrabbedLocation;
                //throwVector.Normalize();

                rb.AddForce(throwVector * multiplier, ForceMode.Impulse);

                currentGrabbedLocation = Vector3.zero;
                //grippedObj = null;
                gripped = false;

                try
                {
                    grippedObj.GetComponentInChildren<GrabPoint>().grabbed = false;
                }
                catch {}
            }
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (gripped) return;

        if (other.gameObject.tag == GrabbableTag)
        {
            //if (other.gameObject.GetComponent<GrabPoint>() != null)
            //{
            //    grippedObj = other.gameObject.transform.parent.gameObject;
            //} else
            
            grippedObj = other.gameObject;
            
            //grippedMat = grippedObj.GetComponent<Renderer>().material;
            //grippedObj.GetComponent<Renderer>().material = CollisionColour;
        }
    }


    void OnCollisionExit()
    {
        if (gripped) return;
        
        grippedObj = null;
    }
}


//public void ReleaseGameObject()
//40
//    {
//41
//        // Only throw an object if we're holding onto something
//42
//        if (_grabbedThrowable != null)
//43
//        {
//44
//            _grabbedThrowable.transform.parent = null; // Un-parent throwable object so it doesn't follow the controller
//45
//            Rigidbody rigidBody = _grabbedThrowable.GetComponent<Rigidbody>();
//46
//            rigidBody.isKinematic = false; // Re-enables the physics engine.
//47
//            Vector3 throwVector = _grabbedThrowable.transform.position - _currentGrabbedLocation; // Get the direction that we're throwing
//48
//            rigidBody.AddForce(throwVector* 10, ForceMode.Impulse); // Throws the ball by sending a force
//49
//            _grabbedThrowable = null;
//50
//        }
//51
//    }
