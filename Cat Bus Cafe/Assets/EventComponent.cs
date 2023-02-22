using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventComponent : MonoBehaviour
{
    public UnityEvent onClick;

    public void OnMouseDown()
    {
        Debug.Log("Clicked " + this.name);
        onClick.Invoke();
    }
}
