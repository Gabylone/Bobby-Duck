﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		DrawDefaultInspector ();

		Inventory inv = (Inventory)target;
		if( GUILayout.Button("Reset Save") ) {
			PlayerPrefs.DeleteAll ();
		}
	}
}
