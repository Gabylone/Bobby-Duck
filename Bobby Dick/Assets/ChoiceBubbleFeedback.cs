using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBubbleFeedback : MonoBehaviour {

	private string[] bubblePhrases = new string[8] {
		"(partir)",
		"(attaquer)",
		"(trade)",
		"(autre)",
		"(dormir)",
		"(nouveau membre)",
		"(loot)",
		"(quete)"
	};


	public string SetSprite (string str) {

		int index = 0;

		foreach (var bubblePhrase in bubblePhrases) {

			if ( str.EndsWith(bubblePhrase) ) {
				

				GetComponent<Image>().enabled = true;
				GetComponent<Image>().sprite = ChoiceManager.feedbackSprites [index];

				if ( str.Length == bubblePhrase.Length ) {
					return str;
				}

				return str.Remove (str.Length-bubblePhrase.Length);
			}

			++index;

		}


		GetComponent<Image>().enabled = false;

		return str;

	}
}
