using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {

//	void OnMouseDown () {
//		Selected ();
//	}

	public virtual void Selected ()
	{
		WorldTouch.somethingWasSelected = true;
	}
}
