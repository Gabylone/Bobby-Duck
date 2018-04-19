using UnityEngine;
using System.Collections;

public class IngredientsSpiral : SpiralInventory {

	public static IngredientsSpiral Instance;

	void Awake () {
		Instance = this;
	}

	public override void Start ()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void Select (int i)
	{

		Ingredient ingr = ElementTransforms [i].GetComponent<Ingredient> ();

		ingr.ExitInventory ();

		base.Select (i);

	}
}
