using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawner : MonoBehaviour
{
    public bool useCurrentAsRespawn = false;

    [Header("Respawn Location / Rotation")]
    public Vector3 respawnPosition;
    public Vector3 respawnRotation;

    [Header("Particles")]
    public ParticleSystem disapearSystem;
    public ParticleSystem reapearSystem;

    [Header("Range")]
    public float respawnDistance;
    public Vector3 origin;
    public bool useObjectPosition = false;
    public Transform originObject;

    Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        if (useCurrentAsRespawn)
        {
            respawnPosition = transform.position;
            respawnRotation = transform.rotation.eulerAngles;
        }
    }

    
    void Update()
    {
        if (useObjectPosition) origin = originObject.position;

        if (Vector3.Distance(transform.position, origin) > respawnDistance)
        {
            Instantiate(disapearSystem, transform.position, transform.rotation);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.rotation = Quaternion.Euler(respawnRotation);
            transform.position = respawnPosition;

            Instantiate(reapearSystem, transform.position, transform.rotation);
        }
    }
}
