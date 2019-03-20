using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public static Music Instance;

    public AudioSource source;

    public AudioClip rushHourClip;
    public AudioClip menuClip;


    public bool play = true;

    private void Awake()
    {
        Instance = this;
    }   

    public void PlayRushHour()
    {
        source.clip = rushHourClip;
        source.Play();
    }

    public void PlayMenu()
    {
        source.clip = menuClip;
        source.Play();
    }
}
