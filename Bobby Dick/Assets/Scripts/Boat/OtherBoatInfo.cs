using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class OtherBoatInfo : BoatInfo {

	private float changeOfChangeDirection = 0.2f;

	private StoryManager storyManager;

	public bool metPlayer = false;

	public override void Init ()
	{
		base.Init ();

		currentDirection = (Directions)Random.Range (0,8);

			// assign story
		storyManager = new StoryManager ();
		storyManager.InitHandler (StoryType.Boat);

		PosX = Random.Range ( 0 , MapGenerator.Instance.MapScale );
		PosY = Random.Range ( 0 , MapGenerator.Instance.MapScale );

	}

	public override void UpdatePosition ()
	{
		base.UpdatePosition ();

		if ( Random.value < 0.4f ) {
			MoveToOtherChunk ();
		}
	}

	void MoveToOtherChunk ()
	{
		Vector2 p = NavigationManager.Instance.getDir (currentDirection);

		int newX = PosX + (int)p.x;
		int newY = PosY + (int)p.y;


		if (newX >= MapGenerator.Instance.MapScale - 1) {

			newX = PosX;
			SwitchDirection ();

		} else if (newX < 0) {

			newX = PosX;
			SwitchDirection ();
			//
		} else if (newY >= MapGenerator.Instance.MapScale - 1) {

			newY = PosY;
			SwitchDirection ();

		} else if (newY < 0) {

			newY = PosY;

			SwitchDirection ();

		} else {
			if (Random.value < changeOfChangeDirection) {
				currentDirection = (Directions)Random.Range (0, 8);
			}
		}

		PosX = newX;
		PosY = newY;

	}

	private void SwitchDirection () {
//		Debug.Log ("switching direction from " + currentDirection.ToString () + " ...");

		currentDirection = NavigationManager.Instance.SwitchDirection (currentDirection);

//		Debug.Log ("... to " + currentDirection.ToString ());
	}

	public StoryManager StoryHandlers {
		get {
			return storyManager;
		}
	}
}
