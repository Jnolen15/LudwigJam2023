using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI pointsReasonText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private bool gameTimerRunning;
    [SerializeField] private float gameTime;
    [SerializeField] private float gameTimer;
    [SerializeField] private float eventTime;
    [SerializeField] private float eventTimer;
    [SerializeField] private BusControl bs;

    [SerializeField] private int points;

    public int Points
    {
        get { return points; }
        set
        {
            points = value;
            pointsText.text = points.ToString();
        }
    }

    private void Start()
    {
        gameTimer = gameTime;
        gameTimerRunning = true;
    }

    void Update()
    {
        // Game Timer
        if (gameTimerRunning)
        {
            if (gameTimer > 0)
            {
                gameTimer -= Time.deltaTime;
                DisplayTime(gameTimer);
            } else
            {
                gameTimerRunning = false;
                timeText.text = new string("00:00");
                Debug.Log("GameOVer");
            }
        }

        // Random Event Timer
        if (eventTime > 0) eventTime -= Time.deltaTime;
        else
        {
            eventTime = eventTimer;

            // Do an event
            if (bs.Passengers.Count > 0)
            {
                var rand = Random.Range(0, 4);
                var randPassenger = Random.Range(0, bs.Passengers.Count);
                if (rand == 0)
                    bs.Passengers[randPassenger].cat.GetComponent<Cat>().RequestSnack();
                else if (rand == 1)
                    bs.Passengers[randPassenger].cat.GetComponent<Cat>().RequestPet();
                else if (rand > 1)
                    bs.Passengers[randPassenger].cat.GetComponent<Cat>().ThrowTrash();
            }
        }
    }

    private void DisplayTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdatePoints(string message, int points)
    {
        Points += points;
        pointsReasonText.text = message + " +" + points;
        StopAllCoroutines();
        StartCoroutine(AnimatePoints());
    }

    IEnumerator AnimatePoints()
    {
        float time = 0;

        while (time < 1f)
        {
            float t = time / 1f;
            t = t * t * (3f - 2f * t);

            pointsReasonText.transform.position = Vector3.Lerp(startPos.position, endPos.position, t);

            time += Time.deltaTime;
            yield return null;
        }
    }
}
