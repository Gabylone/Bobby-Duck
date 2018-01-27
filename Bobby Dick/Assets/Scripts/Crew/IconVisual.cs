using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IconVisual : MonoBehaviour {

	void Start () {

		LootUI.useInventory += HandleUseInventory;
		DisplayItem_Crew.onRemoveItemFromMember += HandleOnRemoveItemFromMember;
	}

	void OnDestroy () {

		LootUI.useInventory -= HandleUseInventory;
		DisplayItem_Crew.onRemoveItemFromMember -= HandleOnRemoveItemFromMember;
	}

	public void InitVisual (Member memberID) {

		FaceImage.color = CrewCreator.Instance.Beige;

		// hair
		if (memberID.hairSpriteID > -1) {
			HairImage.sprite = memberID.Male ? CrewCreator.Instance.HairSprites_Male [memberID.hairSpriteID] : CrewCreator.Instance.HairSprites_Female [memberID.hairSpriteID];
			HairImage.enabled = true;
		} else {
			HairImage.enabled = false;
		}

		HairImage.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		if (memberID.beardSpriteID > -1) {
			BeardImage.enabled = true;
			BeardImage.sprite = CrewCreator.Instance.BeardSprites [memberID.beardSpriteID];
		} else {
			BeardImage.enabled = false;

		}

		BeardImage.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		// eyes
		EyesImage.sprite = CrewCreator.Instance.EyesSprites [memberID.eyeSpriteID];
		EyebrowsImage.sprite = CrewCreator.Instance.EyebrowsSprites [memberID.eyebrowsSpriteID];
		EyebrowsImage.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		// nose
		NoseImage.sprite = CrewCreator.Instance.NoseSprites[memberID.noseSpriteID];

		// mouth
		MouthImage.sprite = CrewCreator.Instance.MouthSprites [memberID.mouthSpriteID];
		MouthImage.color = CrewCreator.Instance.Beige;

		// body
		BodyImage.sprite = CrewCreator.Instance.BodySprites[memberID.Male ? 0:1];

//		Debug.Log (memberID.equipedWeapon.name);
		UpdateWeaponSprite (memberID.equipedWeapon.spriteID);

	}

	void HandleOnRemoveItemFromMember (Item item)
	{
		if (item.category == ItemCategory.Clothes) {
			//
		} else {
			weaponImage.sprite = CrewCreator.Instance.handSprite;
		}
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		switch (actionType) {
		case InventoryActionType.Equip:
		case InventoryActionType.PurchaseAndEquip:
			if (CrewMember.GetSelectedMember.GetEquipment (CrewMember.EquipmentPart.Weapon) != null && LootUI.Instance.SelectedItem.category == ItemCategory.Weapon) {
				weaponImage.enabled = true;
				UpdateWeaponSprite (CrewMember.GetSelectedMember.GetEquipment (CrewMember.EquipmentPart.Weapon).spriteID);
			}
			break;
		default:
			break;
		}


	}

	public void UpdateWeaponSprite (int spriteID) {
//		if (spriteID >= CrewCreator.Instance.weaponSprites.Length)
//			Debug.Log ("sprite id : " + spriteID + " .... " +  CrewCreator.Instance.weaponSprites.Length);
		weaponImage.sprite = CrewCreator.Instance.weaponSprites[spriteID];
	}

	[Header("BobyParts")]
	[SerializeField]
	private Image faceImage;
	[SerializeField]
	private Image beardImage;
	[SerializeField]
	private Image bodyImage;
	[SerializeField]
	private Image hairImage;
	[SerializeField]
	private Image eyesImage;
	[SerializeField]
	private Image eyebrowsImage;
	[SerializeField]
	private Image mouthImage;
	[SerializeField]
	private Image noseImage;
	[SerializeField]
	private Image weaponImage;

	public Image FaceImage {
		get {
			return faceImage;
		}
	}

	public Image WeaponImage {
		get {
			return weaponImage;
		}
	}

	public Image BeardImage {
		get {
			return beardImage;
		}
	}

	public Image HairImage {
		get {
			return hairImage;
		}
	}

	public Image EyesImage {
		get {
			return eyesImage;
		}
	}

	public Image EyebrowsImage {
		get {
			return eyebrowsImage;
		}
	}

	public Image MouthImage {
		get {
			return mouthImage;
		}
	}

	public Image NoseImage {
		get {
			return noseImage;
		}
	}

	public Image BodyImage {
		get {
			return bodyImage;
		}
	}
}
