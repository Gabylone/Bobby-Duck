﻿using UnityEngine;
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

		// STORY LAYERS
	private int previousLayer = 0;
	private int storyLayer = 0;

	[Header("Clue")]
	private int[] clueIslandsXPos;
	private int[] clueIslandsYPos;

	[Header("Treasure")]
	private int treasureIslandXPos = 0;
	private int treasureIslandYPos = 0;

	[Header("Home")]
	private int homeIslandXPos = 0;
	private int homeIslandYPos = 0;


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
		onIsland = true;

		MapManager.Instance.MapButton.Opened = false;
		MapManager.Instance.MapButton.Locked = true;

		NavigationManager.Instance.NavigationTriggers.SetActive (false);

		StartCoroutine (EnterCoroutine ());
	}

	IEnumerator EnterCoroutine () {

		Transitions.Instance.ScreenTransition.Switch ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		Transitions.Instance.ActionTransition.Switch();
		Transitions.Instance.ScreenTransition.Switch ();

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, Transitions.Instance.ActionTransition.Duration);

		if (IslandManager.Instance.CurrentIsland.Story == null) {
			IslandManager.Instance.CurrentIsland.Story = StoryLoader.Instance.RandomStory;
			Debug.Log ("setting new story to island");
		}

		StoryReader.Instance.Reset ();
		onIsland = true;

		yield return new WaitForSeconds (Transitions.Instance.ActionTransition.Duration);

		SoundManager.Instance.AmbianceSource.volume = SoundManager.Instance.AmbianceSource.volume / 2;

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region leave island
	public void Leave () {

		// une ile a UNE histoire. une histoire a DES histoire.
		if ( StoryLayer > 0 ) {
			string fallbackNode = CurrentIsland.Story.fallbackNode;

			StoryLayer = CurrentIsland.Stories.FindIndex (x => x.name == CurrentIsland.Story.fallbackStory);
			if (StoryLayer<0) {
				Debug.LogError ("pas trouvé de fall back story");
				return;
			}

//			StoryLayer = CurrentIsland.Story.fallbackStory;

			print ("target layer : " + storyLayer);

			Node node = StoryReader.Instance.GetNodeFromText (fallbackNode);
			StoryReader.Instance.GoToNode (node);

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

			if ( id >= islandIds.Length ) {
				Debug.LogError ("island ID out of range /// ID : " + id + " / LENGHT : " + IslandIds.Length);
				return IslandDatas[0];
			}

			return IslandDatas [id];
		}
		set {
			int id = IslandIds [MapManager.Instance.PosX, MapManager.Instance.PosY];
			IslandDatas [id] = value;
		}
	}
	#endregion

	#region properties
	public int PreviousLayer {
		get {
			return previousLayer;
		}
		set {
			previousLayer = value;
		}
	}
	public int StoryLayer {
		get {
			return storyLayer;
		}
		set {
			PreviousLayer = storyLayer;
			storyLayer = value;
		}
	}
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