using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTrigger : MonoBehaviour {

	Zombie linkedZombie;

	// Use this for initialization
	void Start () {
		linkedZombie = GetComponentInParent<Zombie> ();
	}
//	
//	void OnTriggerEnter ( Collider col ) {
//
//		if ( col.GetComponent<Soldier>() != null ) {
//			linkedZombie.GoToPosition (col.transform.position);
//		}
//
//	}
}
