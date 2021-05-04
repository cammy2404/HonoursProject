using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlanetToSelection : MonoBehaviour
{
    [Range(0.1f, 10)]
    public float speed = 2.5f;

    Transform movePlanet = null;

    Vector3 position;
    Vector3 startPosition;

    GameObject highlighter;

    bool move = false;
    float t = 0;

    // Update is called once per frame
    void Update()
    {
        if (move) {
            t += Time.deltaTime / speed;
            movePlanet.position = Vector3.Lerp(startPosition, position, t);

            if (movePlanet.position == position)
            {
                move = false;
                GameObject hl = Instantiate(highlighter);
                hl.transform.position = movePlanet.position;
                hl.transform.rotation = Quaternion.Euler(0, 180, 0);
                hl.transform.GetChild(0).GetComponent<TextMesh>().text = movePlanet.name;
            }
        }
    }

    public void SetupMovement(Transform planet, Vector3 pos, GameObject hl)
    {
        movePlanet = planet;
        startPosition = movePlanet.position;
        position = pos;
        highlighter = hl;
        move = true;
    }
}
