using UnityEngine;
using System.Collections;



public class Touchable : MonoBehaviour {

	public virtual void Touch () {
		//
	}

	public bool AlreadyTouched ()
	{
		if ( GetComponent<FloatComponent>() != null ) {
			return false;
		}

		return true;
	}
}
