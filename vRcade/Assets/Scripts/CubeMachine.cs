using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMachine : MonoBehaviour
{
    public GameObject linkedButton;
    
    //public GameObject prefab;

    public bool isAbsolutePosition;
    public Vector3 absolutePosition;

    public float yOffset = 0.25f;

    public bool isGrabbable;

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
            GameObject prefab = GameObject.FindWithTag("BlockSelector").GetComponent<BlockControl>().currentBlock;
            butPressedDown = true;

            Vector3 newPos;

            if (isAbsolutePosition)
                newPos = absolutePosition;
            else
            {
                Vector3 thisHeight = transform.localScale;
                Vector3 prefabHeight = prefab.transform.localScale / 2;

                float spawnHeight = thisHeight.y + prefabHeight.y + yOffset;

                newPos = new Vector3(transform.position.x, spawnHeight, transform.position.z);
            }
            //Debug.Log("thisHeight - " + thisHeight + "  ::  prefabHeight - " + prefabHeight + "  ::  spawnHeight - " + spawnHeight + "  ::  newPos - " + newPos);

            GameObject obj = Instantiate(prefab, newPos, Quaternion.Euler(0, 0, 0));
            if (isGrabbable)
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
