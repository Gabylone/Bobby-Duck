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

		None
	}

	// Use this for initialization
	void Start () {
		
	}

	public void UpdateButtons (ButtonType buttonType1 , ButtonType buttonType2 = ButtonType.None) {

		foreach ( GameObject button in buttonObjects ) {
			button.SetActive (false);
		}

		buttonObjects [(int)buttonType1].SetActive (true);
		if ( buttonType2 != ButtonType.None )
			buttonObjects [(int)buttonType2].SetActive (true);

	}
}
