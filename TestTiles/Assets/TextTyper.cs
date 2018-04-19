using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : MonoBehaviour {

	public Text uiText;

	public float typeRate = 0.5f;

	string textToType = "";
	int textCharacter = 0;
	public int letterRate = 3;

	string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

	public delegate void OnStartTyping ();
	public OnStartTyping onStartTyping;

	public delegate void OnStopTyping ();
	public OnStopTyping onStopTyping;

	public void Clear () {
		uiText.text = "";
	}

	public virtual void Start () {
		Clear ();
	}

	public IEnumerator TypeCoroutine () {

		if (onStartTyping != null )
			onStartTyping ();

		textCharacter = 1;

		int load = 0;

		while ( true ) {

			textCharacter += letterRate;
			if (textCharacter >= textToType.Length)
				break;
			
			++load;

			if ( load > typeRate ) {

				string randomLetters = "" +
					alphabet [Random.Range (0, alphabet.Length)] +
					alphabet [Random.Range (0, alphabet.Length)] +
					alphabet [Random.Range (0, alphabet.Length)] +
					alphabet [Random.Range (0, alphabet.Length)];

				load = 0;
				uiText.text = textToType.Remove (textCharacter) + randomLetters + "_";

				Sound.Instance.PlayRandomTypeSound ();
				yield return new WaitForSeconds (typeRate);
			}
		}

		uiText.text = textToType;

		textToType = "";

		Sound.Instance.PlayRandomComputerSound ();

		if (onStopTyping != null )
			onStopTyping ();

	}


	public void AddToText (string str) {
		textToType += str;
	}
	public void SkipLign () {
		textToType += "\n";
	}
	public void SkipLign(int c) {
		for (int i = 0; i < c; i++) {
			SkipLign ();
		}
	}
	public void UpdateText () {
		Clear ();
		StartCoroutine (TypeCoroutine ());
	}
}
