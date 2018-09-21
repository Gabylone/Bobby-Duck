using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance;

    public AudioSource ambianceSource;
    public AudioSource musicSource;
    public AudioSource defaultAudioSource;

    public AudioClip[] clips;

    public bool playSounds = true;

    public enum SoundType
    {
        Purchase,
        Client_Happy,
        Client_Mad,
        Door_Close,
        Door_Open,
        Eat,
        Ingredient1,
        Ingredient2,
        MoneyBag,
        Plate,
        Slide,
        Star,
        Upgrade,
        UI_Bip,
        UI_Close,
        UI_Open,
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Play ( SoundType type)
    {
        Play(defaultAudioSource,clips[(int)type]);
    }

    public void Play ( AudioSource source, SoundType type)
    {
        Play(source, clips[(int)type]);
    }
    
    public void Play(AudioClip audioClip)
    {
        Play(defaultAudioSource, audioClip);
    }

    public void Play(AudioSource audioSource, AudioClip audioClip)
    {
        if (!playSounds)
        {
            return;
        }
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
