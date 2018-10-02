using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierTrigger : MonoBehaviour {

	Soldier linkedSoldier;

	void Start () {
		linkedSoldier = GetComponentInParent<Soldier> ();
	}

//	void OnTriggerEnter ( Collider col ) {
//
//		Zombie zombie = col.GetComponent<Zombie> ();
//
//		if ( zombie != null ) {
//			linkedSoldier.AddZombieTarget (zombie);
//		}
//
//	}
//
}
