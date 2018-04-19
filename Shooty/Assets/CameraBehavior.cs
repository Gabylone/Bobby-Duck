using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

	public float decalX = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tPos = getTargetPos;

		Vector3 camPos = transform.position;
		camPos.x = (tPos.x + decalX);
		transform.position = camPos;
	}

	public Vector3 getTargetPos {
		get {

			Soldier closestSoldier = Soldier.soldiers [0];

			foreach (var soldier in Soldier.soldiers) {
				if ( soldier.transform.position.x < closestSoldier.transform.position.x ) {
					closestSoldier = soldier;
				}
			}

			return closestSoldier.transform.position;

		}
	}
//
//	public Vector3 getTargetPos {
//		get {
//			if ( Soldier_Player.selectedSoldiers.Count == 0 ) {
//				return Soldier_Player.playerSoldiers [0].transform.position;
//			}
//
//			return Soldier_Player.selectedSoldiers [0].transform.position;
//		}
//	}
}
