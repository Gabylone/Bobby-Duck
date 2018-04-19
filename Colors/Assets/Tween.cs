using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Tween : MonoBehaviour {

	public static float dur = 0.3f;

	public static float amount = 1.25f;

	public static void Bounce ( Transform t ) {

		HOTween.To (t, dur/2f, "localScale", Vector3.one * amount, false, EaseType.EaseOutBounce, 0f);
		HOTween.To (t, dur/2f, "localScale", Vector3.one, false, EaseType.Linear, dur/2f);

	}
}
