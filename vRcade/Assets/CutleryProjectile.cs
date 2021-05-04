using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutleryProjectile : MonoBehaviour
{
    public int projectileCount = 10;
    public GameObject[] projectiles;

    bool initComplete = false;

    CannonControl cc;

    void Start()
    {
        cc = GameObject.FindGameObjectWithTag("Cannon").GetComponent<CannonControl>();
    }

    void Update()
    {
        if (!initComplete)
        {
            Vector3 velo = gameObject.GetComponent<Rigidbody>().velocity;
            
            Vector3 throwVector = cc.cannonFront.position - cc.cannonRear.position;

            Vector3 throwVectorNormal = throwVector.normalized;

            for (int i = 0; i < projectileCount; i++)
            {
                GameObject go = Instantiate(projectiles[Random.Range(0, projectiles.Length)], transform, false);
                //Vector3 veloChange = new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), Random.Range(0, 0.1f));
                //Vector3 newVelo = velo + veloChange;
                //Debug.Log(newVelo.normalized);
                //go.GetComponent<Rigidbody>().AddForce(newVelo, ForceMode.Impulse);


                Rigidbody rb = go.GetComponent<Rigidbody>();
                rb.AddForce(cc.force * throwVectorNormal, ForceMode.Impulse);

            }
            initComplete = true;
        }
    }
}
