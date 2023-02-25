using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBus : MonoBehaviour
{
    [SerializeField] private bool busGoing;
    [SerializeField] private AudioClip rev;
    [SerializeField] private AudioClip breaking;
    private BusControl bc;
    private AudioSource audioSource;

    private void Start()
    {
        bc = GetComponentInParent<BusControl>();
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on Coots :3. Toggling bus");

        if (busGoing)
        {
            busGoing = !busGoing;
            bc.StopBus();
            audioSource.PlayOneShot(breaking);
        }
        else
        {
            busGoing = !busGoing;
            bc.StartBus();
            audioSource.PlayOneShot(rev);
        }
    }
}
