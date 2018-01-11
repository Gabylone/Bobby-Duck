using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLevel : MonoBehaviour {

	public Image fillImage;
	Text text;

	// Use this for initialization
	void Start () {

		text = GetComponentInChildren<Text> ();

		LootManager.Instance.onWrongLevelEvent += HandleOnWrongLevelEvent;

		CrewInventory.Instance.openInventory += HandleOpenInventory;

		StatButton.onClickStatButton += UpdateUI;

		HandleOpenInventory (CrewMember.selectedMember);

	}

	void HandleOpenInventory (CrewMember member)
	{
		UpdateUI ();
	}

	void UpdateUI (){

		if (CrewMember.selectedMember == null)
			return;

		CrewMember crewMember = CrewMember.selectedMember;

		// INFO
		text.text = crewMember.Level.ToString ();

		fillImage.fillAmount = ((float)crewMember.CurrentXp / (float)crewMember.xpToLevelUp);

		BounceLevel ();

//		if (crewMember.Level == crewMember.maxLevel) {
//			text.text = "MAX";
//			return;
//		}
//
	}

	#region level icons
	void BounceLevel () {
		Tween.Bounce (fillImage.transform);
	}

	void HandleOnWrongLevelEvent ()
	{
		BounceLevel ();
		TaintLevelImage ();
	}
	void TaintLevelImage() {
		
		text.color = Color.red;
		Invoke ("UntaintLevelImage",1f);
	}
	void UntaintLevelImage () {
		text.color = Color.white;
	}
	#endregion

}
