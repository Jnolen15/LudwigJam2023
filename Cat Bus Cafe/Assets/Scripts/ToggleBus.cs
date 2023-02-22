using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBus : MonoBehaviour
{
    [SerializeField] private bool busGoing;
    private BusControl bc;

    private void Start()
    {
        bc = GetComponentInParent<BusControl>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on Coots :3. Toggling bus");

        if (busGoing)
        {
            busGoing = !busGoing;
            bc.StopBus();
        }
        else
        {
            busGoing = !busGoing;
            bc.StartBus();
        }
    }
}
