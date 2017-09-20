using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLevel : MonoBehaviour {

	Image fillImage;
	Text text;

	// Use this for initialization
	void Awake () {

		fillImage = GetComponent<Image> ();
		text = GetComponentInChildren<Text> ();

		LootManager.Instance.onWrongLevelEvent += HandleOnWrongLevelEvent;

		CrewInventory.Instance.openInventory += HandleOnCardUpdate;
	}

	#region level icons
	void HandleOnCardUpdate (CrewMember crewMember)
	{
		// INFO
		text.text = crewMember.Level.ToString ();

		if ( crewMember.StatPoints > 0 ) {
			text.text += "<b>("+crewMember.StatPoints+")</b>";
		}

		fillImage.fillAmount = ((float)crewMember.CurrentXp / (float)crewMember.xpToLevelUp);
	}
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
