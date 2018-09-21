using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

        int i = 0;
        foreach (var item in GetComponentsInChildren<LevelButton>())
        {
            item.id = i;

            ++i;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
