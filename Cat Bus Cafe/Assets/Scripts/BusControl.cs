using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusControl : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float busSpeed;
    [SerializeField] private float stopRange;
    [SerializeField] private float stopGenDistance;
    public float busLocation;
    public BusStop nextStop;
    // LISTS
    public List<GameObject> catList = new List<GameObject>();
    public List<Passenger> Passengers = new List<Passenger>();
    public List<Passenger> Leaving = new List<Passenger>();
    public List<Passenger> Boarding = new List<Passenger>();
    // MISC
    private SeatControl seatControl;
    private GameManager gManager;
    public bool busIsStopped;

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
        gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        seatControl = this.GetComponent<SeatControl>();

        //GenerateStop();

        // Create first stop
        Passenger[] newCats = new Passenger[2];
        for (int i = 0; i < 2; i++)
        {
            var newCat = new Passenger(Random.Range(2, 5), GetRandomCat());
            newCats[i] = newCat;
        }

        var newStop = new BusStop(busLocation, newCats);
        nextStop = newStop;


        StopBus();
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

        if (nextStop == null)
            return;

        if (nextStop.distance < busLocation)
            MissedStop(nextStop);
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

    private void MissedStop(BusStop stop)
    {
        Debug.Log("Missed stop: " + stop.name + "!");
        nextStop = null;

        // Update Bus sign
        this.GetComponent<SignAnimator>().ResetSign();

        UpdatePassengerStops();
        GenerateStop();
    }

    private void UpdatePassengerStops()
    {
        foreach (Passenger cat in Passengers)
        {
            cat.stopNum--;
            if (cat.stopNum < 0)
                cat.cat.GetComponent<Cat>().React(5f, "Angry");
        }
    }

    private void TryArriveAtStop()
    {
        if (nextStop == null)
            return;

        if ((nextStop.distance - busLocation) > stopRange)
            return;

        // Let off passengers
        Debug.Log("Arrived at stop: " + nextStop.name + " Distance away: " + (nextStop.distance - busLocation));
        UpdatePassengerStops();
        LetOffPassengers();

        // Add new passengers to boarding list
        foreach (Passenger newCat in nextStop.Cats)
        {
            //Passengers.Add(newCat);
            //seatControl.AssignSeat(newCat.cat);
            Boarding.Add(newCat);
        }

        // Update Bus sign
        this.GetComponent<SignAnimator>().ResetSign();

        // Remove stop and look for another
        nextStop = null;
        GenerateStop();

        // Animate Passengers
        StartCoroutine(AnimateBoarding());
    }

    private void LetOffPassengers()
    {
        if (Passengers.Count <= 0)
            return;

        // Look for passsenges who are ready to get off
        var newCat = Passengers[0];
        foreach (Passenger curCat in Passengers)
        {
            newCat = curCat;
            if (curCat.stopNum <= 0)
                break;
        }
        // If this passenger still has stops > 0, then none need to get off
        if (newCat.stopNum > 0)
            return;

        if (newCat.stopNum == 0)
            newCat.cat.GetComponent<Cat>().React(5f, "Happy");

        // Let off passenger
        Debug.Log("Letting off: " + newCat.name);

        // Remove stop and look for another
        //seatControl.UnassignSeat(newCat.cat);
        Passengers.Remove(newCat);
        Leaving.Add(newCat);
        LetOffPassengers();
    }

    private IEnumerator AnimateBoarding()
    {
        yield return new WaitForSeconds(0.2f);

        // Animate leaving passenders
        int numRightDropOffs = 0;
        foreach (Passenger newCat in Leaving)
        {
            if (newCat.stopNum == 0)
                numRightDropOffs++;

            seatControl.UnassignSeat(newCat.cat);
        }

        // Give points
        if(numRightDropOffs > 0)
            gManager.UpdatePoints(new string("On-Time Dropoff (" + numRightDropOffs + ")"), numRightDropOffs*5);

        Leaving.Clear();

        yield return new WaitForSeconds(0.6f);

        // Add new passengers
        gManager.UpdatePoints(new string("Picked up cats"), Boarding.Count * 2);
        foreach (Passenger newCat in Boarding)
        {
            var seatOpen = seatControl.AssignSeat(newCat.cat);
            if(seatOpen != null)
                Passengers.Add(newCat);
            else
            {
                Debug.Log("Cat could not be let on");
            }
        }

        Boarding.Clear();
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

        var distance = (stopGenDistance + Random.Range(-10, 20));
        var newStop = new BusStop(busLocation + distance, newCats);        
        nextStop = newStop;
    }

    private GameObject GetRandomCat()
    {
        var newCat = Instantiate(catList[Random.Range(0, catList.Count-1)]);
        return (newCat);
    }
}
