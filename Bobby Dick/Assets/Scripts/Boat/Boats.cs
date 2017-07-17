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

		NavigationManager.Instance.EnterNewChunk += HideBoat;
		NavigationManager.Instance.EnterNewChunk += UpdatePlayerBoatPosition;
		NavigationManager.Instance.EnterNewChunk += UpdateEnemyBoatPosition;

		playerBoat.BoatInfo = playerBoatInfo;

		playerBoat.Init ();
		otherBoat.Init ();

	}

	public int amountMult = 1;

	public void RandomizeBoats( ) {
		
		playerBoatInfo = new PlayerBoatInfo ();
		playerBoatInfo.Randomize ();

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

			if ( boat.PosX == Boats.Instance.PlayerBoatInfo.PosX && boat.PosY == Boats.Instance.PlayerBoatInfo.PosY ) {

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

	public void LoadBoats () {
		playerBoatInfo = SaveManager.Instance.CurrentData.playerBoatInfo;
		otherBoatInfos = SaveManager.Instance.CurrentData.otherBoatInfos;
	}
	public void SaveBoats () {
		SaveManager.Instance.CurrentData.playerBoatInfo = playerBoatInfo;
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

	public PlayerBoatInfo PlayerBoatInfo {
		get {
			return playerBoatInfo;
		}
	}
}
