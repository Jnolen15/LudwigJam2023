using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [System.Serializable]
    public class SnackOrder
    {
        public Snack.SnackType snacky;
        public Snack.Flavor flava;
        public bool wifMilk;
        public bool wifBoba;
    }
    public SnackOrder newSnack = new SnackOrder();

    private void OnMouseDown()
    {
        RequestSnack();

        newSnack.snacky = Snack.SnackType.Nothing;
        newSnack.flava = Snack.Flavor.Nothing;
        newSnack.wifMilk = false;
        newSnack.wifBoba = false;
    }

    public void RequestSnack()
    {
        bool noDrink = false;
        string requestMessage = "";
        //SnackOrder newSnack = new SnackOrder();

        // Intro
        var rand = Random.Range(0, 4);
        if (rand == 0)
            requestMessage += "Could I please get a";
        else if (rand == 1)
            requestMessage += "I'd like to order a";
        else if (rand == 2)
            requestMessage += "Yo, get me a";
        else if (rand == 3)
            requestMessage += "Give me your best";

        // Pick flavor
        rand = Random.Range(0, 4);
        if (rand > 0)
        {
            rand = Random.Range(1, 4);
            if (rand == 1)
            {
                newSnack.flava = Snack.Flavor.Green;
                requestMessage += " catnip";
            }
            else if (rand == 2)
            {
                newSnack.flava = Snack.Flavor.Red;
                requestMessage += " strawberry";
            }
            else if (rand == 3)
            {
                newSnack.flava = Snack.Flavor.Blend;
                requestMessage += " blended";
            }
        }

        if(newSnack.flava != Snack.Flavor.Nothing)
            requestMessage += " flavor drink";
        else
            requestMessage += " cup";

        // With Milk
        rand = Random.Range(0, 2);
        if (rand == 0)
            newSnack.wifMilk = false;
        else if (rand == 1)
        {
            newSnack.wifMilk = true;
            requestMessage += " with milk";
        }

        // With Boba
        rand = Random.Range(0, 2);
        if (rand == 0)
            newSnack.wifBoba = false;
        else if (rand == 1)
        {
            newSnack.wifBoba = true;
            if(newSnack.wifMilk)
                requestMessage += " and";
            else
                requestMessage += " with";
            requestMessage += " boba";
        }

        // If no drink
        if (newSnack.flava == Snack.Flavor.Nothing && !newSnack.wifMilk && !newSnack.wifBoba)
        {
            noDrink = true;
        }

        // Pick Snack
        rand = Random.Range(0, 4);
        if (rand > 0)
        {
            // Fix text if they diddn't order a drink
            if (noDrink)
            {
                requestMessage = "Im not too thirsty, but could I have just a";
            }
            else
                requestMessage += ".";

            // Intermission
            if (!noDrink)
            {
                // Intro
                rand = Random.Range(0, 3);
                if (rand == 0)
                    requestMessage += " Oh, and I'll have a";
                else if (rand == 1)
                    requestMessage += " Can I also get a";
                else if (rand == 2)
                    requestMessage += " Throw in a";
                else if (rand == 3)
                    requestMessage += " I'll take a treat too, how about a";
            }

            rand = Random.Range(1, 4);
            if (rand == 1)
            {
                newSnack.snacky = Snack.SnackType.Square;
                requestMessage += " cookie";
            }
            else if (rand == 2)
            {
                newSnack.snacky = Snack.SnackType.Pop;
                requestMessage += " cake pop";
            }
            else if (rand == 3)
            {
                newSnack.snacky = Snack.SnackType.Ring;
                requestMessage += " doughnut";
            }

            if (!noDrink)
                requestMessage += " with it";
        }

        // Outro
        rand = Random.Range(0, 4);
        if (rand == 0)
            requestMessage += ", thanks!";
        else if (rand == 1)
            requestMessage += ". Meow.";
        else if (rand == 2)
            requestMessage += ". On the double.";
        else if (rand == 3)
            requestMessage += ". Right meow!";

        // They want no drink and no snack
        if (noDrink && newSnack.snacky == Snack.SnackType.Nothing)
        {
            requestMessage = "Could I get a... uh... um... wait I'm not hungry. Ok nevermind then.";
        }

        Debug.Log(requestMessage);
    }
}
