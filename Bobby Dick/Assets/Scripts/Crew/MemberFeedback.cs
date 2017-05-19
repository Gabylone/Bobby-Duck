using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MemberFeedback : MonoBehaviour {

	[Header("Info")]
	[SerializeField] private Transform infoTransform;
	[SerializeField] private Text smallText;
	[SerializeField] private Text bigText;
	[SerializeField] private float duration;
	[SerializeField] private float decalY = 1f;
	[SerializeField] private float targetScale = 1.5f;
	private Vector3 initPos = Vector3.zero;

	CrewIcon icon;

	bool displaying = false;

	float timer = 0f;

	void Start () {
		icon = GetComponent<CrewIcon> ();

		initPos = infoTransform.localPosition;
		infoTransform.gameObject.SetActive (false);

	}

	void Update () {
		if (displaying) {
			timer += Time.deltaTime;

			float l = timer/duration;

			infoTransform.localPosition = Vector3.Lerp ( initPos , initPos + Vector3.up * decalY, l );
			infoTransform.localScale = Vector3.Lerp ( Vector3.one , Vector3.one * targetScale, l );

			if ( l >= 1 )
				EndDisplay ();
		}
	}

	public void DisplayInfo (string smallContent , string bigContent, Color textColor ) {
		infoTransform.gameObject.SetActive (true);

		timer = 0f;
		displaying = true;

		smallText.text = smallContent;
		bigText.text = bigContent;

		bigText.color = textColor;
	}

	private void EndDisplay () {

		infoTransform.gameObject.SetActive (false);
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
