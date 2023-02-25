using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetScript : MonoBehaviour
{
    public GameObject Pet1;
    public GameObject Pet2;
    private bool inPetAnim;

    public void OnMouseDown()
    {
        if (!inPetAnim)
        {
            StopAllCoroutines();
            StartCoroutine(PetAnim());
        }
    }

    IEnumerator PetAnim()
    {
        inPetAnim = true;
        var sprite = transform.GetChild(0);
        sprite.localScale = new Vector3(1.1f, 0.9f, 1);
        Pet1.SetActive(false);
        Pet2.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Pet2.SetActive(false);
        Pet1.SetActive(true);
        sprite.localScale = new Vector3(1, 1, 1);
        inPetAnim = false;
        yield return new WaitForSeconds(0.2f);
        Pet1.SetActive(false);
    }
}
