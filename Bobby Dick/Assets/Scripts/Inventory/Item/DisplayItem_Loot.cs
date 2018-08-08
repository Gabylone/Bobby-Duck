using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem_Loot : DisplayItem {

	public GameObject group;

	public static DisplayItem_Loot selectedDisplayItem = null;

	public Image itemImage;

	public int index = 0;


	public bool selected = false;

    private void Awake()
    {
        selectedDisplayItem = null;
    }

    void Start () {

		itemImage.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, Random.Range (-30, 30)));

		CrewMember.onWrongLevel += HandleOnWrongLevelEvent;

	}

	void HandleOnWrongLevelEvent ()
	{
		Tween.Bounce (transform);
	}

	public void Select () {

		if ( selected ) {
			Deselect ();
			return;
		}

        // select
		if (selectedDisplayItem != null) {
			selectedDisplayItem.Deselect ();
		}
        selectedDisplayItem = this;
        selected = true;

		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);

		LootUI.Instance.SelectedItem = HandledItem;

		Tween.Bounce (transform);

		button.image.color = Color.blue;

	}

	public void Deselect () {

		selectedDisplayItem = null;

		selected = false;

		LootUI.Instance.SelectedItem = null;

		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);

		UpdateColor ();

	}

	void UpdateColor ()
	{
		if (HandledItem == null ) {
			return;
		}

		float a = 0.7f;

        if (CrewMember.GetSelectedMember.GetEquipment(HandledItem.EquipmentPart) == HandledItem && index == 0 )
        {
            if (HandledItem.category == ItemCategory.Clothes || HandledItem.category == ItemCategory.Weapon)
            {
                image.color = LootManager.Instance.item_EquipedColor;
                return;
            }
            
        }

		if ( HandledItem.level > CrewMember.GetSelectedMember.Level ) {
                image.color = LootManager.Instance.item_SuperiorColor;
			image.color = new Color(1f, a , a);
        } else if ( HandledItem.level < CrewMember.GetSelectedMember.Level && HandledItem.level > 0 ) {
                image.color = LootManager.Instance.item_InferiorColor;
			image.color = new Color(a, 1f, a);
        } else {
                image.color = LootManager.Instance.item_DefaultColor;
        }
	}

	public override Item HandledItem {
		get {
			return base.HandledItem;
		}
		set {
			
			base.HandledItem = value;

			if (value == null) {
//				itemImage.enabled = false;
				group.SetActive (false);
				return;
			}

			group.SetActive (true);

			UpdateColor ();

			if (value.spriteID < 0) {
				itemImage.enabled = false;
			} else {
				itemImage.enabled = true;
				itemImage.sprite = LootManager.Instance.getItemSprite (value.category, value.spriteID);
			}

		}
	}
}
