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
    public List<GameObject> catList = new List<GameObject>();
    public List<BusStop> stopList = new List<BusStop>();
    public List<Passenger> Passengers = new List<Passenger>();
    // MISC
    private SeatControl seatControl;
    private bool busIsStopped;

    // BUS STOP CLASS
    [System.Serializable]
    public class BusStop
    {
        public float distance;
        public string name;
        public List<Passenger> Cats = new List<Passenger>();

        public BusStop(float dist, Passenger[] newCats, string title = "New Stop")
        {
            distance = dist;
            name = title;

            for (int i = 0; i < newCats.Length; i++)
            {
                Cats.Add(newCats[i]);
            }
        }
    }

    // PASSENGER CLASS
    [System.Serializable]
    public class Passenger
    {
        public int stopNum;
        public string name;
        public GameObject cat;

        public Passenger(int dist, GameObject newCat, string title = "Cat")
        {
            stopNum = dist;
            name = title;
            cat = newCat;
            cat.SetActive(false);
        }
    }

    void Start()
    {
        seatControl = this.GetComponent<SeatControl>();

        GenerateStop();
    }

    void Update()
    {
        if (!busIsStopped)
            MoveBus();

        // For testing
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateStop();
        }
    }

    // ============ BUS CONTROL ============
    private void MoveBus()
    {
        busLocation += busSpeed * Time.deltaTime;

        if (stopList.Count <= 0)
            return;

        var stop = stopList[0];
        if (stop.distance < busLocation)
            MissedStop(stop);
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
        foreach (Passenger newCat in stop.Cats)
        {
            Passengers.Add(newCat);
            seatControl.AssignSeat(newCat.cat);
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

        var newCat = Passengers[0];
        if (newCat.stopNum > 0)
            return;

        // Let off passenger
        Debug.Log("Letting off: " + newCat.name);

        // Remove stop and look for another
        seatControl.UnassignSeat(newCat.cat);
        Passengers.Remove(newCat);
        LetOffPassengers();
    }


    // ============ GENERATION ============
    private void GenerateStop()
    {
        int rand = Random.Range(1, 3);
        Passenger[] newCats = new Passenger[rand];
        for (int i = 0; i < rand; i++)
        {
            var newCat = new Passenger(Random.Range(2, 5), GetRandomCat());
            newCats[i] = newCat;
        }

        var newStop = new BusStop(busLocation + stopGenDistance, newCats);        
        stopList.Add(newStop);
    }

    private GameObject GetRandomCat()
    {
        var newCat = Instantiate(catList[Random.Range(0, catList.Count-1)]);
        return (newCat);
    }
}
