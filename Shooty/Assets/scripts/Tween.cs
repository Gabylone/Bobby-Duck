using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Tween : MonoBehaviour {

	static float defaultDuration = 0.2f;

	static float defaultScale = 1.2f;

	public static void Bounce ( Transform tr ) {
		Bounce (tr, defaultDuration, defaultScale);
	}

	public static void Bounce ( Transform tr , float dur) {
		Bounce (tr, dur, defaultScale);
	}

	public static void Bounce ( Transform tr , float dur , float scale ) {
		HOTween.To (tr, defaultDuration/2f, "localScale", Vector3.one * defaultScale, false, EaseType.EaseOutBounce, 0f);
		HOTween.To (tr, defaultDuration/2f, "localScale", Vector3.one, false, EaseType.Linear, defaultDuration/2f);
	}

}
