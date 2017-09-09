using UnityEngine;
using System.Collections;

public class PlayerBoat : Boat {

	public static PlayerBoat Instance;

	void Awake () {
		Instance = this;
	}

	public override void Init ()
	{
		base.Init();

		BoatInfo = Boats.Instance.PlayerBoatInfo;

		NavigationManager.Instance.EnterNewChunk += UpdatePositionOnScreen;

	}

	public override void UpdatePositionOnScreen ()
	{
		base.UpdatePositionOnScreen ();

		Vector2 getDir = NavigationManager.Instance.getDir(BoatInfo.currentDirection);
		GetTransform.position = NavigationManager.Instance.Anchors[(int)BoatInfo.currentDirection].position;
	}

	public override void Update ()
	{
		if (StoryLauncher.Instance.PlayingStory == false) {
			base.Update ();
		}
	}
}
