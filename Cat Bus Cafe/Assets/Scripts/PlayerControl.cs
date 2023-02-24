using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameObject curSnack;
    [SerializeField] private Snack curSnackScript;
    [SerializeField] private bool hasSnack;

    public void TakeSnack(GameObject newSnack)
    {
        hasSnack = true;
        curSnack = newSnack;
        curSnackScript = curSnack.GetComponent<Snack>();
    }

    // Deliver snack to cat. When clicking a cat
    public void GiveMeSnack(Cat theCat)
    {
        if (!hasSnack || !theCat.waitingForOrder)
            return;

        Cat.SnackOrder myOrder = new Cat.SnackOrder();

        myOrder.flava = curSnackScript.flavor;
        myOrder.snacky = curSnackScript.snackType;
        myOrder.wifMilk = curSnackScript.hasMilk;
        myOrder.wifBoba = curSnackScript.hasBoba;

        theCat.GiveSnackOrder(myOrder);

        hasSnack = false;
        Destroy(curSnack);
        curSnack = null;
        curSnackScript = null;
        hasSnack = false;
    }

    // Gets rid of snack when click on trash
    public void TossSnack()
    {
        if (curSnack == null)
            return;

        Destroy(curSnack);
        curSnack = null;
        curSnackScript = null;
        hasSnack = false;
    }
}
