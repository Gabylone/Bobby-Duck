using UnityEngine;
using System.Collections;

public class Apple : Ingredient {

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();

		DisablePhysics ();
	}

	public override void Interact ()
	{
		if (!harvested) {

			Push ();
			harvested = true;

			EnablePhysics ();

		} else {

			base.Interact ();
		}
	}

}
