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
	[SerializeField] private GameObject priceObj;

	private int index = 0;

	private string name;
	private string description;
	private int param;
	private int price;

	bool enabled = false;

	public void Select () {
		GetComponentInParent<LootUI> ().UpdateActionButton (index);
	}

	public bool Enabled {
		get {
			return enabled;
		}
		set {
			enabled = value;
			GetComponent<Button> ().interactable = value;
		}
	}

	#region params
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
			paramObj.SetActive ( value > 0 );
		}
	}

	public int Price {
		get {
			return price;
		}
		set {
			price = value;
			priceText.text = price.ToString ();
			priceObj.SetActive (value > 0);
		}
	}

	public Button Button {
		get {
			return button;
		}
	}

	public int Index {
		get {
			return index;
		}
		set {
			index = value;
		}
	}
	#endregion
}
