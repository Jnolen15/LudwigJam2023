using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer signRenderer;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private float animDist;
    [SerializeField] private List<AnimPoint> animPoints = new List<AnimPoint>();
    [SerializeField] private int animCount;
    [SerializeField] private float curInterval;
    private BusControl bc;

    [System.Serializable]
    public class AnimPoint
    {
        public Sprite sprite;
        public float interval;
    }

    void Start()
    {
        bc = gameObject.GetComponent<BusControl>();
    }

    void Update()
    {
        if ((bc.nextStop.distance - bc.busLocation) < animDist)
        {
            UpdateSign();
        }

        if (signRenderer.sprite == animPoints[3].sprite)
            indicator.SetActive(true);
        else
            indicator.SetActive(false);
    }

    public void ResetSign()
    {
        animCount = 0;
        curInterval = 0;
        signRenderer.sprite = defaultSprite;
    }

    private void UpdateSign()
    {
        curInterval = (bc.nextStop.distance - bc.busLocation);

        if (animCount >= animPoints.Count)
            return;

        if (curInterval <= animPoints[animCount].interval)
        {
            if (animCount == 3)
                this.GetComponent<AudioSource>().Play();
            signRenderer.sprite = animPoints[animCount].sprite;
            animCount++;
        }
    }
}
