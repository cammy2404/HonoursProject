using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public Transform numbersObj;
    public GameObject heightDetection;
    [Tooltip("Set active to use the current score \nSet inactive to use top score")]
    public bool currentScore = true;

    int score = 0;

    Transform[] nums;
    Transform[] numsPos;

    int hundreds;
    int tens;
    int ones;

    int prevHundreds = -1;
    int prevTens = -1;
    int prevOnes = -1;

    // Start is called before the first frame update
    void Start()
    {
        nums = new Transform[numbersObj.childCount];
        numsPos = new Transform[transform.childCount];

        for (int i = 0; i < numbersObj.childCount; i++)
        {
            nums[i] = numbersObj.GetChild(i);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            numsPos[i] = transform.GetChild(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        score = (currentScore) ? heightDetection.GetComponent<HeightDetection>().currScore : heightDetection.GetComponent<HeightDetection>().score;

        score %= 1000;
        if (score < 0) score = 0;

        hundreds = score / 100;
        tens = (score - (hundreds * 100)) / 10;
        ones = score - ((hundreds * 100) + (tens * 10));

        if (hundreds != prevHundreds)
            Display(numsPos[0], hundreds);
        if (tens != prevTens)
            Display(numsPos[1], tens);
        if (ones != prevOnes)
            Display(numsPos[2], ones);

        prevHundreds = hundreds;
        prevTens = tens;
        prevOnes = ones;
    }

    void Display(Transform pos, int val)
    {
        if (pos.childCount != 0)
            foreach (Transform t in pos)
                Destroy(t.gameObject);

        GameObject go = Instantiate(nums[val].gameObject, pos);
        go.layer = pos.gameObject.layer;
    }
}
