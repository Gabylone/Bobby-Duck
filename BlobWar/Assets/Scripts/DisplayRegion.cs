using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRegion : DisplayGroup {

	public static DisplayRegion Instance;

	public Transform regionButtonParent;

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

	public override void Close ()
	{
		base.Close ();

		RegionButton.GetSelected.Deselect ();

	}

}
