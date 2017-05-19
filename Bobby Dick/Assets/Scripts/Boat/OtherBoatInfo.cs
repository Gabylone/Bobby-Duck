using UnityEngine;

public class OtherBoatInfo : BoatInfo {

	private float changeOfChangeDirection = 10f;

	public override void Init ()
	{
		base.Init ();

		currentDirection = (Directions)Random.Range (0,8);
	}

	public override void UpdatePosition ()
	{
		base.UpdatePosition ();

		PosX += (int)NavigationManager.Instance.getDir (currentDirection).x;
		PosY += (int)NavigationManager.Instance.getDir (currentDirection).y;

		if ( Random.value < changeOfChangeDirection ) {
			currentDirection = (Directions)Random.Range (0,8);
		}
	}
}
