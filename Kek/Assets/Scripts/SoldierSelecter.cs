using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSelecter : MonoBehaviour {

	Soldier linkedSoldier;

	void Start () {
		linkedSoldier = GetComponentInParent<Soldier> ();
	}

	void OnMouseDown()
	{
		print ("oui ?");
//		linkedSoldier.SwitchSelected ();
//		Application.LoadLevel("SomeLevel");
	}
}
