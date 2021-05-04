using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, new Vector3(0, CalculateTragectoryAtX(1, 2, 45, 9.8f, 4), 0));

        //Debug.Log(CalculateTragectoryAtX(1, 2, 45, 9.8f, 4));

        //Vector3[] coords = new Vector3[10];

        //for (int i = 0; i < coords.Length; i++)
        //{
        //    float y = CalculateTragectoryAtX(i / 10, 2, 45, 9.8f, 4);
        //    coords[i] = new Vector3(i, y, i);
        //}

        //for (int i = 1; i < coords.Length; i++)
        //{
        //    Gizmos.DrawLine(coords[i - 1], coords[i]);
        //}
    }




    // Tragectory 
    // y = h + x * tan(α) - g * x² / 2 * V₀² * cos²(α)

    float CalculateTragectoryAtX(float x, float height, float alpha, float gravity, float velocity)
    {
        float y = height + x * Mathf.Tan(alpha) - gravity * Mathf.Pow(x, 2) / 2 * Mathf.Pow(velocity, 2) * Mathf.Acos(alpha);
        return y;
    }
}
