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
    private Image imgmusic;
    private Sprite musicon;
    private Sprite musicoff;

    private void GetSlider()
    {
        slider = GameObject.Find("SoundSlider").GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { SoundSlide(); });
    }

    private void GetToggler()
    {
        Button toggler = GameObject.Find("MusicButton").GetComponent<Button>();
        toggler.onClick.AddListener(delegate { ToggleMusic(); });
        imgmusic = GameObject.Find("MusicImage").GetComponent<Image>();
        musicon = Resources.Load<Sprite>("musicon");
        musicoff = Resources.Load<Sprite>("musicoff");
    }

    public void Start()
    {
        GetSlider();
        GetToggler();
        SceneManager.sceneLoaded += OnNewSceneLoaded;
        audioData = GetComponent<AudioSource>();
        audioData.volume = slider.value;
        audioData.Play(0);
        audioData.loop = true;
        DontDestroyOnLoad(this.gameObject);
    }

    void SoundSlide()
    {
        audioData.volume = slider.value;
    }

    void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New Scene Loaded and MusicScript detected it.");
    }

    public void ToggleMusic()
    {
        Debug.Log("Music Toggle");
        audioData.mute = !audioData.mute;
        imgmusic.sprite = audioData.mute ? musicoff : musicon; 
    }
}
