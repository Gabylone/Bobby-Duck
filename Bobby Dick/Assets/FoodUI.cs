using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FoodUI : UiIcon {

	[SerializeField]
	private Image image;

	[SerializeField]
	private Text uiText;

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
	}

	public override void UpdateUI ()
	{
		base.UpdateUI ();

		int allFood = LootManager.Instance.getLoot (Crews.Side.Player).getCategory(ItemCategory.Provisions).Length;

		string s = "" + allFood;

		uiText.text = s;
	}
}
