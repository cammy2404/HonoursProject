using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMachine : MonoBehaviour
{
    public GameObject linkedButton;
    public GameObject prefab;
    public float yOffset = 0.25f;

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

            Vector3 thisHeight = transform.localScale;
            Vector3 prefabHeight = prefab.transform.localScale / 2;

            float spawnHeight = thisHeight.y + prefabHeight.y + yOffset;

            Vector3 newPos = new Vector3(transform.position.x, spawnHeight, transform.position.z);

            //Debug.Log("thisHeight - " + thisHeight + "  ::  prefabHeight - " + prefabHeight + "  ::  spawnHeight - " + spawnHeight + "  ::  newPos - " + newPos);

            GameObject obj = Instantiate(prefab, newPos, Quaternion.Euler(0, 0, 0));
            obj.transform.parent = GameObject.FindGameObjectWithTag("GrabbableParent").transform;
        }

        if (!button.pressed && butPressedDown)
        {
            butPressedDown = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        if (linkedButton != null)
            Gizmos.DrawLine(transform.position, linkedButton.transform.position);
    }
}
