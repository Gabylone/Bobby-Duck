using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("delay",0.001f);
	}

    void delay()
    {
        transform.position = new Vector3(0f, Tile.maxY + 1);
    }
	
}
