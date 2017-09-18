using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayName : MonoBehaviour {

	[SerializeField]
	private Text textUI_Name;

	// Use this for initialization
	void Start () {
		PlayerLoot.Instance.openInventory += HandleOpenInventory;
	}

	void HandleOpenInventory (CrewMember member)
	{
		textUI_Name.text = member.MemberName;
	}
}
