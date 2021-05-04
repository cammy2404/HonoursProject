using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldHandSelection : MonoBehaviour
{
    // Public Variables
    [Header("Highlight")]
    public GameObject highlight;
    public Material lineMaterial;

    [Header("After Selected")]
    public GameObject gravityPlanets;
    public GameObject planetMachine;
    public GameObject gameConfirmer;

    // Local variables
    GameObject playerHead;
    GameObject gamePlanetParent;
    List<GameObject> gamePlanets;

    GameObject rightHand;
    GameObject rightHandHighlight;
    LineRenderer rightLineRenderer;
    GameObject rightClosest;
    readonly string _RIGHTHAND_ = "RightTrigger";
    
    GameObject leftHand;
    GameObject leftHandHighlight;
    LineRenderer leftLineRenderer;
    GameObject leftClosest;
    readonly string _LEFTHAND_ = "LeftTrigger";

    bool handsFound = false;

    public SceneIndexes sceneIndex;
    public bool TestStart = false;


    void Start() {
        gamePlanets = new List<GameObject>();

        playerHead = GameObject.FindWithTag("MainCamera");

        leftHandHighlight = Instantiate(highlight, this.transform);
        leftHandHighlight.GetComponent<Rigidbody>().freezeRotation = true;
        leftHandHighlight.SetActive(false);

        rightHandHighlight = Instantiate(highlight, this.transform);
        rightHandHighlight.GetComponent<Rigidbody>().freezeRotation = true;
        rightHandHighlight.SetActive(false);
    }

    
    void Update()
    {
        if (gamePlanetParent == null)
            gamePlanetParent = GameObject.FindWithTag("GamePlanetParent");

        if (!handsFound)
        {
            try
            {
                rightHand = GameObject.FindGameObjectWithTag("RightHand");
                leftHand = GameObject.FindGameObjectWithTag("LeftHand");

                if (leftHand != null && rightHand != null)
                {
                    handsFound = true;
                    leftLineRenderer = CreateLine(leftHand);
                    rightLineRenderer = CreateLine(rightHand);
                }
            }
            catch
            {
                Debug.Log("Hands not found yet");
            }
        }
        else
        {
            GameObject[] gp = GameObject.FindGameObjectsWithTag("GamePlanet");
            if (gp.Length > 0)
            {
                foreach (GameObject g in gp)
                {
                    if (!gamePlanets.Contains(g))
                        gamePlanets.Add(g);
                }

                if (gamePlanets.Count > 0)
                {
                    leftClosest = HighlightClosest(leftHand, leftHandHighlight, leftLineRenderer);
                    rightClosest = HighlightClosest(rightHand, rightHandHighlight, rightLineRenderer);

                    CheckPressed(_LEFTHAND_, leftClosest);
                    CheckPressed(_RIGHTHAND_, rightClosest);
                }
            }
        }

        if (TestStart)
        {
            TestStart = false;
            GameManager.instance.LoadGame((int)sceneIndex);
        }
    }


    GameObject HighlightClosest(GameObject hand, GameObject highlight, LineRenderer line)
    {
        float closest = float.MaxValue;
        GameObject clostestPlanet = null;
        foreach (GameObject g in gamePlanets)
        {
            Transform planet = g.transform;
            float dist = Vector3.Distance(planet.position, hand.transform.position);
            if (dist < closest)
            {
                closest = dist;
                clostestPlanet = g;
            }
        }

        if (clostestPlanet != null)
        {
            highlight.SetActive(true);
            highlight.transform.position = clostestPlanet.transform.position;
            highlight.transform.LookAt(playerHead.transform, Vector3.up);
            highlight.transform.GetChild(0).GetComponent<TextMesh>().text = clostestPlanet.name;

            line.enabled = true;
            line.SetPosition(0, hand.transform.position);
            line.SetPosition(1, clostestPlanet.transform.position);
        }

        return clostestPlanet;
    }


    LineRenderer CreateLine(GameObject hand)
    {
        LineRenderer line = hand.AddComponent<LineRenderer>();
        line.material = lineMaterial;
        line.startWidth = 0.025f;
        line.endWidth = 0.1f;
        line.numCapVertices = 24;
        line.enabled = false;

        return line;
    }


    void CheckPressed(string hand, GameObject planet)
    {
        if (Input.GetButtonDown(hand) || TestStart)
        {
            Transform selectedPlanet = null;

            string pName = planet.name;
            GameObject gp = Instantiate(gravityPlanets);
            gp.transform.position = gamePlanetParent.transform.position;
            gp.transform.rotation = gamePlanetParent.transform.rotation;

            foreach (Transform t in gp.transform.GetChild(0))
            {
                if (t.gameObject.name.Equals(pName))
                {
                    selectedPlanet = t;
                    selectedPlanet.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }

            leftLineRenderer.enabled = false;
            rightLineRenderer.enabled = false;
            leftHandHighlight.SetActive(false);
            rightHandHighlight.SetActive(false);
            gamePlanetParent.SetActive(false);
            gamePlanets.Clear();

            GameObject gc = Instantiate(gameConfirmer);
            gc.transform.position = planetMachine.transform.position;
            gc.transform.rotation = planetMachine.transform.rotation;

            PlanetConfirmer pc = gc.transform.GetChild(3).GetComponent<PlanetConfirmer>();
            pc.planet = selectedPlanet.gameObject;

            planetMachine.SetActive(false);
            
            MovePlanetToSelection mpts = GetComponent<MovePlanetToSelection>();
            mpts.SetupMovement(selectedPlanet, transform.position, highlight);


        }
    }
}
