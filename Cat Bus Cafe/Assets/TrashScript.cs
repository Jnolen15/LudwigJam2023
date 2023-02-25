using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    private GameManager gm;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnMouseDown()
    {
        gm.UpdatePoints("Picked up trash", 2);
        Destroy(gameObject);
    }
}
