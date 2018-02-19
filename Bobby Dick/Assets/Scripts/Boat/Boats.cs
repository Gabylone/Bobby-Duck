using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boats : MonoBehaviour {

	public static Boats Instance;

	public static PlayerBoatInfo playerBoatInfo;

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
		Karma.onChangeKarma += HandleOnChangeKarma;
	}

	void HandleOnChangeKarma (int previousKarma, int newKarma)
	{
		if (previousKarma > newKarma) {
			if (Karma.Instance.CurrentKarma > -Karma.Instance.maxKarma) {
				AddImperialBoat ();
			}
		} else {
			if (Karma.Instance.CurrentKarma < 0) {
				RemoveImperialBoat ();
			}
		}
	}

	void AddImperialBoat ()
	{
		OtherBoatInfo otherBoatInfo = new OtherBoatInfo ();
		otherBoatInfo.Init ();
		otherBoatInfo.Randomize ();

		int imperialID = StoryLoader.Instance.FindIndexByName ("Impériaux",StoryType.Boat);

		otherBoatInfo.storyManager.storyHandlers[0].storyID = imperialID;
			
		otherBoatInfos.Add(otherBoatInfo);

	}

	void RemoveImperialBoat ()
	{
		print ("removing imperial boat");

		otherBoatInfos.RemoveAt(OtherBoatInfos.Count-1);
	}

	public void RandomizeBoats( ) {

		playerBoatInfo = new PlayerBoatInfo ();
		playerBoatInfo.Init ();
		playerBoatInfo.Randomize ();

		for (int i = 0; i < otherBoatAmount; i++) {
			OtherBoatInfo otherBoatInfo = new OtherBoatInfo ();
			otherBoatInfo.Init ();
			otherBoatInfo.Randomize ();
			otherBoatInfos.Add(otherBoatInfo);
		}
	}

	public void LoadBoats () {
		playerBoatInfo = SaveManager.Instance.GameData.playerBoatInfo;
		otherBoatInfos = SaveManager.Instance.GameData.otherBoatInfos;

		playerBoatInfo.Init ();
		foreach (var item in otherBoatInfos) {
			item.Init ();
		}
	}
	public void SaveBoats () {
		SaveManager.Instance.GameData.playerBoatInfo = playerBoatInfo;
		SaveManager.Instance.GameData.otherBoatInfos = OtherBoatInfos;
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
