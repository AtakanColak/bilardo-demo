using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Slider))]
public class MusicScript : MonoBehaviour
{

    public AudioSource audioData;
    public Slider slider;

    public void Start()
    {
        audioData = GetComponent<AudioSource>();
        audioData.volume = slider.value;
        audioData.Play(0);
        audioData.loop = true;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SoundSlide()
    {
        audioData.volume = slider.value;
    }
}
