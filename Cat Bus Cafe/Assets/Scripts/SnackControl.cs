using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackControl : MonoBehaviour
{
    public GameObject snackPrefab;
    public List<Transform> spots = new List<Transform>();

    [SerializeField] private GameObject mover;
    [SerializeField] private GameObject curSnack;
    [SerializeField] private Snack curSnackScript;
    [SerializeField] private bool makingSnack;
    [SerializeField] private int snackPos = -1;

    public void MoveNext()
    {
        if (!makingSnack)
            return;

        snackPos++;

        if(snackPos < spots.Count)
        {
            curSnack.transform.transform.position = spots[snackPos].position;
            mover.transform.transform.position = spots[snackPos].position;
        }
        else
        {
            Debug.Log("Snack as far as can be");
            snackPos = spots.Count - 1;
        }
    }

    public void MoveBack()
    {
        if (!makingSnack)
            return;

        snackPos--;

        if (snackPos >= 0)
        {
            curSnack.transform.transform.position = spots[snackPos].position;
            mover.transform.transform.position = spots[snackPos].position;
        }
        else
        {
            Debug.Log("Snack as far as can be");
            snackPos = 0;
        }
    }

    // ============ Obeject Functions ============
    public void CreateNewSnack()
    {
        if (makingSnack)
            return;

        snackPos = 0;
        mover.transform.transform.position = spots[snackPos].position;
        curSnack = Instantiate(snackPrefab, spots[snackPos].position, spots[snackPos].rotation);
        curSnackScript = curSnack.GetComponent<Snack>();
        makingSnack = true;
        mover.SetActive(true);
    }

    public void FlavorDrinkGreen()
    {
        if (snackPos != 1)
            return;

        switch (curSnackScript.flavor)
        {
            case Snack.Flavor.Nothing:
                curSnackScript.flavor = Snack.Flavor.Green;
                break;
            case Snack.Flavor.Green:
                curSnackScript.flavor = Snack.Flavor.Green;
                break;
            case Snack.Flavor.Red:
                curSnackScript.flavor = Snack.Flavor.Blend;
                break;
            case Snack.Flavor.Blend:
                curSnackScript.flavor = Snack.Flavor.Blend;
                break;
        }

        curSnackScript.UpdateMaterial();
        Debug.Log("Added green flavor");
    }

    public void FlavorDrinkRed()
    {
        if (snackPos != 2)
            return;

        switch (curSnackScript.flavor)
        {
            case Snack.Flavor.Nothing:
                curSnackScript.flavor = Snack.Flavor.Red;
                break;
            case Snack.Flavor.Green:
                curSnackScript.flavor = Snack.Flavor.Blend;
                break;
            case Snack.Flavor.Red:
                curSnackScript.flavor = Snack.Flavor.Red;
                break;
            case Snack.Flavor.Blend:
                curSnackScript.flavor = Snack.Flavor.Blend;
                break;
        }

        curSnackScript.UpdateMaterial();
        Debug.Log("Added green flavor");
    }

    public void AddMilk()
    {
        if (snackPos != 3)
            return;

        curSnackScript.hasMilk = true;

        curSnackScript.UpdateMaterial();
        Debug.Log("Added milk");
    }

    public void AddBoba()
    {
        if (snackPos != 3)
            return;

        curSnackScript.hasBoba = true;

        curSnackScript.UpdateMaterial();
        Debug.Log("Added Boba");
    }

    public void AddSnack(string type)
    {
        if (snackPos != 4)
            return;

        switch (type)
        {
            case "Nothing":
                curSnackScript.snackType = Snack.SnackType.Nothing;
                break;
            case "Pop":
                curSnackScript.snackType = Snack.SnackType.Pop;
                break;
            case "Ring":
                curSnackScript.snackType = Snack.SnackType.Ring;
                break;
            case "Square":
                curSnackScript.snackType = Snack.SnackType.Square;
                break;
        }

        curSnackScript.UpdateMaterial();
        Debug.Log("Added snack");
    }

    public void Finish()
    {
        if (!makingSnack)
            return;

        Debug.Log("Treat Finished!");
        mover.SetActive(false);
        makingSnack = false;
        curSnack = null;
        curSnackScript = null;
        snackPos = -1;
    }
}
