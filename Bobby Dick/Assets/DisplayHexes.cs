using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHexes : MonoBehaviour {

	public GameObject hexPrefab;

	int scale = 10;

	float hexHeight;

	float hexDecal = 148.7f;

	// Use this for initialization
	void Start () {

		hexHeight = hexPrefab.GetComponent<Image> ().rectTransform.rect.height;

		bool down = false;

		for (int x = 0; x < scale; x++) {
			for (int y = 0; y < scale; y++) {

				// INST
				GameObject hexObj = Instantiate (hexPrefab, transform);

				// SCALE
				hexObj.transform.localScale = Vector3.one;

				// POS
				Vector3 pos;

				float yPos = down ? (hexHeight/2f) : 0f;

				pos = new Vector3 ( x * hexDecal , yPos * y, 0f);

				hexObj.transform.position = pos;

			}	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
