using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Door : MonoBehaviour {

    public static Door Instance;

    Animator animator;

    public float openTime = 1f;

    public AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}

    public void Open()
    {
        SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Door_Open);
        animator.SetBool("opened", true);
    }

    public void Close()
    {
        SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Door_Close);
        animator.SetBool("opened", false);
    }
}
