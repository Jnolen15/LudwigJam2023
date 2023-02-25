using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private DialogueManager dlog;
    private GameManager gManager;

    [System.Serializable]
    public class SnackOrder
    {
        public Snack.SnackType snacky;
        public Snack.Flavor flava;
        public bool wifMilk;
        public bool wifBoba;

        public void PrintOrder()
        {
            string milk = "";
            if (wifMilk)
                milk = " with milk";
            else
                milk = " no milk";

            string boba = "";
            if (wifBoba)
                boba = " with boba.";
            else
                boba = " no boba.";

            Debug.Log("Snack: " + snacky + " Flava: " + flava + milk + boba);
        }
    }
    
    public string cName;
    public GameObject popUp;
    public GameObject excalamtionMarker;
    public GameObject Pet1;
    public GameObject Pet2;
    public GameObject trash;
    public SnackOrder newSnack = new SnackOrder();
    public string requestMessage;
    public bool hasOrdered;
    public bool waitingForOrder;
    private bool noDrink;
    private bool petRequested;
    private bool inPetAnim;
    private bool inPetTime;
    [SerializeField] private float petTime;
    private float petTimer;
    private int numPets;

    private void Start()
    {
        dlog = GameObject.FindGameObjectWithTag("UI").GetComponent<DialogueManager>();
        gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (inPetTime)
        {
            if (petTimer > 0) 
                petTimer -= Time.deltaTime;
            else
                EndPet();
        }
    }

    private void OnMouseDown()
    {
        // Pet time stuff
        if (petRequested)
        {
            excalamtionMarker.SetActive(false);
            petRequested = false;
            StartPet();
        }

        if (inPetTime && !inPetAnim)
        {
            StopAllCoroutines();
            StartCoroutine(PetAnim());
            numPets++;
        }

        // Snack dialogue stuff
        if (waitingForOrder)
        {
            var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
            pc.GiveMeSnack(this);
            hasOrdered = false;
            waitingForOrder = false;
        }

        if (hasOrdered)
        {
            dlog.TypeDialogue(requestMessage, cName);
            excalamtionMarker.SetActive(false);
            waitingForOrder = true;
        }
    }

    public void React(float time, string emotion)
    {
        Vector3 emojiSpawnPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        var pop = Instantiate(popUp, emojiSpawnPos, transform.rotation);
        pop.GetComponent<Popup>().Setup(time, emotion);
    }

    // ========= TRASH STUFF =========
    public void ThrowTrash()
    {
        var garbage = Instantiate(trash, transform.position, transform.rotation);
        garbage.GetComponent<Rigidbody>().AddForce(new Vector3(0, 2, -3), ForceMode.Impulse);
    }
    

    // ========= PET STUFF =========
    public void RequestPet()
    {
        petRequested = true;
        excalamtionMarker.SetActive(true);
    }

    public void StartPet()
    {
        inPetTime = true;
        petTimer = petTime;
    }

    public void EndPet()
    {
        StopAllCoroutines();
        inPetTime = false;
        inPetAnim = false;
        Pet2.SetActive(false);
        Pet1.SetActive(false);
        gManager.UpdatePoints(new string("Pets " + numPets), numPets / 5);
        React(2f, "Happy");
        numPets = 0;
        transform.GetChild(0).localScale = new Vector3(1, 1, 1);
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
    }

    // ========= SNACK STUFF =========
    public void GiveSnackOrder(SnackOrder order)
    {
        waitingForOrder = false;

        Debug.Log("I asked for ");
        newSnack.PrintOrder();

        Debug.Log("You gave me ");
        order.PrintOrder();

        int score = CalculateScore(order);

        // Response
        string response = "";
        string emote = "";
        if (score == 0)
        {
            response += "This isn't what I asked for at all!";
            emote = "Angry";
        }
        else if (score == 1)
        {
            response += "I think you gave me the wrong order?";
            emote = "Spooked";
        }
        else if (score == 2)
        {
            response += "I guess this works.";
            emote = "Sad";
        }
        else if (score == 3)
        {
            response += "Not quite what I ordered, but thanks.";
            emote = "Neutral";
        }
        else if (score == 4)
        {
            response += "Thanks.";
            emote = "Happy";
        }
        else if (score == 5)
        {
            response += "Perfect! Thank you.";
            emote = "Pog";
        }

        response += (" " + score + "/5");

        React(3f, emote);
        dlog.TypeDialogue(response, cName);
        gManager.UpdatePoints("Treat Made", score);
    }

    private int CalculateScore(SnackOrder order)
    {
        int score = 5;

        // IF cat did not order a drink
        if (noDrink)
        {
            // And they were given one
            if (order.flava != Snack.Flavor.Nothing || order.wifMilk || order.wifBoba)
                score -= 3;
        }
        else
        {
            // They wern't given a drink
            if (order.flava == Snack.Flavor.Nothing && !order.wifMilk && !order.wifBoba)
            {
                score -= 3;
            }
            // They were
            else
            {
                // Did the flavor match?
                if (order.flava != newSnack.flava)
                    score--;
                // Did the toppings match?
                if (order.wifMilk != newSnack.wifMilk)
                    score--;
                if (order.wifBoba != newSnack.wifBoba)
                    score--;
            }
        }

        // Did the snack match?
        if (order.snacky != newSnack.snacky)
        {
            // If it diddn't and thats all they wanted loose more
            if (noDrink)
                score -= 3;
            else
                score--;
        }

        // Lowest possible is 0
        if (score < 0) score = 0;

        Debug.Log("Your score: " + score);
        return score;
    }

    public void RequestSnack()
    {
        // Only request if havnt ordered or not waiting for an order
        if (hasOrdered || waitingForOrder)
            return;

        excalamtionMarker.SetActive(true);

        hasOrdered = true;
        noDrink = false;
        requestMessage = "";

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
                rand = Random.Range(0, 4);
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
            gManager.UpdatePoints("Free Points?", 10);
            waitingForOrder = false;
            hasOrdered = false;
        }
    }
}
