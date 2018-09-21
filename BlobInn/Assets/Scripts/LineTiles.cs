using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTiles : MonoBehaviour {
    // Use this for initialization
    void Start()
    {
        Invoke("StartDelay", 0000.1f);
    }

    void StartDelay()
    {
        transform.position = new Vector3(0f, Tile.maxY + 1);
    }
}
