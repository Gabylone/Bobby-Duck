using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boats : MonoBehaviour {

	public static Boats Instance;

	public static PlayerBoatInfo playerBoatInfo;

	BoatData boatData;

	public List<OtherBoatInfo> getBoats {
		//= new List<OtherBoatInfo> ();
		get {
			return boatData.boats;
		}
	}

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

		SaveManager.onSave += HandleOnSave;
		SaveManager.onLoad += HandleOnLoad;

		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void Update() {
		if ( Input.GetKeyDown(KeyCode.M) ) {
			HandleOnSave();
		}
	}

	public void RandomizeBoats( ) {

		playerBoatInfo = new PlayerBoatInfo ();
		playerBoatInfo.Init ();
		playerBoatInfo.Randomize ();


		boatData = new BoatData ();
		boatData.boats = new List<OtherBoatInfo> ();
		for (int i = 0; i < otherBoatAmount; i++) {
			OtherBoatInfo newBoat = new OtherBoatInfo ();
			newBoat.Init ();
			newBoat.Randomize ();
			boatData.boats.Add(newBoat);
		}
	}

	#region story
	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.DestroyShip ) {

			OtherBoatInfo boatInfo = EnemyBoat.Instance.OtherBoatInfo;
			boatData.boats.Remove (boatInfo);

			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

		}
	}
	#endregion

	#region karma
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
		OtherBoatInfo newBoat = new OtherBoatInfo ();
		newBoat.Init ();
		newBoat.Randomize ();

		int imperialID = StoryLoader.Instance.FindIndexByName ("Impériaux",StoryType.Boat);

		newBoat.storyManager.storyHandlers[0].storyID = imperialID;
			
		boatData.boats.Add (newBoat);

	}

	void RemoveImperialBoat ()
	{
		boatData.boats.RemoveAt(getBoats.Count-1);
	}
	#endregion

	#region save & load
	public void HandleOnSave () {

		SaveManager.Instance.GameData.playerBoatInfo = playerBoatInfo;

		SaveTool.Instance.SaveToPath ("boat data", boatData);

	}

	public void HandleOnLoad () {
		
		playerBoatInfo = SaveManager.Instance.GameData.playerBoatInfo;
		playerBoatInfo.Init ();

		boatData = SaveTool.Instance.LoadFromPath ("boat data", "BoatData") as BoatData;
		foreach (var item in getBoats) {
			item.Init ();
		}
	}
	#endregion
}

public class BoatData {
	
	public List<OtherBoatInfo>	boats;

	public BoatData() {
		//
	}

	public BoatData ( List<OtherBoatInfo> _boats ) {
		this.boats = _boats;
	}
}
