using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class InfoFeedbacks : MonoBehaviour {

	public GameObject group;

	public RectTransform rectTransform;
	public Text text;
	public Image image;

	public float decalY = 30f;

	public float duration = 2f;

    Vector3 initPos;

	// Use this for initialization
	public virtual void Start () {

		Hide ();


        initPos = rectTransform.localPosition;
        //rectTransform.transform.SetParent(transform.)

	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            Print("soir");
        }
    }
    public virtual void Print ( string str ) {
		Print (str, Color.white);
	}
	public virtual void Print ( string str , Color color ) {
		
		Show ();


		rectTransform.localPosition = initPos;

		HOTween.Kill (rectTransform);
		HOTween.Kill (transform);


		Tween.ClearFade (transform);

		CancelInvoke ("Hide");
		CancelInvoke ("Fade");

		HOTween.To (rectTransform, duration, "localPosition", initPos + Vector3.up * decalY);

		text.text = str;
		image.color = color;

		Invoke ("Fade",duration/2f);
	}

	void Fade () {
		Tween.Fade (transform, duration/2f);

		Invoke ("Hide", duration/2f);
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}
