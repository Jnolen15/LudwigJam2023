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
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private bool gameTimerRunning;
    [SerializeField] private float gameTime;
    [SerializeField] private float gameTimer;
    [SerializeField] private float eventTime;
    [SerializeField] private float eventTimer;
    [SerializeField] private BusControl bs;
    [SerializeField] private GameObject windowMuck;
    [SerializeField] private int points;

    [SerializeField] private AudioClip pointSound1;
    [SerializeField] private AudioClip pointSound2;
    [SerializeField] private AudioClip pointSound3;
    [SerializeField] private AudioClip partySound;
    [SerializeField] private AudioClip mudSplat;
    private AudioSource audioSource;

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
        Time.timeScale = 1;

        gameTimer = gameTime;
        gameTimerRunning = true;
        audioSource = this.GetComponent<AudioSource>();
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
                EndGame();
            }
        }

        // Random Event Timer
        if (eventTime > 0) eventTime -= Time.deltaTime;
        else
        {
            float randSway = Random.Range(-3, 2);
            eventTime = eventTimer + randSway;

            if (bs.Passengers.Count > 0)
                DoEvent();
        }
    }

    private void DisplayTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public void EndGame()
    {
        audioSource.PlayOneShot(partySound);
        Time.timeScale = 0;
        endScreen.SetActive(true);
        endScoreText.text = points.ToString();
    }

    // ========= Events =========
    public void DoEvent()
    {
        var rand = Random.Range(0, 6);
        if (rand == 0)
            StartMuck();
        else
        {
            var randPassenger = Random.Range(0, bs.Passengers.Count);

            if (!CatIsOpenToTask(randPassenger))
                return;

            if (rand < 3)
                bs.Passengers[randPassenger].cat.GetComponent<Cat>().RequestSnack();
            else if (rand == 4)
                bs.Passengers[randPassenger].cat.GetComponent<Cat>().RequestPet();
            else if (rand == 5)
                bs.Passengers[randPassenger].cat.GetComponent<Cat>().ThrowTrash();
        }

    }

    private bool CatIsOpenToTask(int catNum)
    {
        var daCat = bs.Passengers[catNum].cat.GetComponent<Cat>();

        bool canDoTask = true;

        if (daCat.waitingForOrder)
            canDoTask = false;
        else if (daCat.hasOrdered)
            canDoTask = false;
        else if (daCat.petRequested)
            canDoTask = false;

        return canDoTask;
    }

    // ========= Points =========
    public void UpdatePoints(string message, int points)
    {
        Points += points;
        pointsReasonText.text = message + " +" + points;
        StopAllCoroutines();
        StartCoroutine(AnimatePoints());

        var randSound = Random.Range(0, 3);
        if (randSound == 0)
            audioSource.PlayOneShot(pointSound1);
        else if (randSound == 1)
            audioSource.PlayOneShot(pointSound2);
        else if (randSound == 1)
            audioSource.PlayOneShot(pointSound3);
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

    // ========= Window Muck Task =========
    private void StartMuck()
    {
        audioSource.PlayOneShot(mudSplat);
        windowMuck.SetActive(true);
        bs.StopBus();
    }

    public void CleanMuck()
    {
        windowMuck.SetActive(false);
        UpdatePoints("Cleaned Window", 4);
    }
}
