using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInput : MonoBehaviour {

	public InputField inputField;

	void Start () {
		Focus ();

		DisplayDescription.Instance.onStartTyping += HandleOnStartTyping;
		DisplayDescription.Instance.onStopTyping += HandleOnStopTyping;
	}

	void HandleOnStopTyping ()
	{
		gameObject.SetActive (true);
	}

	void HandleOnStartTyping ()
	{
		gameObject.SetActive (false);
	}

	public delegate void OnInput ( Verb verb , Item item );
	public static OnInput onInput;

	public void OnEndEdit () {


		string str = inputField.text;

		if (str.Length == 0)
			return;

		string[] parts = str.Split (new char[0]);

		if (parts.Length == 0) {
			Debug.Log ("input vide");
			return;
		}

		Verb verb = Verb.Find (parts [0]);
		Item item = null;

		foreach (var part in parts) {

			item = Item.FindInTile (part);

			if (item == null) {
//				Debug.Log ("trouvé dans input (depuis Tile : " + item.name);
				item = Item.FindUsableAnytime (part);

			}

		}


		if (onInput != null)
			onInput (verb, item);

		if (verb != null && item != null) {
			Debug.Log (item.word.name);
			string targetCellContent = verb.cellContents [item.row];

			ActionManager.CheckAction (targetCellContent);

		}

		Clear ();

	}

	public void OnValueChanged () {
		Sound.Instance.PlayRandomTypeSound ();
	}

	void Clear ()
	{
		inputField.text = "";
		Focus ();
	}
	void Focus () {
		inputField.Select ();
		inputField.ActivateInputField ();
	}
}
