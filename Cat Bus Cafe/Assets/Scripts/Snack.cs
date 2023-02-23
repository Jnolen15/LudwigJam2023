using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snack : MonoBehaviour
{
    public GameObject square;
    public GameObject pop;
    public GameObject ring;
    public List<snackVisual> materialList = new List<snackVisual>();

    [System.Serializable]
    public class snackVisual
    {
        public Material mat;
        public Flavor flava;
        public bool wifMilk;
        public bool wifBoba;
    }

    public enum Flavor
    {
        Nothing,
        Green, // Catnip
        Red, // Strawberry?
        Blend // Blend
    }
    public Flavor flavor;

    public bool hasMilk;
    public bool hasBoba;

    public enum SnackType
    {
        Nothing,
        Square,
        Pop,
        Ring
    }
    public SnackType snackType;

    public void UpdateMaterial()
    {
        // Show the snack
        pop.SetActive(false);
        square.SetActive(false);
        ring.SetActive(false);

        switch (snackType)
        {
            case SnackType.Nothing:
                break;
            case SnackType.Pop:
                pop.SetActive(true);
                break;
            case SnackType.Square:
                square.SetActive(true);
                break;
            case SnackType.Ring:
                ring.SetActive(true);
                break;
        }


        // Update the drink material
        foreach(snackVisual visual in materialList)
        {
            bool isCorrect = true;

            // Find a material that matches
            if (flavor != visual.flava)
                isCorrect = false;

            if (hasMilk != visual.wifMilk)
                isCorrect = false;

            if (hasBoba != visual.wifBoba)
                isCorrect = false;

            // Set material
            if (isCorrect)
            {
                transform.GetChild(0).GetComponent<Renderer>().material = visual.mat;
                break;
            }
        }
    }
}
