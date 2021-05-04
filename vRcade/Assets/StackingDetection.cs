using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackingDetection : MonoBehaviour
{
    public Vector3Int resolution = new Vector3Int(5, 3, 5);
    public float threshold;

    public bool stacked = false;

    Mesh mesh;
    Vector3[] vertices;

    Vector3 size;
    Matrix4x4 localToWorld;

    void Start()
    {
        size = GetComponentInChildren<Collider>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        vertices = GetVertices();
        RaycastHit hit;

        float closestPoint = float.MaxValue;
        for (int i = 0; i < vertices.Length; i++)
        {
            try
            {
                Physics.Raycast(vertices[i], -Vector3.up, out hit, 2);

                if (hit.transform.gameObject == gameObject) break;

                float rayDist = Vector3.Distance(vertices[i], hit.point);

                closestPoint = (rayDist < closestPoint) ? rayDist : closestPoint;

                Color c;
                if (closestPoint <= threshold) c = Color.blue;
                else c = Color.red;

                Debug.DrawLine(vertices[i], hit.point, c);
            }
            catch { };
        }

        if (closestPoint <= threshold) stacked = true;
        else stacked = false;
    }

    Vector3[] GetVertices()
    {
        List<Vector3> vList = new List<Vector3>();
        Vector3 pos = transform.position;
        localToWorld = transform.localToWorldMatrix;

        float xStep = size.x / (resolution.x - 1);
        float yStep = size.y / (resolution.y - 1);
        float zStep = size.z / (resolution.z - 1);

        for (int y = 0; y < resolution.y; y++)
            for (int x = 0; x < resolution.x; x++)
                for (int z = 0; z < resolution.z; z++)
                {
                    Vector3 newPos = new Vector3((x * xStep) - (size.x / 2), (y * yStep), (z * zStep) - (size.z / 2));
                    vList.Add(localToWorld.MultiplyPoint3x4(newPos));
                }
        

        return vList.ToArray();
    }
}


//// Bottom Centre
//Vector3 newVal = Vector3.zero;
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));


//// Bottom Corners
//newVal = new Vector3((size.x), yPos, (size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3(-(size.x), yPos, -(size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3((size.x), yPos, -(size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3(-(size.x), yPos, (size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));


//// Bottom Midpoints
//newVal = new Vector3(0, yPos, (size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3((size.x / 2), yPos, (size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3(-(size.x / 2), yPos, (size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3(0, yPos, -(size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3((size.x / 2), yPos, -(size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3(-(size.x / 2), yPos, -(size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));


//// Top
//newVal = new Vector3((size.x), size.y, (size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3(-(size.x), size.y, -(size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3((size.x), size.y, -(size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));

//newVal = new Vector3(-(size.x), size.y, (size.z));
//vList.Add(localToWorld.MultiplyPoint3x4(newVal));