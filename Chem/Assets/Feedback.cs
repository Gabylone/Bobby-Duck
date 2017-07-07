using UnityEngine;
using System.Collections;

public class Feedback : MonoBehaviour {

	public static Feedback Instance;

	public float decalY = 1f;

	[SerializeField]
	private GameObject feedback;

	bool visible = false;

	void Awake () {
		Instance = this;
	}

	void Start () {
		Visible = false;
	}

	public void Place (Vector3 p) {
		feedback.transform.position = p + (Vector3.up * decalY);
		Visible = true;
	}

	public bool Visible {
		get {
			return visible;
		}
		set {
			visible = value;

			feedback.SetActive (value);

		}
	}
}
