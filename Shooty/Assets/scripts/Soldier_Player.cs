using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier_Player : Soldier {

	public delegate void OnMove (Vector3 p);
	public static OnMove onMove;

	Color initColor;

	public override void Start ()
	{
		base.Start ();

		playerSoldiers.Add (this);

		initColor = renderer.material.color;

		WorldTouch.onTouchWorld += HandleOnTouchWorld;
	}

	public override void Update() {
		base.Update ();
	}

	#region move
	public override void Moving_Start ()
	{
		base.Moving_Start ();
	}
	public override void Move (Vector3 v)
	{
		base.Move (v);

		if (onMove != null)
			onMove (v);

		Deselect ();
	}
	#endregion

	#region shoot
	public override void Shoot_Start ()
	{
		base.Shoot_Start ();

		Deselect ();
	}
	#endregion

	#region selected
	void HandleOnTouchWorld (Vector3 p, WorldTouch.ClickType clickType )
	{
		switch (clickType) {
		case WorldTouch.ClickType.Left:
//			Deselect ();


			if (selected) {
				Move (p);
			}
			break;
		case WorldTouch.ClickType.Right:
			if (selected) {
				Move (p);
			}
			break;
		default:
			break;
		}


	}

	public override void Select ()
	{
		if (selected)
			return;

		base.Select ();

		renderer.material.color = Color.yellow;

		selectedSoldiers.Add (this);

		Tween.Bounce (transform);
	}

	public override void Deselect ()
	{
		if (!selected)
			return;

		base.Deselect ();

		selectedSoldiers.Remove (this);

		renderer.material.color = initColor;

		Tween.Bounce (transform);
	}
	#endregion

	#region to
	public static List<Soldier> selectedSoldiers = new List<Soldier>();
	public static List<Soldier> playerSoldiers = new List<Soldier>();
	public static Soldier getRandomSoldier { 
		get { 
			return playerSoldiers [Random.Range (0, playerSoldiers.Count)];
		}
	}
	#endregion



}
