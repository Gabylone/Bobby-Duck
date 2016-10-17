using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButton : MonoBehaviour {

	[Header("UI elements")]
	[SerializeField] private Button button;
	[SerializeField] private Text nameText;
	[SerializeField] private Text descriptionText;
	[SerializeField] private Text paramText;
	[SerializeField] private Text priceText;

	[SerializeField] private GameObject paramObj;


	private string name;
	private string description;
	private int param;
	private int price;

	public string Name {
		get {
			return name;
		}
		set {
			name = value;
			nameText.text = name;
		}
	}

	public string Description {
		get {
			return description;
		}
		set {
			description = value;
			descriptionText.text = description;
		}
	}

	public int Param {
		get {
			return param;
		}
		set {
			param = value;
			paramText.text = param.ToString ();
		}
	}

	public int Price {
		get {
			return price;
		}
		set {
			price = value;
			priceText.text = price.ToString ();
		}
	}

	public Button Button {
		get {
			return button;
		}
	}

	public GameObject ParamObj {
		get {
			return paramObj;
		}
		set {
			paramObj = value;
		}
	}
}
