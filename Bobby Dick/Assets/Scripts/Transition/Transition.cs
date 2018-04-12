using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class Transition : MonoBehaviour {

		// lerp
	public Color targetColor;

	[SerializeField]
	private Image targetImage;

	[SerializeField]
	private GameObject transitionCanvas;

	public void FadeIn (float duration)
	{
		transitionCanvas.SetActive (true);
		targetImage.color = Color.clear;
		HOTween.To (targetImage , duration, "color" , targetColor);
	}
	public void FadeOut (float duration)
	{
		targetImage.color = targetColor;
		HOTween.To (targetImage , duration, "color" , Color.clear);
		Invoke ("FadeOutDelay", duration);
	}
	void FadeOutDelay () {
		transitionCanvas.SetActive (false);
	}

}
