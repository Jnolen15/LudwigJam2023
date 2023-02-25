using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BusControl bs;
    [SerializeField] private float eventTime;
    [SerializeField] private float eventTimer;

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
}
