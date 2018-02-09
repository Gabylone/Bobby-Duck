using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFormulas : MonoBehaviour {

	public Text text;

	void Start () {
		
	}

	public void ShowFormulas () {

		bool foundOne = false;

		string str = "";

		foreach (var form in FormulaManager.Instance.formulas) {

			if ( form.found == true ) {
				str += " " + form.name;
				foundOne = true;
			}

		}

		if ( foundOne ) {

			text.text = "Formules :" + str;

		} else {

			text.text = "";

		}

	}
}
