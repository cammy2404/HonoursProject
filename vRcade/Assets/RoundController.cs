using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    int roundLength = 30;

    public HeightDetection heightDetection;
    public TopScoreDisplay topScoreDisplay;

    [HideInInspector]
    public bool playing = false;

    public Transform startRoundButton;
    bool startRound = false;

    [Header("Timer Display")]
    public Transform numbersObj;
    public Transform numbersDisplay;
    public Vector3 rotationOffset;
    Transform[] nums;
    Transform[] numberPositions;

    [Header("Ending a game")]
    public Transform fireworks;

    int totalSeconds;
    int mins;
    int seconds;

    int minTensPrev = -1, minOnesPrev = -1, secTensPrev = -1, secOnesPrev = -1;

    public bool TestStart = false;

    void Start()
    {
        numberPositions = new Transform[numbersDisplay.childCount - 1];
        for (int i = 0; i < numberPositions.Length; i++)
        {
            numberPositions[i] = numbersDisplay.GetChild(i);
            foreach (Transform t in numberPositions[i])
                Destroy(t.gameObject);
        }

        nums = new Transform[numbersObj.childCount];
        for (int i = 0; i < numbersObj.childCount; i++)
            nums[i] = numbersObj.GetChild(i);

        for (int i = 0; i < 4; i++)
            DisplayNumber(0, i);

    }

    void Update()
    {
        if (!playing && startRound)
        {
            startRound = false;

            StartRound(roundLength);
        }

        if (TestStart)
        {
            TestStart = false;
            GameManager.instance.LoadHome();
        }
    }

    public void StartRound(int newRoundLength = 60)
    {
        totalSeconds = roundLength = newRoundLength;

        StartCoroutine(StartRoundTimer());
        StartCoroutine(CountdownSecond());
    }

    IEnumerator StartRoundTimer()
    {
        playing = true;
        yield return new WaitForSeconds(roundLength);
        EndGame();
    }

    IEnumerator CountdownSecond()
    {
        mins = totalSeconds / 60;
        seconds = totalSeconds % 60;

        UpdateTimerDisplay();

        yield return new WaitForSeconds(1);

        totalSeconds--;
        if (totalSeconds >= 0)
            StartCoroutine(CountdownSecond());
    }

    void EndGame()
    {
        topScoreDisplay.AddScore(heightDetection.score);
        
        CleanUpGame();
        Fireworks();

        startRoundButton.GetComponent<StartRound>().ResetButton();
        
        playing = false;
    }

    void CleanUpGame()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Grabbable");
        foreach(GameObject go in objs)
            Destroy(go);

        heightDetection.ResetHeights();
    }

    void Fireworks()
    {
        foreach (Transform t in fireworks)
        {
            t.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }

    void UpdateTimerDisplay()
    {
        int minTens = mins / 10;
        int minOnes = mins - (minTens * 10);

        int secTens = seconds / 10;
        int secOnes = seconds - (secTens * 10);

        if (minTens != minTensPrev)
            DisplayNumber(minTens, 0);
    
        if (minOnes != minOnesPrev) 
            DisplayNumber(minOnes, 1);


        if (secTens != secTensPrev)
            DisplayNumber(secTens, 2);
        
        if (secOnes != secOnesPrev)
            DisplayNumber(secOnes, 3);

        minTensPrev = minTens;
        minOnesPrev = minOnes;

        secTensPrev = secTens;
        secOnesPrev = secOnes;
    }

    void DisplayNumber(int val, int position)
    {
        if (numberPositions[position].childCount != 0)
            foreach (Transform t in numberPositions[position])
                Destroy(t.gameObject);

        GameObject numObj = Instantiate(nums[val].gameObject, numberPositions[position]);
        numObj.transform.position = numberPositions[position].position;
        numObj.transform.rotation = Quaternion.Euler(rotationOffset);
        numObj.layer = gameObject.layer;
    }
}
