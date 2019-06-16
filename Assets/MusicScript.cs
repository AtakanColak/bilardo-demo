using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
public class MusicScript : MonoBehaviour
{

    public AudioSource audioData;
    private Slider slider;

    private void GetSlider() {
        slider = GameObject.Find("SoundSlider").GetComponent<Slider>();
    }

    public void Start()
    {
        GetSlider();
        SceneManager.sceneLoaded += OnNewSceneLoaded;
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

    void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    //public void ToogleMusic()
    //{
    //    audioData.mute = !audioData.mute;
    //}
}
