using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayCrew : MonoBehaviour {

	public GameObject targetGameObject;

	public RectTransform rectTransform;

	public float duration = 1f;
	public float decal = 200f;

	Vector2 initPos;

	// Use this for initialization
	void Start () {
		CrewInventory.Instance.openInventory += HandleOpenInventory;
		CrewInventory.Instance.closeInventory += HandleCloseInventory;

		Hide ();

		initPos = rectTransform.anchoredPosition;
	}

	void HandleCloseInventory ()
	{
		CancelInvoke ("Hide");
		CancelInvoke ("ShowDelay");
		HOTween.To (rectTransform, duration, "anchoredPosition", initPos + Vector2.up * decal, false);
		Invoke ("Hide",duration);
	}

	void Hide () {
		targetGameObject.SetActive (false);
	}

	void HandleOpenInventory (CrewMember member)
	{
		HOTween.To (rectTransform, duration, "anchoredPosition", initPos, false);

		targetGameObject.SetActive (true);

		CancelInvoke ("Hide");
		CancelInvoke ("ShowDelay");
		Invoke ("ShowDelay",duration);
	}

	void ShowDelay () {
		Tween.Bounce (transform,0.05f,1.03f);

	}

}
