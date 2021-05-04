using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    public Material lineMaterial;
    public GameObject currentBlock { get; private set; }

    public GameObject defaultBlock;

    bool scriptAddedToHands = false;

    void Start()
    {
        currentBlock = defaultBlock;
    }

    void Update()
    {
        if (scriptAddedToHands == false)
        {
            try
            {
                HandBlockSelector lefthand = GameObject.FindGameObjectWithTag("LeftHand").AddComponent<HandBlockSelector>();
                HandBlockSelector righthand = GameObject.FindGameObjectWithTag("RightHand").AddComponent<HandBlockSelector>();

                lefthand.SetupHand("Left", lineMaterial);
                righthand.SetupHand("Right", lineMaterial);

                scriptAddedToHands = true;

            } catch
            {
                //Debug.LogError("Couldnt find hands");
            }
        }
    }

    public void SetCurrent(GameObject item)
    {
        currentBlock = item;
    }
}
