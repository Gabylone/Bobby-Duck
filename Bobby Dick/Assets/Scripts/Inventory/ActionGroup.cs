using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionGroup : MonoBehaviour {

	[SerializeField]
	private GameObject[] buttonObjects;

	public enum ButtonType {
		Eat,
		Equip,
		Throw,
		Purchase,
		Sell,
		PickUp,
		PurchaseAndEquip,

		None
	}

	bool visible = false;

	// Use this for initialization
	void Start () {
		
	}

	public void UpdateButtons (ButtonType[] buttonTypes) {

		foreach ( GameObject button in buttonObjects ) {
			button.SetActive (false);
		}

		buttonObjects [(int)buttonTypes[0]].SetActive (true);
		Tween.Bounce (buttonObjects [(int)buttonTypes [0]].transform);

		if (buttonTypes.Length > 1) {
			buttonObjects [(int)buttonTypes [1]].SetActive (true);
			Tween.Bounce (buttonObjects [(int)buttonTypes [1]].transform);
		}

	}

	public bool Visible {
		get {
			return visible;
		}
		set {
			visible = value;

			gameObject.SetActive (value);
		}
	}

	public GameObject[] ButtonObjects {
		get {
			return buttonObjects;
		}
	}
}
