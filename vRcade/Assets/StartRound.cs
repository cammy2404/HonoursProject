using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRound : MonoBehaviour
{
    [Tooltip("Round length in seconds")]
    public int roundLength = 120;

    public GameObject gameController;
    public GameObject brokenVersion;

    public Vector2 randomRange = new Vector2(0.5f, 1.5f);

    public bool start = false;

    GameObject[] initialObjects;

    void Start()
    {
        initialObjects = new GameObject[transform.childCount];

        for (int i = 0; i < initialObjects.Length; i++)
            initialObjects[i] = transform.GetChild(i).gameObject;

    }

    void Update()
    {
        if (start && !gameController.GetComponent<RoundController>().playing)
        {
            start = false;
            ShatterObject();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "LeftHand" || other.gameObject.tag == "RightHand")
        {
            ShatterObject();
        }
    }

    void ShatterObject()
    {
        DestroyAllChildren(transform);

        GetComponent<BoxCollider>().enabled = false;

        GameObject go = Instantiate(brokenVersion, transform);
        AddForceToChildren(go.transform);
        Destroy(go, 5f);

        gameController.GetComponent<RoundController>().StartRound(roundLength);
    }

    void DestroyAllChildren(Transform trans)
    {
        foreach (Transform t in trans)
            t.gameObject.SetActive(false);
    }

    void AddForceToChildren(Transform trans)
    {
        foreach(Transform t in trans)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.forward * Random.Range(randomRange.x, randomRange.y), ForceMode.Impulse);
        }
    }

    public void ResetButton()
    {
        GetComponent<BoxCollider>().enabled = true;
        
        foreach(GameObject go in initialObjects)
        {
            go.SetActive(true);
        }
    }
}
