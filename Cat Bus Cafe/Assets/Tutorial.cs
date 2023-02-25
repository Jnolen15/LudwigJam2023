using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [System.Serializable]
    public class TutSegment
    {
        [TextArea]
        public string sentence;
        public GameObject arrow;
        public bool lookAtSnack;
    }

    public List<TutSegment> tutorialList = new List<TutSegment>();
    [SerializeField] private int textPos;

    // Yea I just yoinked DialogueManager script to modify it some. Running low on time
    [SerializeField] private CameraControl cam;
    [SerializeField] private MainMenu mm;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI dlogText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private float typingSpeed;
    [SerializeField] bool textisOpen;
    [SerializeField] bool isTypying;
    [SerializeField] bool isFinished;
    [SerializeField] bool lookSnack;
    string curText;

    private void Start()
    {
        ToggleDialogue(true);

        // Text stuff
        isFinished = false;
        dlogText.text = "";
        TypeNextDialogue(tutorialList[textPos]);
    }

    public void ProgressText()
    {
        // skip text
        if (isTypying)
        {
            StopAllCoroutines();
            dlogText.text = curText;
            isFinished = true;
            isTypying = false;
        }
        // Go to next if finished
        else if (isFinished)
        {
            // Hide past arrows
            if (tutorialList[textPos].arrow != null)
                tutorialList[textPos].arrow.SetActive(false);

            textPos++;

            // Return to main when over
            if (textPos >= tutorialList.Count)
            {
                mm.Main();
                return;
            }

            // Text stuff
            isFinished = false;
            dlogText.text = "";
            TypeNextDialogue(tutorialList[textPos]);

            // Move camera
            if (lookSnack != tutorialList[textPos].lookAtSnack)
            {
                lookSnack = tutorialList[textPos].lookAtSnack;

                if (lookSnack)
                    cam.MoveToSnack();
                else
                    cam.MoveToMain();
            }
        }
    }

    // Toggles the dialogue open and closed
    public void ToggleDialogue(bool toggle)
    {
        textisOpen = toggle;
        textBox.SetActive(toggle);

        if (!toggle)
        {
            curText = "";
            dlogText.text = "";
            isTypying = false;
            isFinished = false;
        }
    }

    // Refrenced by other scripts to type text
    public void TypeNextDialogue(TutSegment segment)
    {
        if (!textisOpen)
            ToggleDialogue(true);

        if (!isTypying)
        {
            curText = segment.sentence;
            nameText.text = "Coots";
            StartCoroutine(TypeLineCharacters(curText));

            if(segment.arrow != null)
                segment.arrow.SetActive(true);
        }
    }

    // Animate text
    IEnumerator TypeLineCharacters(string line)
    {
        isTypying = true;

        dlogText.text = "";
        int charCount = 0;

        char[] letterArray = line.ToCharArray();

        //yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < line.Length; i++)
        {
            dlogText.text += letterArray[i];

            //if (0 == charCount % 2 && letterArray[i] != ' ')
            //    uiReferences.boop.Play();
            charCount++;

            yield return new WaitForSecondsRealtime(typingSpeed);

            //i = line.Length;
        }

        isTypying = false;
        isFinished = true;
    }
}
