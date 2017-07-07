using UnityEngine;
using System.Collections;

public class Tile : Touchable {

	public override void Touch ()
	{
		if (!base.AlreadyTouched ())
			return;

		base.Touch ();

		gameObject.AddComponent<FloatComponent_Tile> ();
	}
}
