using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IslandManager : MonoBehaviour {

	public static IslandManager Instance;

	[Header("Island")]
	[SerializeField] private Image islandImage;

	[SerializeField]
	private List<IslandData> islandDatas = new List<IslandData>();

	[SerializeField]
	private int[,] islandIds;

	private bool onIsland = false;

	[Header("Clue")]
	private int[] clueIslandsXPos;
	private int[] clueIslandsYPos;

	[Header("Treasure")]
	private int treasureIslandXPos = 0;
	private int treasureIslandYPos = 0;

	[Header("Home")]
	private int homeIslandXPos = 0;
	private int homeIslandYPos = 0;

	// STORY //
	private string secondStory_FallbackMark = "";

	[SerializeField] private Vector3 boat_DecalToIsland = Vector3.zero;


	void Awake() {
		Instance = this;
	}

	void Update () {
		if ( Input.GetKeyDown (KeyCode.Q) )
			Leave ();
	}

	#region island image
	public void UpdateIslandPosition () {
		islandImage.gameObject.SetActive ( MapManager.Instance.NearIsland );
		if ( MapManager.Instance.NearIsland )
			islandImage.transform.localPosition = MapManager.Instance.CurrentIsland.Position;
	}
	#endregion

	#region enter island
	public void Enter (){
		
		if (OnIsland)
			return;
		
		MapManager.Instance.MapButton.Opened = false;
		MapManager.Instance.MapButton.Locked = true;

		NavigationManager.Instance.NavigationTriggers.SetActive (false);

		StartCoroutine (EnterCoroutine ());
	}

	IEnumerator EnterCoroutine () {

		Transitions.Instance.ScreenTransition.Switch ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);
		BoatManager.Instance.BoatTransform.position = IslandManager.Instance.IslandImage.transform.position + boat_DecalToIsland;

		Transitions.Instance.ActionTransition.Switch();
		Transitions.Instance.ScreenTransition.Switch ();

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, Transitions.Instance.ActionTransition.Duration);

		if (IslandManager.Instance.CurrentIsland.Story == null) {
			IslandManager.Instance.CurrentIsland.Story = StoryLoader.Instance.RandomStory;
		}

		StoryReader.Instance.SetStory (IslandManager.Instance.CurrentIsland.Story);

		onIsland = true;

		yield return new WaitForSeconds (Transitions.Instance.ActionTransition.Duration);

		SoundManager.Instance.AmbianceSource.volume = SoundManager.Instance.AmbianceSource.volume / 2;

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region leave island
	public void Leave () {

		if ( false ) {
//		if ( StoryLoader.Instance.SecondStory_Active ) {

			print ("back to initial story");

//			StoryLoader.Instance.SecondStory_Active = false;

			Mark mark = IslandManager.Instance.CurrentIsland.Story.marks.Find ( x => x.name == secondStory_FallbackMark);
			StoryReader.Instance.Decal = mark.x;
			StoryReader.Instance.Index = mark.y;

			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

			return;

		}

		onIsland = false;

		Transitions.Instance.ActionTransition.Switch();

		MapManager.Instance.MapButton.Locked = false;
		NavigationManager.Instance.NavigationTriggers.SetActive (true);

		Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);

		Crews.enemyCrew.Hide ();

		MapManager.Instance.CurrentIsland.visited = true;

		SoundManager.Instance.AmbianceSource.volume = SoundManager.Instance.AmbianceSource.volume * 2;
		WeatherManager.Instance.PlaySound ();
	}
	#endregion

	#region propeties
	public bool OnIsland {
		get {
			return onIsland;
		}
	}

	public Image IslandImage {
		get {
			return islandImage;
		}
		set {
			islandImage = value;
		}
	}

	public IslandData CurrentIsland {
		get {

			int id = IslandIds [MapManager.Instance.PosX, MapManager.Instance.PosY];
			return IslandDatas [id];
		}
		set {
			int id = IslandIds [MapManager.Instance.PosX, MapManager.Instance.PosY];
			IslandDatas [id] = value;
		}
	}
	#endregion

	#region properties
	public List<IslandData> IslandDatas {
		get {
			return islandDatas;
		}
		set {
			islandDatas = value;
		}
	}
	public int[,] IslandIds {
		get {
			return islandIds;
		}
		set {
			islandIds = value;
		}
	}

	public int TreasureIslandXPos {
		get {
			return treasureIslandXPos;
		}
		set {
			treasureIslandXPos = value;
		}
	}

	public int TreasureIslandYPos {
		get {
			return treasureIslandYPos;
		}
		set {
			treasureIslandYPos = value;
		}
	}

	public int HomeIslandXPos {
		get {
			return homeIslandXPos;
		}
		set {
			homeIslandXPos = value;
		}
	}

	public int HomeIslandYPos {
		get {
			return homeIslandYPos;
		}
		set {
			homeIslandYPos = value;
		}
	}

	public int[] ClueIslandsXPos {
		get {
			return clueIslandsXPos;
		}
		set {
			clueIslandsXPos = value;
		}
	}

	public int[] ClueIslandsYPos {
		get {
			return clueIslandsYPos;
		}
		set {
			clueIslandsYPos = value;
		}
	}
	#endregion
}