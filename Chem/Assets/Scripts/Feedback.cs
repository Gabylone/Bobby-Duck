using UnityEngine;
using System.Collections;

public class Feedback : MonoBehaviour {

	public static Feedback Instance;

	public float decalY = 1f;

	[SerializeField]
	private GameObject feedback;

	bool visible = false;

	Transform target;

	void Awake () {
		Instance = this;
	}

	void Update () 
	{
		if ( visible ) {

			feedback.transform.position = target.position + (Vector3.up * decalY);

		}
	}

	void Start () {

		Hide ();

		Interactable.onEnterInteractable += HandleOnEnterInteractable;
		Interactable.onExitInteractable += HandleOnExitInteractable;
	}

	void HandleOnExitInteractable ()
	{
		Hide ();
	}

	void HandleOnEnterInteractable (Transform target)
	{
		Show ();

		this.target = target;
	}

	void Hide () {
		feedback.SetActive (false);
		visible = false;
	}

	void Show () {
		Tween.Bounce (feedback.transform);
		feedback.SetActive (true);
		visible = true;
	}


}
