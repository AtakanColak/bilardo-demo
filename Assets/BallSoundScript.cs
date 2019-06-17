using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSoundScript : MonoBehaviour
{

    public AudioClip hit;
    private AudioSource source;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        slider = GameObject.Find("SoundSlider").GetComponent<Slider>();
        hit = Resources.Load("hitsound") as AudioClip;
    }

    void OnCollisionEnter(Collision collision)
    {
        source.PlayOneShot(hit, slider.value);
    }
}
