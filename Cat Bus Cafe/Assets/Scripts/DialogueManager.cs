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

    [SerializeField] private AudioClip talk1;
    [SerializeField] private AudioClip talk2;
    [SerializeField] private AudioClip talk3;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
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

            var randSound = Random.Range(0, 3);
            if (randSound == 0)
                audioSource.PlayOneShot(talk1);
            else if (randSound == 1)
                audioSource.PlayOneShot(talk2);
            else if (randSound == 1)
                audioSource.PlayOneShot(talk3);
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

            if (0 == charCount % 6)
            {
                var randSound = Random.Range(0, 3);
                if (randSound == 0)
                    audioSource.PlayOneShot(talk1);
                else if (randSound == 1)
                    audioSource.PlayOneShot(talk2);
                else if (randSound == 1)
                    audioSource.PlayOneShot(talk3);
            }
            charCount++;

            yield return new WaitForSecondsRealtime(typingSpeed); 
            
            //i = line.Length;
        }

        isTypying = false;
        isFinished = true;
    }
}
