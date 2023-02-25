using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI dlogText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private float typingSpeed;
    string curText;
    [SerializeField] bool textisOpen;
    [SerializeField] bool isTypying;
    [SerializeField] bool isFinished;

    private void Update()
    {
        if (!textisOpen)
            return;
    }

    public void ProgressText()
    {
        Debug.Log("Click");

        // skip text
        if (isTypying)
        {
            StopAllCoroutines();
            dlogText.text = curText;
            isFinished = true;
            isTypying = false;
        }
        // Close if finished
        else if (isFinished)
        {
            ToggleDialogue(false);
            dlogText.text = "";
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
    public void TypeDialogue(string words, string name)
    {
        if (!textisOpen)
            ToggleDialogue(true);

        if (!isTypying)
        {
            curText = words;
            nameText.text = name;
            StartCoroutine(TypeLineCharacters(curText));
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
