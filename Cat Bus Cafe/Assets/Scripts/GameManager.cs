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
    [SerializeField] private GameObject windowMuck;
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
            float randSway = Random.Range(-3, 2);
            eventTime = eventTimer + randSway;

            // Do an event
            if (bs.Passengers.Count > 0)
            {
                var rand = Random.Range(0, 6);
                var randPassenger = Random.Range(0, bs.Passengers.Count);
                if (rand < 2)
                    bs.Passengers[randPassenger].cat.GetComponent<Cat>().RequestSnack();
                else if (rand == 3)
                    bs.Passengers[randPassenger].cat.GetComponent<Cat>().RequestPet();
                else if (rand == 4)
                    bs.Passengers[randPassenger].cat.GetComponent<Cat>().ThrowTrash();
                else if (rand == 5)
                    StartMuck();
            }
        }

        // TESTING
        if(Input.GetKeyDown(KeyCode.M))
            StartMuck();
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

    // Window Muck Task
    private void StartMuck()
    {
        windowMuck.SetActive(true);
        bs.StopBus();
    }

    public void CleanMuck()
    {
        windowMuck.SetActive(false);
        UpdatePoints("Cleaned Window", 4);
    }
}
