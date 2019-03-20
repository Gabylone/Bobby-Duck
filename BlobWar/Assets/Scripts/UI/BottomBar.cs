using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class BottomBar : MonoBehaviour {

    public static BottomBar Instance;

    public float duration = 0.2f;

    public float decalY = 50f;

    Vector3 initPos = Vector3.zero;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        initPos = transform.position;
	}

    public void Down()
    {
        HOTween.To( transform , duration , "position" , initPos + Vector3.up * decalY , false , EaseType.Linear , 0f );
    }

    public void Up()
    {
        HOTween.To(transform, duration, "position", initPos, false, EaseType.Linear, 0f);
    }
}
