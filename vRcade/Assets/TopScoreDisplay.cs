using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopScoreDisplay : MonoBehaviour
{
    public GameObject playerText;
    public Transform numbersObj;

    public float startingPoint = 1.5f;
    public float step = 1;

    Transform[] nums;

    List<int> scores = new List<int>();

    public bool addOne = false;

    // Start is called before the first frame update
    void Start()
    {
        nums = new Transform[numbersObj.childCount];

        for (int i = 0; i < numbersObj.childCount; i++)
        {
            nums[i] = numbersObj.GetChild(i);
        }
    }

    void Update()
    {
        if (addOne)
        {
            addOne = false;
            AddScore(Random.Range(0, 999));
        }
    }

    public void AddScore(int score)
    {
        scores.Add(score);
        Debug.Log(score);

        GameObject pText = Instantiate(playerText, transform);
        GameObject pNum = Instantiate(nums[scores.Count].gameObject, transform);

        float yPos = startingPoint + (step * (scores.Count - 1));

        pText.transform.position = transform.position + new Vector3(1, -(yPos / 2), 0);

        pNum.transform.position = transform.position + new Vector3(0.5f / 2, -((yPos + 0.3f) / 2), 0);
        pNum.transform.rotation = Quaternion.Euler(Vector3.zero);


        int hundreds = score / 100;
        int tens = (score - (hundreds * 100)) / 10;
        int ones = score - ((hundreds * 100) + (tens * 10));

        GameObject numH = Instantiate(nums[hundreds].gameObject, transform);
        numH.transform.position = transform.position + new Vector3(-1f / 2, -((yPos + 0.3f) / 2), 0);
        numH.transform.rotation = Quaternion.Euler(Vector3.zero);
        numH.layer = gameObject.layer;

        GameObject numT = Instantiate(nums[tens].gameObject, transform);
        numT.transform.position = transform.position + new Vector3(-1.5f / 2, -((yPos + 0.3f) / 2), 0);
        numT.transform.rotation = Quaternion.Euler(Vector3.zero);
        numT.layer = gameObject.layer;

        GameObject numO = Instantiate(nums[ones].gameObject, transform);
        numO.transform.position = transform.position + new Vector3(-2f / 2, -((yPos + 0.3f) / 2), 0);
        numO.transform.rotation = Quaternion.Euler(Vector3.zero);
        numO.layer = gameObject.layer;
    }
}