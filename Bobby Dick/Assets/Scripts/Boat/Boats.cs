using UnityEngine;
using System.Collections;

public class Boats : MonoBehaviour {

	public static Boats Instance;

	private PlayerBoatInfo playerBoatInfo;

	private OtherBoatInfo[] otherBoatInfos;
	private OtherBoatInfo otherBoatInfo;

	[Header("Boats")]
	[SerializeField]
	private PlayerBoat playerBoat;

	[SerializeField]
	private EnemyBoat otherBoat;

	[SerializeField]
	private int otherBoatAmount = 10;

	bool metBoat = false;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	public void Init () {

		playerBoatInfo = new PlayerBoatInfo ();
		playerBoat.BoatInfo = playerBoatInfo;

		otherBoatInfos = new OtherBoatInfo[otherBoatAmount];
		for (int i = 0; i < otherBoatInfos.Length; i++) {
			otherBoatInfos [i] = new OtherBoatInfo ();
		}

		NavigationManager.Instance.EnterNewChunk += HideBoat;
	}

	public void ShowBoat (OtherBoatInfo boatInfo)
	{
		otherBoat.OtherBoatInfo = boatInfo;
		otherBoatInfo = boatInfo;

		otherBoat.UpdatePositionOnScreen ();

		metBoat = true;

		otherBoat.Visible = true;

	}

	public void HideBoat () {

		if (!metBoat) {
			otherBoat.OtherBoatInfo = null;
			otherBoat.Visible = false;
		}

		metBoat = false;

	}

	public OtherBoatInfo[] OtherBoatInfos {
		get {
			return otherBoatInfos;
		}
	}

	public OtherBoatInfo OtherBoatInfo {
		get {
			return otherBoatInfo;
		}
		set {
			otherBoatInfo = value;
		}
	}

	public EnemyBoat OtherBoat {
		get {
			return otherBoat;
		}
	}
}
