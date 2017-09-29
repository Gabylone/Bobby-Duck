using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour {

		// components
	private Transform _transform;

	[Header("UI Elements")]
	[SerializeField]
	private GameObject cardObject;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Image heartImage;

	[SerializeField]
	private GameObject heartGroup;

	[SerializeField]
	private Text defenceText;

	[SerializeField]
	private Text attackText;

	[SerializeField]
	private Text levelText;

	public virtual void Init () {
		HideCard ();

		LootUI.useInventory+= HandleUseInventory;
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		UpdateMember (CrewMember.selectedMember);
	}

	public virtual void UpdateMember ( CrewMember member ) {

		Tween.Bounce (heartGroup.transform);

		nameText.text = member.MemberName;

		levelText.text = member.Level.ToString ();

		heartImage.fillAmount = (float)member.Health / (float)member.MemberID.maxHealth;

		attackText.text = member.Attack.ToString ();

		defenceText.text = member.Defense.ToString ();
	}

	public void ShowCard () {
		//
		cardObject.SetActive (true);

	}
	public void HideCard () {
		cardObject.SetActive (false);
	}


}
