using UnityEngine;

public class OtherBoatInfo : BoatInfo {

	private float changeOfChangeDirection = 10f;

	private Story story;

	public override void Init ()
	{
		base.Init ();

		currentDirection = (Directions)Random.Range (0,8);

		story = StoryLoader.Instance.BoatStories [Random.Range (0,StoryLoader.Instance.BoatStories.Count)];
	}

	public override void UpdatePosition ()
	{
		base.UpdatePosition ();

		PosX += (int)NavigationManager.Instance.getDir (currentDirection).x;
		PosY += (int)NavigationManager.Instance.getDir (currentDirection).y;

		if ( Random.value < changeOfChangeDirection ) {
			currentDirection = (Directions)Random.Range (0,8);
		}

		Debug.Log ("boats are moving...");
	}
}
