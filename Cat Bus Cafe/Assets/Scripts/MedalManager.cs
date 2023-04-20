using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MedalManager : MonoBehaviour
{
    [SerializeField] private GameObject MedalGroup;
    [SerializeField] private GameObject newMedalText;
    [SerializeField] private Image medalImage;
    [SerializeField] private GameObject nextMedalGroup;
    [SerializeField] private TextMeshProUGUI nextMedalScore;
    [SerializeField] private int medalTier;

    public List<MedalInfo> medals = new List<MedalInfo>();

    [System.Serializable]
    public class MedalInfo
    {
        public int score;
        public Sprite image;
    }

    public void UpdateMedals(int score)
    {
        int curmedal = PlayerPrefs.GetInt("MedalTier", -1);
        int thisMedal = -1;

        // Find highest medal earned
        for (int i = 0; i < medals.Count; i++)
        {
            if (score >= medals[i].score)
                thisMedal = i;
        }

        Debug.Log("This medal: " + thisMedal + " current best medal " + curmedal);

        // Have not reached first medal
        if (thisMedal == -1 && curmedal == -1)
        {
            MedalGroup.SetActive(false);
        }
        // If new medal earned
        else if (thisMedal > curmedal)
        {
            newMedalText.SetActive(true);
            medalImage.sprite = medals[thisMedal].image;
            PlayerPrefs.SetInt("MedalTier", thisMedal);
        }
        else
        {
            medalImage.sprite = medals[curmedal].image;
        }

        // If dont have the highest medal teir, show required score
        curmedal = PlayerPrefs.GetInt("MedalTier", -1);
        if (curmedal >= medals.Count-1)
        {
            nextMedalGroup.SetActive(false);
        } else
        {
            Debug.Log("Next medal teir " + curmedal+1);
            nextMedalScore.text = medals[curmedal+1].score.ToString();
        }
    }
}
