using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewInfo : MonoBehaviour {

	[Header("Info")]
	[SerializeField] private Text targetText;
	[SerializeField] private float duration;
	[SerializeField] private float decalY = 1f;
	[SerializeField] private float targetScale = 1.5f;
	private Vector3 initPos = Vector3.zero;

	bool displaying = false;

	float timer = 0f;

	void Start () {

		initPos = targetText.transform.position;

		targetText.gameObject.SetActive (false);

	}

	void Update () {
		if (displaying) {
			timer += Time.deltaTime;

			float l = timer/duration;

			targetText.transform.position = Vector3.Lerp ( initPos , initPos + Vector3.up * decalY, l );
			targetText.transform.localScale = Vector3.Lerp ( Vector3.one , Vector3.one * targetScale, l );

			if ( l >= 1 )
				EndDisplay ();
		}
	}

	public void DisplayInfo (string info) {
		targetText.gameObject.SetActive (true);

		timer = 0f;
		displaying = true;

		targetText.text = info;
	}

	private void EndDisplay () {

		targetText.gameObject.SetActive (false);
		displaying = false;
	}

	private CrewID crewID;
	public CrewID getCrewID {
		get {
			return crewID;
		}
		set {
			crewID = value;
		}
	}
}

public class CrewID {
	public int faceID = 0;
	public int beardID = 0;
	public int hairID = 0;
	public Color skinColor;

	public CrewID (
		int face,
		int beard,
		int hair,
		Color skin )
	{
		faceID = face;
		beardID = beard;
		hairID = hair;
		skinColor = skin;
	}
}
