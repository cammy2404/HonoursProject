using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollControl : MonoBehaviour
{
    [Header("Ragdoll")]
    public Transform Head;
    public Transform LeftHand;
    public Transform RightHand;

    [Header("Controllers")]
    public Transform Camera;
    Transform RightController;
    Transform LeftController;

    bool connected = false;

    void Update()
    {

        if (connected == false)
        {
            connected = ControllersFound();
            return;
        }

        Head.position = Camera.position;
        LeftHand.position = LeftController.position;
        RightHand.position = RightController.position;
    }

    bool ControllersFound()
    {
        if (RightController == null)
        {
            try
            {
                RightController = GameObject.FindGameObjectWithTag("RightHand").transform;
            }
            catch
            {
                Debug.Log("Right Controller Not Found Yet");
                return false;
            }
        }

        if (LeftController == null)
        {
            try
            {
                LeftController = GameObject.FindGameObjectWithTag("LeftHand").transform;
            }
            catch
            {
                Debug.Log("Left Controller Not Found Yet");
                return false;
            }
        }

        return true;
    }
}
