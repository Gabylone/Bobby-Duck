using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDisplay : MonoBehaviour {

	public GameObject linePrefab;

	public Material whiteMat;
	public Material clearMat;
	public GameObject cubePartPrefab;

	void Start () {

//		CreateCubeParts ();

		CreateLines ();

	}

	void CreateCubeParts ()
	{
		for (int h = 0; h < Grid.height+1; h++) {

			for (int x = 0; x < Grid.scaleX+1; x++) {

				for (int y = 0; y < Grid.scaleY+1; y++) {

					GameObject cubePart = Instantiate (cubePartPrefab, transform) as GameObject;

					cubePart.transform.position = new Vector3 (x,h,y);

				}

			}

		}
	}

	// Use this for initialization
	void CreateLines () {

		for (int x = 0; x < Grid.scaleX+1; x++) {

			for (int y = 0; y < Grid.scaleY+1; y++) {

				Vector3 pos1 = new Vector3 (x,0,y);
				Vector3 pos2 = new Vector3 (x,Grid.height,y);

				CreateLine (pos1, pos2);

			}

		}

		for (int h = 0; h < Grid.height+1; h++) {

			for (int x = 0; x < Grid.scaleX+1; x++) {

				Vector3 pos1 = new Vector3 (x,h,0);
				Vector3 pos2 = new Vector3 (x,h,Grid.scaleY);

				CreateLine (pos1, pos2);

			}


			for (int y = 0; y < Grid.scaleY+1; y++) {

				Vector3 pos1 = new Vector3 (0,h,y);
				Vector3 pos2 = new Vector3 (Grid.scaleX,h,y);

				CreateLine (pos1, pos2);

			}



		}


	}

	void CreateLine ( Vector3 pos1 , Vector3 pos2 ) {
		
		GameObject lineObj = Instantiate (linePrefab, transform) as GameObject;

		LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer> ();

		Material mat = clearMat;

		if ( pos1.x <= 0 && pos1.z <= 0 ) {
			mat = whiteMat;
		}

		if ( pos1.x >= Grid.scaleX && pos1.z <= 0 ) {
			mat = whiteMat;
		}

		if ( pos1.x >= Grid.scaleX && pos1.z >= Grid.scaleX ) {
			mat = whiteMat;
		}

		if ( pos1.x <= 0 && pos1.z >= Grid.scaleX ) {
			mat = whiteMat;
		}

//		if ( pos1.x >= Grid.scaleX || pos1.z >= Grid.scaleY  || pos1.y >= Grid.height ) {
//			mat = whiteMat;
//		}

		lineRenderer.material = mat;

		lineRenderer.SetPosition (0,pos1);
		lineRenderer.SetPosition (1,pos2);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static Vector3 RoundToGrid (Vector3 pos) {

		pos -= Vector3.one * 0.5f;

		Vector3 roundedPos = new Vector3 ( Mathf.Round (pos.x)  ,Mathf.Round (pos.y) , Mathf.Round (pos.z) );
//		return roundedPos;
		return roundedPos + Vector3.one * 0.5f;
	}
}
