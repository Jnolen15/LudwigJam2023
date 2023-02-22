using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignAnimator : MonoBehaviour
{
    [SerializeField] private float animDist;
    [SerializeField] private List<AnimPoint> animPoints = new List<AnimPoint>();
    [SerializeField] private int animCount;
    [SerializeField] private float curInterval;
    [SerializeField] private bool isAnimating;
    private BusControl bc;

    [System.Serializable]
    public class AnimPoint
    {
        public GameObject spot;
        public float interval;
    }

    void Start()
    {
        bc = gameObject.GetComponent<BusControl>();
    }

    void Update()
    {
        if ((bc.nextStop.distance - bc.busLocation) < animDist)
        {
            if (!isAnimating)
                StartAnimating();

            UpdateSign();
        } 
        else if(isAnimating)
            EndAnimating();
    }

    private void StartAnimating()
    {
        animCount = 0;
        curInterval = 0;
        isAnimating = true;
    }

    private void EndAnimating()
    {
        SetAllInactive();
        isAnimating = false;
    }

    private void UpdateSign()
    {
        curInterval = (bc.nextStop.distance - bc.busLocation);

        if (animCount >= animPoints.Count)
            return;

        if (curInterval <= animPoints[animCount].interval)
        {
            SetAllInactive();
            animPoints[animCount].spot.SetActive(true);
            animCount++;
        }
    }

    private void SetAllInactive()
    {
        foreach (AnimPoint point in animPoints)
        {
            point.spot.SetActive(false);
        }
    }
}
