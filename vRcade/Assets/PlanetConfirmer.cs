using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetConfirmer : MonoBehaviour
{
    public GameObject linkedButton;

    [HideInInspector]
    public GameObject planet;

    Button button;
    bool butPressedDown = false;

    void Start()
    {
        button = linkedButton.GetComponent<Button>();
    }

    void Update()
    {
        if (button.pressed && !butPressedDown)
        {
            butPressedDown = true;

            GameManager.instance.LoadGame((int)planet.GetComponent<GameLocation>().index);
        }

        if (!button.pressed && butPressedDown)
        {
            butPressedDown = false;
        }
    }
}
