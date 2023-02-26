using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer sfxMixer;
    
    public void OnChangeSider(float value)
    {
        sfxMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
    }
}
