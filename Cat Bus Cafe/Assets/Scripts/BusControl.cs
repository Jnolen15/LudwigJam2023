using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusControl : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float busSpeed;
    [SerializeField] private float stopRange;
    [SerializeField] private float stopGenDistance;
    [SerializeField] private float busLocation;
    // LISTS
    public List<BusStop> stopList = new List<BusStop>();
    public List<Passenger> Passengers = new List<Passenger>();
    // MISC
    private bool busIsStopped;

    // BUS STOP CLASS
    [System.Serializable]
    public class BusStop
    {
        public float distance;
        public string name;
        public List<Passenger> Cats = new List<Passenger>();

        public BusStop(float dist, int numCats, string title = "New Stop")
        {
            distance = dist;
            name = title;

            for (int i = 0; i < numCats; i++)
            {
                Cats.Add(new Passenger(Random.Range(1, 5)));
            }
        }
    }

    // PASSENGER CLASS
    [System.Serializable]
    public class Passenger
    {
        public int stopNum;
        public string name;

        public Passenger(int dist, string title = "Cat")
        {
            stopNum = dist;
            name = title;
        }
    }

    void Start()
    {
        GenerateStop();
    }

    void Update()
    {
        if(!busIsStopped)
            MoveBus();

        // For testing
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateStop();
        }
    }

    private void MoveBus()
    {
        busLocation += busSpeed * Time.deltaTime;

        if (stopList.Count <= 0)
            return;

        var stop = stopList[0];
        if (stop.distance < busLocation)
            MissedStop(stop);
    }

    private void GenerateStop()
    {
        int rand = Random.Range(1, 3);
        var newStop = new BusStop(busLocation + stopGenDistance, rand);
        stopList.Add(newStop);
    }

    public void StopBus()
    {
        if (busIsStopped) return;

        Debug.Log("Stopping");
        busIsStopped = true;
        TryArriveAtStop();
    }

    public void StartBus()
    {
        if (!busIsStopped) return;

        Debug.Log("Starting");
        busIsStopped = false;
    }

    private void TryArriveAtStop()
    {
        if (stopList.Count <= 0)
            return;

        var stop = stopList[0];
        if ((stop.distance - busLocation) > stopRange)
            return;

        // Let off passengers
        Debug.Log("Arrived at stop: " + stop.name + " Distance away: " + (stop.distance - busLocation));
        UpdatePassengerStops();
        LetOffPassengers();

        // Add new passengers
        foreach (Passenger cat in stop.Cats)
        {
            Passengers.Add(cat);
        }

        // Remove stop and look for another
        stopList.Remove(stop);
        TryArriveAtStop();
    }

    private void MissedStop(BusStop stop)
    {
        Debug.Log("Missed stop: " + stop.name + "!");
        stopList.Remove(stop);

        UpdatePassengerStops();
    }

    private void UpdatePassengerStops()
    {
        foreach (Passenger cat in Passengers)
        {
            cat.stopNum--;
        }
    }

    private void LetOffPassengers()
    {
        if (Passengers.Count <= 0)
            return;

        var cat = Passengers[0];
        if (cat.stopNum > 0)
            return;

        // Let off passenger
        Debug.Log("Letting off: " + cat.name);

        // Remove stop and look for another
        Passengers.Remove(cat);
        LetOffPassengers();
    }
}
