using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IslandManager : MonoBehaviour {

	public static IslandManager Instance;

	[Header("Island")]
	[SerializeField] private Image islandImage;

	bool onIsland = false;

	[Header("Clue")]
	[SerializeField] private int clueIslandIndex = 2;
	[SerializeField] private int clueIslandXPos = 0;
	[SerializeField] private int clueIslandYPos = 0;

	[Header("Treasure")]
	[SerializeField] private int treasureIslandIndex = 3;
	[SerializeField] private int treasureIslandXPos = 0;
	[SerializeField] private int treasureIslandYPos = 0;

	[Header("Home")]
	[SerializeField] private int homeIslandIndex = 1;
	[SerializeField] private int homeIslandXPos = 0;
	[SerializeField] private int homeIslandYPos = 0;

	[SerializeField] private Vector3 decal;

	[SerializeField]
	private bool playIntroduction = false;

	void Awake() {
		Instance = this;
	}

	void Update () {
		if ( Input.GetKeyDown (KeyCode.Q) )
			Leave ();
	}

	public void Enter (){
		if (OnIsland)
			return;
		MapManager.Instance.MapButton.Opened = false;

		StartCoroutine (EnterCoroutine ());
	}

	IEnumerator EnterCoroutine () {

		Transitions.Instance.ScreenTransition.Switch ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);
		BoatManager.Instance.BoatTransform.position = IslandManager.Instance.IslandImage.transform.position + decal;

		Transitions.Instance.ActionTransition.Switch();
		Transitions.Instance.ScreenTransition.Switch ();

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, Transitions.Instance.ActionTransition.Duration);

		if ( StoryLoader.Instance.CurrentIslandStory == null ) {
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

		Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);

		Crews.enemyCrew.Hide ();

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

}

public class IslandData {

	private Story story;

	private List<Loot> loots = new List<Loot>();
	private List<Crew> crews = new List<Crew>();

	private bool gaveClue = false;
	private Vector2 position;

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
