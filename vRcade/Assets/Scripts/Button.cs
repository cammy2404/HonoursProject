using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public float endstop;
    public float pressedStop;
    public bool oneTime = false;

    public bool pressed {
        get
        {
            return butPressed;
        }
    }

    Transform button;
    Vector3 startPos;
    bool butPressed = false;
    bool canPress = true;

    void Start()
    {
        button = transform;
        startPos = button.position;
    }

    void Update()
    {
        float currOffset = startPos.y - button.position.y;

        // Constrain the button pressed down
        if (currOffset > endstop)
            button.position = new Vector3(button.position.x, startPos.y - endstop, button.position.z);

        // Constrain the button from moving up
        if (button.position.y > startPos.y)
            button.position = new Vector3(button.position.x, startPos.y, button.position.z);

        
        // Check if the button is pressed far enough down to consider it pressed
        if (canPress)
            if (currOffset > pressedStop)
                butPressed = true;
            else
                butPressed = false;

        if (butPressed && oneTime)
        {
            canPress = false;
        }
    }
}