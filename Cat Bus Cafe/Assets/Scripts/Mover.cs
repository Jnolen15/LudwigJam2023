using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform moveToLocation;
    [SerializeField] private Transform neighborSnapPoint;
    [SerializeField] private BusControl bus;

    private void Start()
    {
        bus = GameObject.FindGameObjectWithTag("Bus").GetComponent<BusControl>();
    }

    void Update()
    {
        if (!bus.busIsStopped)
        {
            var step = moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * step);
        }

        if(transform.position.x > 35)
        {
            transform.position = new Vector3(neighborSnapPoint.position.x, transform.position.y, transform.position.z);
        }
    }
}
