using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Holoville.HOTween;

public class SoldierAI : Soldier {

	public static bool soldierTouched = false;

	public override void PlaceStart ()
	{
		base.PlaceStart ();
	}

	public override void PlaceExit ()
	{
		base.PlaceExit ();

		Move (Swipe.Direction.None);
	}

	public override void Start ()
	{
		base.Start ();

		if ( !placing )
			Move (Swipe.Direction.None);
	}

	public override void Update ()
	{
		base.Update ();

		if ( _shootTimer <= 0f && !placing ) {

			Shoot ();

			_shootTimer = shootRate;

		}

		_shootTimer -= Time.deltaTime;
	}

	public override void Shoot ()
	{
		base.Shoot ();

		if (CurrentLign.enemies.Count > 0) {
			ShootAnim ();
		}
	}

	public override void Move(Swipe.Direction direction)
	{
		CurrentLign.RemoveSoldier (this);
		CurrentLign.UpdateSoldierAnchors ();

		if ( currentLignIndex == Player.Instance.currentLignIndex ) {
			Player.Instance.Move (Swipe.Direction.None);
		}

		base.Move(direction);

		CurrentLign.AddSoldier (this);
		CurrentLign.UpdateSoldierAnchors ();

		if ( currentLignIndex == Player.Instance.currentLignIndex ) {
			Player.Instance.Move (Swipe.Direction.None);
		}

		Tween.Bounce (transform);

	}

	void OnMouseDown () {

		soldierTouched = true;

		Tween.Scale (transform, 0.2f, 1.15f);

		Swipe.onSwipe += HandleOnSwipe;

	}

	void OnMouseUp () {
		Tween.Bounce (transform);
	}

	void HandleOnSwipe (Swipe.Direction direction)
	{
		Swipe.onSwipe -= HandleOnSwipe;

		Move (direction);
	}
}
