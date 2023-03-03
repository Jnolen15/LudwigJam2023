using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    [SerializeField] private GameObject trash;
    private GameManager gm;
    private AudioSource audiosource;
    private bool pickedUp;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        audiosource = this.GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (!pickedUp)
        {
            trash.SetActive(false);
            audiosource.Play();
            gm.UpdatePoints("Picked up trash", 2);
            Destroy(gameObject, 0.5f);
            pickedUp = true;
        }
    }
}
