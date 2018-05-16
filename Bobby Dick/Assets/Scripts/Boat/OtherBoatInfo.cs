using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class OtherBoatInfo : BoatInfo {

	public StoryManager storyManager;

	private float changeOfChangeDirection = 0.2f;

	public override void Init ()
	{
		base.Init ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}


	public override void Randomize ()
	{
		base.Randomize ();

		SetCoords(MapGenerator.Instance.RandomCoords);
		currentDirection = (Directions)Random.Range (0,8);

		// assign story
		storyManager = new StoryManager ();
		storyManager.InitHandler (StoryType.Boat);
	}

	public override void UpdatePosition ()
	{
		base.UpdatePosition ();

		if ( Random.value < 0.4f ) {
			MoveToOtherChunk ();
		}
	}

	void HandleChunkEvent ()
	{
		UpdatePosition ();

		CheckForPlayer ();
	}

	void CheckForPlayer ()
	{
		if ( coords == Boats.playerBoatInfo.coords ) {
			//
			ShowOnScreen();

		}
	}

	void MoveToOtherChunk ()
	{
		Coords newCoords = coords + NavigationManager.Instance.getNewCoords (currentDirection);

		if (newCoords.x >= MapGenerator.Instance.MapScale - 1) {

			newCoords.x = coords.x;
			SwitchDirection ();

		} else if (newCoords.x < 0) {

			newCoords.x = coords.x;
			SwitchDirection ();
			//
		} else if (newCoords.y>= MapGenerator.Instance.MapScale - 1) {

			newCoords.y = coords.y;
			SwitchDirection ();

		} else if (newCoords.y < 0) {

			newCoords.y = coords.y;
			SwitchDirection ();

		} else {
			if (Random.value < changeOfChangeDirection) {
				currentDirection = (Directions)Random.Range (0, 8);
			}
		}

		SetCoords(newCoords);

	}

	private void SwitchDirection () {
//		Debug.Log ("switching direction from " + currentDirection.ToString () + " ...");

		currentDirection = NavigationManager.GetOppositeDirection (currentDirection);

//		Debug.Log ("... to " + currentDirection.ToString ());
	}

	public void ShowOnScreen () {
		EnemyBoat.Instance.Show (this);
	}

	void OnDestroy() {
		NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;
	}
}
