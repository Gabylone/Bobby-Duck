using UnityEngine;
using System.Collections;

public class Potion : Throwable {
	
	public override void Start ()
	{
		base.Start ();
	}

	public override void Collide (GameObject obj)
	{
		base.Collide (obj);

		Touchable touchable = obj.GetComponent<Touchable> ();

		if ( touchable == null ) {
			return;
		}

		touchable.Touch ();

	}

}
