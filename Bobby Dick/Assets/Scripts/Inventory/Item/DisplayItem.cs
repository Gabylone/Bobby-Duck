using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class DisplayItem : MonoBehaviour {

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

	private string name;
	private string description;
	private int param = 0;
	private int price = 0;
	private int weight = 0;
	private int level = 0;

	bool enabled = false;

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
			if (nameText == null)
				return;

			name = value;
			nameText.text = name;

		}
	}

	public int Value {
		get {
			return param;
		}
		set {

			if (paramObj == null)
				return;

			param = value;

			Tween.Bounce (paramObj.transform);

			paramText.text = param.ToString ();
			paramObj.SetActive ( value > 0 );
		}
	}

	public int Price {
		get {
			return price;
		}
		set {

			if (priceObj == null)
				return;

			price = value;

			Tween.Bounce (priceObj.transform);

			priceText.text = price.ToString ();
			priceObj.SetActive (value > 0);
		}
	}

	public int Weight {
		get {
			return weight;
		}
		set {

			if (weightObj == null)
				return;

			weight = value;

			Tween.Bounce (weightObj.transform);

			weightText.text = weight.ToString ();
			weightObj.SetActive (value > 0);
		}
	}

	public int Level {
		get {
			return level;
		}
		set {

			if (lvlObj == null)
				return;

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

	public virtual Item HandledItem {
		get {
			return handledItem;
		}
		set {

			Tween.ClearFade (transform);

			if (handledItem == null) {
				Tween.Bounce (transform, 0.2f, 1.05f);
			}

			handledItem = value;

			if (value == null) {
				Name = "";
				Value = 0;
				Price = 0;
				Weight = 0;
				Level = 0;
				return;
			}

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
	public void Clear () {

		handledItem = null;

		Name = "";
		Value = 0;
		Price = 0;
		Weight = 0;
		Level = 0;

		Tween.Fade ( transform , 0.3f  );
	}
	#endregion
}
