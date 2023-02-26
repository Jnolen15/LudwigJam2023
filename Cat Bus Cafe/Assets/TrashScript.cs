using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    private GameManager gm;
    private AudioSource audiosource;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        audiosource = this.GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        audiosource.Play();
        gm.UpdatePoints("Picked up trash", 2);
        Destroy(gameObject, 0.5f);
    }
}
