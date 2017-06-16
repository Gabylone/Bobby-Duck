using UnityEngine;
using System.Collections;

public class Boats : MonoBehaviour {

	public static Boats Instance;

	[SerializeField]
	private PlayerBoatInfo playerBoatInfo;
	[SerializeField]
	private OtherBoatInfo[] otherBoatInfos;

	[Header("Boats")]
	[SerializeField]
	private PlayerBoat playerBoat;

	[SerializeField]
	private EnemyBoat otherBoat;

	[SerializeField]
	private int otherBoatAmount = 10;

	[Header("Movement")]
	[SerializeField]
	private float chanceOfMoving = 0.5f;

	[SerializeField]
	private float timeToMove = 20f;

	private float timer = 0f;
	private bool metBoat = false;

	void Awake () {
		Instance = this;
	}


	// Use this for initialization
	public void Init () {

		playerBoatInfo = new PlayerBoatInfo ();
		playerBoatInfo.Init ();
		playerBoat.BoatInfo = playerBoatInfo;

		otherBoatAmount = MapGenerator.Instance.MapScale;

		otherBoatInfos = new OtherBoatInfo[otherBoatAmount];
		for (int i = 0; i < otherBoatInfos.Length; i++) {
			otherBoatInfos [i] = new OtherBoatInfo ();
			otherBoatInfos [i].Init ();
		}

		NavigationManager.Instance.EnterNewChunk += HideBoat;
		NavigationManager.Instance.EnterNewChunk += UpdateEnemyBoatPosition;
		NavigationManager.Instance.EnterNewChunk += UpdatePlayerBoatPosition;
	}

	void UpdateEnemyBoatPosition ()
	{
		foreach (OtherBoatInfo boat in OtherBoatInfos) {
			
			boat.UpdatePosition ();

			if ( boat.PosX == PlayerBoatInfo.Instance.PosX && boat.PosY == PlayerBoatInfo.Instance.PosY ) {

				ShowBoat (boat);

			}
		}

	}

	void UpdatePlayerBoatPosition () {
		playerBoatInfo.UpdatePosition ();

	}

	public void ShowBoat (OtherBoatInfo boatInfo)
	{
		otherBoat.OtherBoatInfo = boatInfo;

		otherBoat.UpdatePositionOnScreen ();

		metBoat = true;

		otherBoat.Visible = true;

	}

	public void HideBoat () {

		otherBoat.OtherBoatInfo = null;
		otherBoat.Visible = false;

		metBoat = false;

	}

	public OtherBoatInfo[] OtherBoatInfos {
		get {
			return otherBoatInfos;
		}
		set {
			otherBoatInfos = value;
		}
	}

	public EnemyBoat OtherBoat {
		get {
			return otherBoat;
		}
	}
}
