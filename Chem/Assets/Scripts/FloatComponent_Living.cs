using UnityEngine;
using System.Collections;

public class FloatComponent_Living : FloatComponent {

	public override void Start ()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void Float_Exit ()
	{
		base.Float_Exit ();

		Kill ();
	}
}
