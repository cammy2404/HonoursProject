using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public BluetoothConnection bluetoothConnection;

    public Image buttonTop;
    public Image buttonLeft;
    public Image buttonRight;
    public Image buttonMiddle;

    [Header("Stack Em")]
    public bool stackEm = false;
    public Text topScoreOutput;
    public Text currentScoreOutput;

    public GameObject heightDetection;

    HeightDetection score;

    // Start is called before the first frame update
    void Start()
    {
        score = heightDetection.GetComponent<HeightDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stackEm)
        {
            topScoreOutput.text = score.score.ToString();
            currentScoreOutput.text = score.currScore.ToString();
        }
    }

}
