using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class DisplayItem : MonoBehaviour {

	private Item handledItem;

	[Header("UI elements")]
	public Button button;
	public Image image;

	[SerializeField] private Text nameText;

	[SerializeField] private Text paramText;
	[SerializeField] private Text priceText;
	[SerializeField] private Text weightText;
	[SerializeField] private Text lvlText;

	[SerializeField] private GameObject paramObj;
	[SerializeField] private GameObject priceObj;
	[SerializeField] private GameObject weightObj;
	public GameObject lvlObj;

	private string name;
	private string description;
	private int param = 0;
	private int price = 0;
	private int weight = 0;
	private int level = 0;

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

			paramObj.SetActive (value > 0);

			Tween.Bounce (paramObj.transform);

			paramText.text = param.ToString ();

			switch (handledItem.category) {
			case ItemCategory.Weapon:
			case ItemCategory.Clothes:
				CrewMember.EquipmentPart part = handledItem.category == ItemCategory.Weapon ? CrewMember.EquipmentPart.Weapon : CrewMember.EquipmentPart.Clothes;
				if (CrewMember.GetSelectedMember.GetEquipment (part) == null || param > CrewMember.GetSelectedMember.GetEquipment (part).value) {
					paramText.color = Color.green;
				} else if (param < CrewMember.GetSelectedMember.GetEquipment (part).value) {
					paramText.color = Color.red;
				} else {
					paramText.color = Color.white;
				}
				break;
			case ItemCategory.Misc:
			case ItemCategory.Provisions:
				paramText.color = Color.white;
				break;
			default:
				break;
			}


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

			if (LootUI.Instance.currentSide == Crews.Side.Enemy) {
				
				if (price > GoldManager.Instance.goldAmount && OtherInventory.Instance.type == OtherInventory.Type.Trade) {
					priceText.color = Color.red;
				} else {
					priceText.color = Color.white;
				}

			} else {
				priceText.color = Color.white;
			}

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

			if (LootUI.Instance.currentSide == Crews.Side.Enemy) {

				if (weight + WeightManager.Instance.CurrentWeight> WeightManager.Instance.CurrentCapacity) {
					weightText.color = Color.red;
				} else {
					weightText.color = Color.white;
				}

			} else {
				weightText.color = Color.white;
			}

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

			Image image = lvlObj.GetComponent<Image> ();

			if (level > CrewMember.GetSelectedMember.Level) {
				image.color = Color.red;
			} else if ( level < CrewMember.GetSelectedMember.Level ) {
				image.color = Color.green;
			} else {
				image.color = Color.white;
			}

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
				return;
			}

			Name 		= handledItem.name;

			Value 		= handledItem.value;

//			if (LootUI.
			if ( LootUI.Instance.categoryContentType == CategoryContentType.OtherTrade ) {
				Price = HandledItem.price;
			} else {
				Price = (int)(HandledItem.price / 2f);
			}
			
			Weight 		= HandledItem.weight;

			Level 		= HandledItem.level;

		}
	}
	public virtual void Clear () {

		handledItem = null;

		Name = "";
		if( paramObj != null )
		paramObj.SetActive (false);
		if( priceObj != null )
		priceObj.SetActive (false);
		if( weightObj != null )
		weightObj.SetActive (false);
		if( lvlObj != null )
		lvlObj.SetActive (false);

		Tween.Fade ( transform , 0.3f  );
	}
	#endregion
}
