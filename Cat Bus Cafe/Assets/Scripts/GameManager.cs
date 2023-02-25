using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
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

    void Update()
    {
        if (eventTime > 0) eventTime -= Time.deltaTime;
        else
        {
            eventTime = eventTimer;

            // Do an event
            if (bs.Passengers.Count > 0)
            {
                var rand = Random.Range(0, bs.Passengers.Count);
                bs.Passengers[rand].cat.GetComponent<Cat>().RequestSnack();
            }
        }
    }

    public void UpdatePoints(string message, int points)
    {
        Points += points;
    }
}
