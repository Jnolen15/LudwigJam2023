using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    [SerializeField] Image bronzeSlot;
    [SerializeField] Image silverSlot;
    [SerializeField] Image goldSlot;
    [SerializeField] Image platSlot;
    [SerializeField] Image diamondSlot;
    [SerializeField] Image rainbowSlot;
    [SerializeField] Sprite blank;
    [SerializeField] Sprite bronze;
    [SerializeField] Sprite silver;
    [SerializeField] Sprite gold;
    [SerializeField] Sprite plat;
    [SerializeField] Sprite diamond;
    [SerializeField] Sprite rainbow;

    void Start()
    {
        int curmedal = PlayerPrefs.GetInt("MedalTier", -1);

        if (curmedal >= 0)
            bronzeSlot.sprite = bronze;
        if (curmedal >= 1)
            silverSlot.sprite = silver;
        if (curmedal >= 2)
            goldSlot.sprite = gold;
        if (curmedal >= 3)
            platSlot.sprite = plat;
        if (curmedal >= 4)
            diamondSlot.sprite = diamond;
        if (curmedal >= 5)
            rainbowSlot.sprite = rainbow;
    }
}
