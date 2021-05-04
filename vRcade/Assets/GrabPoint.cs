using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    public float grabDistance = 1;

    MeshRenderer rend;
    Transform leftHand;
    Transform rightHand;
    bool handsFound = false;

    [Header("Left Hand Values")]
    public Vector3 grabPositionLeft = new Vector3(-0.0296f, -0.0444f, 0.0806f);
    public Vector3 grabRotationLeft = new Vector3(90, 0, 0);

    [Header("Right Hand Values")]
    public Vector3 grabPositionRight = new Vector3(-0.0296f, -0.0444f, 0.0806f);
    public Vector3 grabRotationRight = new Vector3(90, 0, 0);

    [Header("Enable Additional Objects")]
    public bool additionalObjects = false;
    public GameObject[] objects;

    [HideInInspector]
    public bool grabbed;
    [HideInInspector]
    public bool prevGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        prevGrabbed = true;
        SetAdditionalObjectsState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!handsFound)
        {
            try
            {
                leftHand = GameObject.FindGameObjectWithTag("LeftHand").transform;
                rightHand = GameObject.FindGameObjectWithTag("RightHand").transform;

                if (leftHand != null && rightHand != null)
                    handsFound = true;
            }
            catch
            {
                //Debug.Log("Hands not found yet");
            }
        }
        else
        {
            if (leftHand == null || rightHand == null)
            {
                handsFound = false;
                return;
            }

            Vector2 dists = new Vector2();

            // Caclulate distance from grab point to the hands
            float dist = Vector3.Distance(transform.position, leftHand.position);
            dists.x = dist;

            dist = Vector3.Distance(transform.position, rightHand.position);
            dists.y = dist;

            // Render the grab point if the hands are within range
            if (dists.x < grabDistance || dists.y < grabDistance)
            {
                rend.enabled = true;
            }
            else
            {
                rend.enabled = false;
            }

            if (additionalObjects)
            {
                SetAdditionalObjectsState(grabbed);
            }
        }
    }

    // Sets all objects to active
    // Only runs when the state changes to prevent the objects being 
    // set to active every frame when they already are.
    void SetAdditionalObjectsState(bool state)
    {
        if (prevGrabbed != state) 
        { 
            foreach (GameObject go in objects)
            {
                go.SetActive(state);
            }
            prevGrabbed = state;
        }
    }
}
