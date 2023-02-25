using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private Sprite angryEmote;
    [SerializeField] private Sprite spookedEmote;
    [SerializeField] private Sprite sadEmote;
    [SerializeField] private Sprite neutralEmote;
    [SerializeField] private Sprite happyEmote;
    [SerializeField] private Sprite pogEmote;
    [SerializeField] private Sprite alertEmote;
    private SpriteRenderer sr;

    public void Setup(float speed, string emote)
    {
        sr = gameObject.GetComponent<SpriteRenderer>();

        if (emote == "Angry")
            sr.sprite = angryEmote;
        else if (emote == "Spooked")
            sr.sprite = spookedEmote;
        else if (emote == "Sad")
            sr.sprite = sadEmote;
        else if (emote == "Neutral")
            sr.sprite = neutralEmote;
        else if (emote == "Happy")
            sr.sprite = happyEmote;
        else if (emote == "Pog")
            sr.sprite = pogEmote;
        else if (emote == "Alert")
            sr.sprite = alertEmote;


        StartCoroutine(Animate(speed));
    }

    IEnumerator Animate(float speed)
    {
        float time = 0;

        float startValue = sr.color.a;
        Vector3 startPos = this.transform.position;
        Vector3 endPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        while (time < speed)
        {
            float t = time / speed;
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(startPos, endPos, t);
            float newAlpha = Mathf.Lerp(startValue, 0, t);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
