using UnityEngine;
using System.Collections;

public class Apple : Ingredient {

	private bool onTree = false;

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();

		OnTree = true;
	}
	
	// Update is called once per frame
	public override void Update () {

		if (!OnTree) {
			base.Update ();
		}
	}

	public override void Interact ()
	{
		if (OnTree) {

			OnTree = false;
			Push ();

		} else {

			base.Interact ();
		}
	}

	public bool OnTree {
		get {
			return onTree;
		}
		set {
			onTree = value;

			Rigidody.isKinematic = onTree;
			Collider.enabled = !onTree;
		}
	}
}
