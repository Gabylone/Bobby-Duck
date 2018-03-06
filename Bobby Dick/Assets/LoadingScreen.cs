using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	public GameObject group;
	public Text loadText1;
	public RectTransform backGround;
	public RectTransform fillRectTransform;

	int max;
	float w = 0;

	public static LoadingScreen Instance;

	void Awake () {
		Instance = this;
	}

	void Start () {
		w = backGround.rect.width;
//		Hide ();
	}

	public void StartLoading ( string title , int max ) {
		
		Show ();
		CancelInvoke ("Hide");
		Tween.ClearFade (group.transform);

		loadText1.text = title;

		this.max = max;

		UpdateImage (0);
	}

	public void Push (int currentLoad) {

		UpdateImage (currentLoad);

	}

	void UpdateImage (int c) {

		float l = (float)c / (float)max;

		fillRectTransform.sizeDelta = new Vector2 ( -w + ( l * w ) , 0f );

	}

	public void End ()
	{
		UpdateImage (max);
		Invoke ("Hide",1f);
		Tween.Fade(group.transform,0.4f);
		Tween.Bounce (group.transform,0.4f,1.05f);
	}

	void Hide () 
	{

		group.SetActive (false);
	}

	void Show ()
	{
		group.SetActive (true);
	}
}
