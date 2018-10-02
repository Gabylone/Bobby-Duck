using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Tween : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	static float dur = 0.4f;
	static float amount = 1.5f;
	public static void Scale (Transform t) {
		HOTween.To (t , dur , "localScale" , Vector3.one * amount , false , EaseType.EaseOutBounce , 0f);
	}
    public static void Scale(Transform t, float _amount )
    {
        HOTween.To(t, dur, "localScale", Vector3.one * _amount, false, EaseType.EaseOutBounce, 0f);
    }

    public static void Descale (Transform t) {
		HOTween.To (t , dur , "localScale" , Vector3.one, false , EaseType.Linear , 0f);
	}

	public static void Bounce (Transform t) {
		HOTween.To (t , dur/2f , "localScale" , Vector3.one * amount , false , EaseType.EaseOutBounce , 0f);
		HOTween.To (t , dur/2f , "localScale" , Vector3.one, false , EaseType.Linear , dur/2f);
	}
}
