using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour {

	public static SoldierManager Instance;

	public int soldierAmount = 0;

	public Transform soldierParent;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
}
