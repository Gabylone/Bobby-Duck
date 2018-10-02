using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

    public static Sound Instance;

    public AudioSource defaultSource;

    public enum Type
    {
        Barricade,
        Buy,
        Equip,
        Fall1,
        Fall2,
        Fall3,
        Fall4,
        Menu1,
        Menu2,
        Menu3,
        Menu4,
        Menu5,
        Menu6,
        Menu7,
        Runaway,
        Sell,
        Shoot,
        Speak1,
        Speak2,
        UI_Correct,
        UI_Wrong,
        Upgrade,
        Zombie1,
        Zombie2,
        Zombie3,
        Zombie4,
        Zombie5,
        Zombie6,
        Zombie7,
        Zombie8,
        Zombie9,
        Zombie10,
        Zombie11,
        Zombie12,
        Zombie13,
    }

    public AudioClip[] audioClips;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioSource source , List<Type> types)
    {
        Type type = types[Random.Range(0, types.Count)];
        PlaySound(source, type);
    }

    public void PlaySound(List<Type> types)
    {
        PlaySound(defaultSource, types);
    }

    public void PlaySound(Type type)
    {
        PlaySound(defaultSource, type);
    }

    public void PlaySound(AudioSource source, Type type)
    {
        source.clip = audioClips[(int)type];

        source.Play();
    }

    internal void PlaySound(AudioSource source , Type type, float v1, float v2)
    {
        source.clip = audioClips[(int)type];
        source.pitch = Random.Range( v1 , v2 );
        source.Play();
    }
}
