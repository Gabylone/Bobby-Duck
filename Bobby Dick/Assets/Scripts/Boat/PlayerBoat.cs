using UnityEngine;
using System.Collections;

public class PlayerBoat : Boat {

	public static PlayerBoat Instance;

	void Awake () {
		Instance = this;
	}

	public override void Start ()
	{
		base.Start ();

		BoatInfo = PlayerBoatInfo.Instance;

		NavigationManager.Instance.EnterNewChunk += UpdatePositionOnScreen;
	}

	public override void Update ()
	{
		base.Update ();
	}
}
