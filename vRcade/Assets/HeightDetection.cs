using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightDetection : MonoBehaviour
{
    [Header("Range")]
    public float left = -0.85f;
    public float right = 1.01f;
    public float top = 0.66f;
    public float bottom = 0.11f;

    [Header("Raycasts")]
    [Min(0)]
    public int xCount;
    [Min(0)]
    public int yCount;
    [Range(0, 2)]
    [Tooltip("0 - No raycasts displayed. \n1 - Show all raycasts. \n2 - Only show hit raycasts")]
    public int rayView = 0;

    Transform currentHeightObj;
    Transform maxHeightObj;

    float width, height;
    float wStep, hStep;
    Vector3 origin;

    float maxHeight;

    float shortestRayCurr;
    float shortestRay;

    [HideInInspector]
    public int score;
    [HideInInspector]
    public int currScore;
    [Min(1)]
    public int scoreMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        maxHeight = origin.y;

        width = (left - right);
        height = (top - bottom);

        wStep = width / (xCount - 1);
        hStep = height / (yCount - 1);

        shortestRay = maxHeight;

        currentHeightObj = gameObject.transform.GetChild(0);
        maxHeightObj = gameObject.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Vector3 pos;
        pos.y = maxHeight;

        shortestRayCurr = maxHeight;

        for (int i = 0; i < xCount; i++) 
        {
            pos.x = left - (i * wStep);

            for (int j = 0; j < yCount; j++)
            {
                pos.z = bottom + (j * hStep);

                Physics.Raycast(pos, -Vector3.up, out hit, maxHeight);

                if (hit.transform.tag == "LeftHand" || hit.transform.tag == "RightHand") break;

                try
                {
                    if (hit.transform.GetComponent<StackingDetection>().stacked)
                    {

                        float rayDist = Vector3.Distance(pos, hit.point);

                        shortestRayCurr = (rayDist < shortestRayCurr) ? rayDist : shortestRayCurr;
                        shortestRay = (rayDist < shortestRay) ? rayDist : shortestRay;

                        switch (rayView)
                        {
                            case 0:
                                // Dont display any raycasts
                                break;

                            case 1:
                                // Show all raycasts but with different colours
                                Color c;
                                if (hit.transform.tag == "Grabbable")
                                    c = Color.blue;
                                else
                                    c = Color.red;
                                Debug.DrawLine(pos, hit.point, c);
                                break;

                            case 2:
                                // Show only hit raycasts
                                if (hit.transform.tag == "Grabbable")
                                    Debug.DrawLine(pos, hit.point, Color.blue);
                                break;

                            default:
                                Debug.LogError("Unexpected value in raycast for HeightDetection");
                                break;
                        }
                    }
                }
                catch { };
            }
        }

        currentHeightObj.position = new Vector3(origin.x, (maxHeight - shortestRayCurr), origin.z);
        maxHeightObj.position = new Vector3(origin.x, (maxHeight - shortestRay), origin.z);

        currScore = (int)((maxHeight - shortestRayCurr) * scoreMultiplier);
        score = (int)((maxHeight - shortestRay) * scoreMultiplier);
    }


    public void ResetHeights()
    {
        shortestRay = maxHeight;
        shortestRayCurr = maxHeight;
    }
}
