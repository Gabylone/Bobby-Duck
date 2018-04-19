using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier_IA : Soldier {

	Color initColor;

	float minShootRate = 3f;
	float maxShootRate = 5f;

	float shootRate_Timer = 0f;

	public override void Start ()
	{
		base.Start ();

		initColor = renderer.material.color;

		if ( Cover.covers.Count == 0 ) {
			return;
		}

		Cover targetCover = Cover.getRandomCover;
		Cover.covers.Remove (targetCover);

		Move (targetCover.GetClosestPosition(transform));

		shootRate_Timer = Random.Range (minShootRate,maxShootRate);
	}

	public override void Update() {
		base.Update ();
	}

	public override void Idle_Update ()
	{
		base.Idle_Update ();

		UpdateAim ();
	}
	public override void InCover_Update ()
	{
		base.InCover_Update ();

		UpdateAim ();
	}

	void UpdateAim() {
		if ( shootRate_Timer <= 0 ) {
			AimAtTarget (Soldier_Player.getRandomSoldier.headAnchor);
			shootRate_Timer = Random.Range (minShootRate,maxShootRate);
		}

		shootRate_Timer -= Time.deltaTime;
	}

	#region selected
	public override void Selected ()
	{
		base.Selected ();

		Tween.Bounce (transform);

		foreach ( Soldier soldier in Soldier_Player.selectedSoldiers ) {
			soldier.AimAtTarget (headAnchor);
		}
	}
	#endregion
}
