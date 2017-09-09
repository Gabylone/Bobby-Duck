using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCrew : MonoBehaviour {

	public GameObject targetGameObject;

	// Use this for initialization
	void Start () {
		PlayerLoot.Instance.openInventory += HandleOpenInventory;
		PlayerLoot.Instance.closeInventory += HandleCloseInventory;

		HandleCloseInventory ();
	}

	void HandleCloseInventory ()
	{
		targetGameObject.SetActive (false);
	}

	void HandleOpenInventory (CrewMember member)
	{
		targetGameObject.SetActive (true);

		Tween.Bounce (transform,0.05f,1.03f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
