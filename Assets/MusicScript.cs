using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
public class MusicScript : MonoBehaviour
{

    static MusicScript instance;

    public AudioSource audioData;
    private Slider slider;
    private Image imgmusic;
    private Sprite musicon;
    private Sprite musicoff;


    private void LoadSprites() {
        musicon = Resources.Load<Sprite>("musicon");
        musicoff = Resources.Load<Sprite>("musicoff");
    }
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
        
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        GameObject gmobj = GameObject.Find("BGMusic");
        GetSlider();
        GetToggler();
        LoadSprites();
        SceneManager.sceneLoaded += OnNewSceneLoaded;
        audioData = GetComponent<AudioSource>();
        audioData.volume = slider.value;
        audioData.Play(0);
        audioData.loop = true;
        
    }

    void SoundSlide()
    {
        audioData.volume = slider.value;
    }

    void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetSlider();
        GetToggler();
        slider.value = audioData.volume;
        imgmusic.sprite = audioData.mute ? musicoff : musicon;
    }

    public void ToggleMusic()
    {
        audioData.mute = !audioData.mute;
        imgmusic.sprite = audioData.mute ? musicoff : musicon; 
    }
}
