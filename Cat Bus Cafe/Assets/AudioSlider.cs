using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private string paramName;
    [SerializeField] private float defaultVal;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(paramName, defaultVal);
    }

    public void SetAudioLevel(float sliderValue)
    {
        mixer.SetFloat(paramName, Mathf.Log10(sliderValue) * 20);

        PlayerPrefs.SetFloat(paramName, sliderValue);
    }
}
