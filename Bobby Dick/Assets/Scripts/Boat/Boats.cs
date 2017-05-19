using UnityEngine;
using System.Collections;

public class Boats : MonoBehaviour {

	public static Boats Instance;

	private PlayerBoatInfo playerBoatInfo;

	private OtherBoatInfo[] otherBoatInfos;

	[Header("Boats")]
	[SerializeField]
	private PlayerBoat playerBoat;

	[SerializeField]
	private EnemyBoat otherBoat;

	[SerializeField]
	private int otherBoatAmount = 10;

	void Awake () {
		Instance = this;

	}

	// Use this for initialization
	public void Init () {

		NavigationManager.Instance.EnterNewChunk += CheckForBoats;

		playerBoatInfo = new PlayerBoatInfo ();
		playerBoat.BoatInfo = playerBoatInfo;

		otherBoatInfos = new OtherBoatInfo[otherBoatAmount];
		for (int i = 0; i < otherBoatInfos.Length; i++) {
			otherBoatInfos [i] = new OtherBoatInfo ();
		}
	}

	void CheckForBoats ()
	{
		foreach ( OtherBoatInfo otherBoatInfo in BoatInfos ) {

			if ( playerBoatInfo.PosX == otherBoatInfo.PosX && playerBoatInfo.PosY == otherBoatInfo.PosY ) {

				otherBoat.UpdatePositionOnScreen ();

				otherBoat.OtherBoatInfo = otherBoatInfo;
				return;
			}

		}

		otherBoat.gameObject.SetActive (false);
	}

	public BoatInfo[] BoatInfos {
		get {
			return otherBoatInfos;
		}
	}
}
