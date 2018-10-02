using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawner : MonoBehaviour {

	public float range = 1f;

	public int minAmount;

	public int maxAmount;

	public GameObject applePrefab;

	// Use this for initialization
	void Start () {
		SpawnApples ();
	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	void SpawnApples ()
	{
		int amount = Random.Range (minAmount , maxAmount);

		for (int i = 0; i < amount; i++) {

			GameObject apple = Instantiate (applePrefab,transform) as GameObject;

			Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * range;

			apple.transform.position = pos;
		}
	}
}
