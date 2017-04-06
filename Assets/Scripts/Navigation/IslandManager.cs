using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IslandManager : MonoBehaviour {

	public static IslandManager Instance;

	[SerializeField]
	private List<IslandData> islandDatas = new List<IslandData>();


	[Header("Island")]
	[SerializeField] private Image islandImage;

	bool onIsland = false;

	[Header("Clue")]
	[SerializeField] private int[] clueIslandsXPos;
	[SerializeField] private int[] clueIslandsYPos;

	[Header("Treasure")]
	[SerializeField] private int treasureIslandXPos = 0;
	[SerializeField] private int treasureIslandYPos = 0;

	[Header("Home")]
	[SerializeField] private int homeIslandXPos = 0;
	[SerializeField] private int homeIslandYPos = 0;

	[SerializeField]
	private GameObject navigationTriggers;

	[SerializeField] private Vector3 decal = Vector3.zero;


	void Awake() {
		Instance = this;
	}

	void Start () {
		foreach ( Animator animator in navigationTriggers.GetComponentsInChildren<Animator>() ){
			animator.SetBool ("feedback", true);
		}
	}

	void Update () {
		if ( Input.GetKeyDown (KeyCode.Q) )
			Leave ();
	}

	public void Enter (){
		
		if (OnIsland)
			return;
		
		MapManager.Instance.MapButton.Opened = false;
		MapManager.Instance.MapButton.Locked = true;

		navigationTriggers.SetActive (false);
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

		StartCoroutine (EnterCoroutine ());
	}

	public void DisableTuto () {
		foreach ( Animator animator in navigationTriggers.GetComponentsInChildren<Animator>() ){
			animator.SetBool ("feedback", false);
		}
	}

	IEnumerator EnterCoroutine () {

		Transitions.Instance.ScreenTransition.Switch ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);
		BoatManager.Instance.BoatTransform.position = IslandManager.Instance.IslandImage.transform.position + decal;

		Transitions.Instance.ActionTransition.Switch();
		Transitions.Instance.ScreenTransition.Switch ();

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, Transitions.Instance.ActionTransition.Duration);

		if (StoryLoader.Instance.CurrentIslandStory == null) {
			print ("histoire nulle : j'en prends unen nouvelle");
			StoryLoader.Instance.CurrentIslandStory = StoryLoader.Instance.RandomStory;
		}

		StoryReader.Instance.SetStory (StoryLoader.Instance.CurrentIslandStory);

		onIsland = true;

		yield return new WaitForSeconds (Transitions.Instance.ActionTransition.Duration);

		SoundManager.Instance.AmbianceSource.volume = SoundManager.Instance.AmbianceSource.volume / 2;

		StoryReader.Instance.UpdateStory ();
	}

	public void Leave () {

		onIsland = false;

		Transitions.Instance.ActionTransition.Switch();

		MapManager.Instance.MapButton.Locked = false;
		navigationTriggers.SetActive (true);

		Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);

		Crews.enemyCrew.Hide ();

		MapManager.Instance.CurrentIsland.visited = true;

		SoundManager.Instance.AmbianceSource.volume = SoundManager.Instance.AmbianceSource.volume * 2;
		WeatherManager.Instance.PlaySound ();
	}

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

	public void SetIsland () {

		islandImage.gameObject.SetActive ( MapManager.Instance.NearIsland );
		if ( MapManager.Instance.NearIsland )
			islandImage.transform.localPosition = MapManager.Instance.CurrentIsland.Position;
	}

	#region properties
	public List<IslandData> IslandDatas {
		get {
			return islandDatas;
		}
		set {
			islandDatas = value;
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

#region island date
[System.Serializable]
public class IslandData {

	[SerializeField]
	private Story story;

	private List<Loot> loots = new List<Loot>();
	private List<Crew> crews = new List<Crew>();

	private bool gaveClue = false;
	private Vector2 position;

	public bool visited = false;
	public bool seen = false;

	public IslandData ()
	{
		
	}

	public IslandData ( Vector2 pos )
	{
		position = pos;
	}

	public Vector2 Position {
		get {
			return position;
		}
	}

	public Story Story {
		get {
			return story;
		}
		set {
			story = value;
		}
	}

	public List<Loot> Loots {
		get {
			return loots;
		}
		set {
			loots = value;
		}
	}

	public List<Crew> Crews {
		get {
			return crews;
		}
		set {
			crews = value;
		}
	}
}
#endregion