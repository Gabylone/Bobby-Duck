using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class OtherBoatInfo : BoatInfo {

	private float changeOfChangeDirection = 0.2f;

	private StoryManager storyManager;

	public override void Init ()
	{
		base.Init ();

		// assign story
		storyManager = new StoryManager ();
		storyManager.InitHandler (StoryType.Boat);

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}


	public override void Randomize ()
	{
		base.Randomize ();

		coords = MapGenerator.Instance.RandomCoords;
		currentDirection = (Directions)Random.Range (0,8);
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

		if ( coords == Boats.PlayerBoatInfo.coords ) {
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

		coords = newCoords;

	}

	private void SwitchDirection () {
//		Debug.Log ("switching direction from " + currentDirection.ToString () + " ...");

		currentDirection = NavigationManager.GetOppositeDirection (currentDirection);

//		Debug.Log ("... to " + currentDirection.ToString ());
	}

	public void ShowOnScreen () {
		EnemyBoat.Instance.Show (this);
	}

	public StoryManager StoryHandlers {
		get {
			return storyManager;
		}
	}

	void OnDestroy() {
		NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;
	}
}
