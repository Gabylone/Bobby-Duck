using UnityEngine;
using System.Collections;

public class Boats : MonoBehaviour {

	public static Boats Instance;

	public static PlayerBoatInfo PlayerBoatInfo;
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

		NavigationManager.Instance.EnterNewChunk += HideBoat;
		NavigationManager.Instance.EnterNewChunk += UpdatePlayerBoatPosition;
		NavigationManager.Instance.EnterNewChunk += UpdateEnemyBoatPosition;

		playerBoat.Init ();
		otherBoat.Init ();

	}

	public int amountMult = 1;

	public void RandomizeBoats( ) {
		
		PlayerBoatInfo = new PlayerBoatInfo ();
		PlayerBoatInfo.Randomize ();

		otherBoatAmount = MapGenerator.Instance.MapScale * amountMult;

		otherBoatInfos = new OtherBoatInfo[otherBoatAmount];
		for (int i = 0; i < otherBoatInfos.Length; i++) {
			otherBoatInfos [i] = new OtherBoatInfo ();
			otherBoatInfos [i].Randomize ();
		}
	}

	void UpdateEnemyBoatPosition ()
	{
		foreach (OtherBoatInfo boat in OtherBoatInfos) {
			
			boat.UpdatePosition ();

			if ( boat.coords == Boats.PlayerBoatInfo.coords ) {

				ShowBoat (boat);

			}
		}

	}

	void UpdatePlayerBoatPosition () {
		PlayerBoatInfo.UpdatePosition ();

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

	public void LoadBoats () {
		PlayerBoatInfo = SaveManager.Instance.CurrentData.playerBoatInfo;
		otherBoatInfos = SaveManager.Instance.CurrentData.otherBoatInfos;
	}
	public void SaveBoats () {
		SaveManager.Instance.CurrentData.playerBoatInfo = PlayerBoatInfo;
		SaveManager.Instance.CurrentData.otherBoatInfos = OtherBoatInfos;
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
