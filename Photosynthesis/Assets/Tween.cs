using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Tween : MonoBehaviour {

	public static float dur = 0.2f;
	public static float amount = 1.2f;

	public static void Bounce ( Transform transform , float _dur , float _amount ) {
		HOTween.To ( transform , _dur/2f , "localScale" , Vector3.one * _amount , false , EaseType.EaseOutBounce , 0f );
		HOTween.To ( transform , _dur/2f , "localScale" , Vector3.one , false , EaseType.Linear , dur/2f );

	}
//	public static void Bounce ( Transform transform , float _dur ) {
//		Bounce (transform, _dur , amount );
//	}
	public static void Bounce ( Transform transform , float _amount ) {
		Bounce (transform, dur , _amount );
	}
	public static void Bounce ( Transform transform ) {
		Bounce ( transform , dur , amount );
	}

}
