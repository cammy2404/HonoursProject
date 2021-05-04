using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBlockSelector : MonoBehaviour
{
    string handName = "";
    int distance;
    Material lineMaterial;

    string in_trigger = "Trigger";

    RaycastHit hit;
    LineRenderer line;
    BlockControl control;

    Transform indexFinger;

    public void SetupHand(string hName, Material mat, int dist = 5)
    {
        handName = hName;
        lineMaterial = mat;
        distance = dist;

        in_trigger = handName + in_trigger;

        line = gameObject.AddComponent<LineRenderer>();
        line.material = lineMaterial;
        line.startWidth = 0.025f;
        line.endWidth = 0.05f;
        line.numCapVertices = 24;
        line.enabled = false;

        control = GameObject.FindGameObjectWithTag("BlockSelector").GetComponent<BlockControl>();

        indexFinger = transform.GetChild(0).GetChild(1).transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = indexFinger.right;
        if (handName == "Left") dir *= -1;

        if (Physics.Raycast(indexFinger.position, dir, out hit, distance))
        {
            Debug.DrawRay(indexFinger.position, dir * hit.distance, Color.yellow);

            if (hit.transform.tag.Equals("BlockSelectorItem"))
            {
                //Debug.Log("Hit something important :: " + hit.transform.name);

                line.SetPosition(0, indexFinger.position);
                line.SetPosition(1, hit.point);
                line.enabled = true;

                if (Input.GetButtonDown(in_trigger))
                {
                    ItemHolder item = hit.transform.GetComponent<ItemHolder>();
                    control.SetCurrent(item.prefab);
                }

            } else
            {
                line.enabled = false;
            }
        } else
        {
            Debug.DrawRay(indexFinger.position, dir * distance, Color.red);
            line.enabled = false;
        }
    }
}
