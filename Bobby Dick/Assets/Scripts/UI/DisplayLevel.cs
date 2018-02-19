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

		CrewMember.onWrongLevel += HandleOnWrongLevelEvent;

		CrewInventory.Instance.openInventory += HandleOpenInventory;

		StatButton.onClickStatButton += UpdateUI;

		HandleOpenInventory (CrewMember.GetSelectedMember);

	}

	void HandleOpenInventory (CrewMember member)
	{
		UpdateUI ();
	}

	void UpdateUI (){

		if (CrewMember.GetSelectedMember == null)
			return;

		CrewMember crewMember = CrewMember.GetSelectedMember;

		// INFO
		text.text = crewMember.Level.ToString ();

		fillImage.fillAmount = ((float)crewMember.CurrentXp / (float)crewMember.xpToLevelUp);

		Tween.Bounce (transform);
//
	}

	#region level icons
	void HandleOnWrongLevelEvent ()
	{
		Tween.Bounce (transform);
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
