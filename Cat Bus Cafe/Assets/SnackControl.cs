using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackControl : MonoBehaviour
{
    public GameObject snackPrefab;
    public List<Transform> spots = new List<Transform>();

    [SerializeField] private GameObject mover;
    [SerializeField] private GameObject curSnack;
    [SerializeField] private bool makingSnack;
    [SerializeField] private int snackPos = -1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateNewSnack();

        if (makingSnack)
        {
            if (Input.GetKeyDown(KeyCode.D))
                MoveNext();

            if (Input.GetKeyDown(KeyCode.A))
                MoveBack();
        }
    }

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
        makingSnack = true;
        mover.SetActive(true);
    }

    public void FlavorDrinkGreen()
    {
        if (snackPos != 1)
            return;

        switch (curSnack.GetComponent<Snack>().flavor)
        {
            case Snack.Flavor.Nothing:
                curSnack.GetComponent<Snack>().flavor = Snack.Flavor.Green;
                break;
            case Snack.Flavor.Green:
                curSnack.GetComponent<Snack>().flavor = Snack.Flavor.Green;
                break;
            case Snack.Flavor.Red:
                curSnack.GetComponent<Snack>().flavor = Snack.Flavor.Blend;
                break;
            case Snack.Flavor.Blend:
                curSnack.GetComponent<Snack>().flavor = Snack.Flavor.Blend;
                break;
        }

        Debug.Log("Added green flavor");
    }

    public void FlavorDrinkRed()
    {
        if (snackPos != 2)
            return;

        switch (curSnack.GetComponent<Snack>().flavor)
        {
            case Snack.Flavor.Nothing:
                curSnack.GetComponent<Snack>().flavor = Snack.Flavor.Red;
                break;
            case Snack.Flavor.Green:
                curSnack.GetComponent<Snack>().flavor = Snack.Flavor.Blend;
                break;
            case Snack.Flavor.Red:
                curSnack.GetComponent<Snack>().flavor = Snack.Flavor.Red;
                break;
            case Snack.Flavor.Blend:
                curSnack.GetComponent<Snack>().flavor = Snack.Flavor.Blend;
                break;
        }

        Debug.Log("Added green flavor");
    }

    public void AddMilk()
    {
        if (snackPos != 3)
            return;

        curSnack.GetComponent<Snack>().hasMilk = true;
        Debug.Log("Added milk");
    }

    public void AddBoba()
    {
        if (snackPos != 3)
            return;

        curSnack.GetComponent<Snack>().hasBoba = true;
        Debug.Log("Added Boba");
    }

    public void AddSnack(string type)
    {
        if (snackPos != 4)
            return;

        switch (type)
        {
            case "Nothing":
                curSnack.GetComponent<Snack>().snackType = Snack.SnackType.Nothing;
                break;
            case "Pop":
                curSnack.GetComponent<Snack>().snackType = Snack.SnackType.Pop;
                break;
            case "Ring":
                curSnack.GetComponent<Snack>().snackType = Snack.SnackType.Ring;
                break;
            case "Square":
                curSnack.GetComponent<Snack>().snackType = Snack.SnackType.Square;
                break;
        }

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
        snackPos = -1;
    }
}
