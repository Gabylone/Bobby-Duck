using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class worldtouchtest : MonoBehaviour {

    public Image image;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        image.color = WorldTouch.Instance.isEnabled ? Color.green : Color.red;
	}
}
