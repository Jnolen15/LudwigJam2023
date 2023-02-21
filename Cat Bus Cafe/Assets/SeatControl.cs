using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatControl : MonoBehaviour
{
    public List<Seat> SeatList = new List<Seat>();

    [System.Serializable]
    public class Seat
    {
        public Transform spot;
        public GameObject cat;
        public bool isTaken;
    }

    public Seat AssignSeat(GameObject boardingCat)
    {
        Seat openSeat = null;
        foreach(Seat seat in SeatList)
        {
            if (!seat.isTaken)
            {
                openSeat = seat;
                break;
            }
        }

        if (openSeat == null)
        {
            Debug.Log("No open seats found");
            return null;
        }

        openSeat.cat = boardingCat;
        openSeat.cat.SetActive(true);
        openSeat.isTaken = true;

        boardingCat.transform.position = openSeat.spot.position;
        boardingCat.transform.rotation = openSeat.spot.rotation;
        boardingCat.transform.SetParent(openSeat.spot);

        return openSeat;
    }

    public void UnassignSeat(GameObject leavingCat)
    {
        Seat openSeat = null;
        foreach (Seat seat in SeatList)
        {
            if (seat.cat == leavingCat)
            {
                openSeat = seat;
                break;
            }
        }

        if (openSeat == null)
        {
            Debug.Log("Cat not on bus");
            return;
        }

        Destroy(openSeat.cat);
        openSeat.cat = null;
        openSeat.isTaken = false;
    }
}
