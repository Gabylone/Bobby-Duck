using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayName : MonoBehaviour {

	[SerializeField]
	private Text textUI_Name;

	// Use this for initialization
	void Start () {
		CrewInventory.Instance.openInventory += HandleOpenInventory;

		HandleOpenInventory (CrewMember.selectedMember);
	}

	void HandleOpenInventory (CrewMember member)
	{
		if (CrewMember.selectedMember == null)
			return;

		textUI_Name.text = member.MemberName;
	}
}
