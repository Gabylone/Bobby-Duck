using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButton : MonoBehaviour {

	[SerializeField]
	private LootUI lootUI;

	private Item handledItem;

	[Header("UI elements")]
	[SerializeField] private Button button;

	[SerializeField] private Text nameText;

	[SerializeField] private Text paramText;
	[SerializeField] private Text priceText;
	[SerializeField] private Text weightText;
	[SerializeField] private Text lvlText;

	[SerializeField] private GameObject paramObj;
	[SerializeField] private GameObject priceObj;
	[SerializeField] private GameObject weightObj;
	[SerializeField] private GameObject lvlObj;

	private int index = 0;

	private string name;
	private string description;
	private int param = 0;
	private int price = 0;
	private int weight = 0;
	private int level = 0;

	bool enabled = false;

	public void Select () {
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);
		lootUI.UpdateActionButton (index);
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

	public int Value {
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

	public int Weight {
		get {
			return weight;
		}
		set {
			weight = value;
			if (weightObj != null) {
				weightText.text = weight.ToString ();
				weightObj.SetActive (value > 0);
			}
		}
	}

	public int Level {
		get {
			return level;
		}
		set {
			level = value;
			lvlText.text = level.ToString ();
			lvlObj.SetActive (value > 0);
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

	public Item HandledItem {
		get {
			return handledItem;
		}
		set {
			handledItem = value;

			Name 		= handledItem.name;

			if (HandledItem.category == ItemCategory.Misc)
				Value = 0;
			else
				Value = handledItem.value;

			Price 		= HandledItem.price;

			Weight 		= HandledItem.weight;

			Level 		= HandledItem.level;
		}
	}
	#endregion
}
