﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class worldtouchtest : MonoBehaviour {

    public Image image;

	// Update is called once per frame
	void Update () {
        image.color = WorldTouch.Instance.isEnabled ? Color.green : Color.red;
	}
}
