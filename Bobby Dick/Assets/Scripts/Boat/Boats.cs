using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boats : MonoBehaviour {

	public static Boats Instance;

	public static PlayerBoatInfo PlayerBoatInfo;

	public List<OtherBoatInfo> otherBoatInfos = new List<OtherBoatInfo> ();

	[SerializeField]
	private int otherBoatAmount = 10;

	[Header("Movement")]
	[SerializeField]
	private float chanceOfMoving = 0.5f;

	[SerializeField]
	private float timeToMove = 20f;

	private float timer = 0f;

	void Awake () {
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.AddKarma:
			if (Karma.Instance.CurrentKarma < 0) {
				RemoveImperialBoat ();
			}
			break;
		case FunctionType.RemoveKarma:
			if ( Karma.Instance.CurrentKarma > -Karma.Instance.maxKarma )
				AddImperialBoat ();
			break;
		default:
			break;
		}
	}

	void AddImperialBoat ()
	{
		OtherBoatInfo otherBoatInfo = new OtherBoatInfo ();
		otherBoatInfo.Init ();
		otherBoatInfo.Randomize ();

		int imperialID = StoryLoader.Instance.FindIndexByName ("Impériaux",StoryType.Boat);

		otherBoatInfo.StoryHandlers.storyHandlers[0].storyID = imperialID;

		otherBoatInfos.Add(otherBoatInfo);


	}

	void RemoveImperialBoat ()
	{
		print ("removing imperial boat");

		otherBoatInfos.RemoveAt(OtherBoatInfos.Count-1);
	}

	public void RandomizeBoats( ) {

		PlayerBoatInfo = new PlayerBoatInfo ();
		PlayerBoatInfo.Init ();

		for (int i = 0; i < otherBoatAmount; i++) {
			OtherBoatInfo otherBoatInfo = new OtherBoatInfo ();
			otherBoatInfo.Init ();
			otherBoatInfo.Randomize ();
			otherBoatInfos.Add(otherBoatInfo);
		}
	}

	public void LoadBoats () {
		PlayerBoatInfo = SaveManager.Instance.CurrentData.playerBoatInfo;
		otherBoatInfos = SaveManager.Instance.CurrentData.otherBoatInfos;
	}
	public void SaveBoats () {
		SaveManager.Instance.CurrentData.playerBoatInfo = PlayerBoatInfo;
		SaveManager.Instance.CurrentData.otherBoatInfos = OtherBoatInfos;
	}

	public List<OtherBoatInfo> OtherBoatInfos {
		get {
			return otherBoatInfos;
		}
		set {
			otherBoatInfos = value;
		}
	}
}
