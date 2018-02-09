using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {

	public static Text text;

	void Awake () {
		text = GetComponentInChildren<Text> ();
	}

	public static void Print (string str) {
		text.text += "\n" + str;
	}
}
