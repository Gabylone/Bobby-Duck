using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class RayBlocker : MonoBehaviour {

	public GameObject group;
	public Image image;

	public float tweenDur = 1f;
	Color initColor;

	// Use this for initialization
	void Start () {

		initColor = image.color;

		Hide ();

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		CrewInventory.Instance.closeInventory += HandleCloseInventory;;
	}

	void HandleCloseInventory ()
	{
//		HOTween.To ( image , tweenDur , "color" , Color.clear );
//
//		Invoke ("Hide" , tweenDur);

		Hide ();
	}

	void HandleOpenInventory (CrewMember member)
	{
//		HOTween.To ( image , tweenDur , "color" , initColor );
//
//		Show ();

		Show ();
	}

	void Hide ()
	{
		group.SetActive (false);
	}
	void Show () {
		group.SetActive (true);
	}
}
