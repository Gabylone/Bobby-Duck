using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public static Game Instance;

	public static List <Player> players = new List<Player> ();

	public enum Phase {
		Photosynthesis,
		LifeCycle,
	}

	public static Phase phase;

	public GameObject canvasObj;

	public static int turn = 0;
	public static int cycle = 0;

	public static int firstPlayerIndex = 0;
	public static int playerIndex = 0;

	public static Player currentPlayer;
	public static Player previousPlayer;

	public delegate void OnNextPlayer (Player player);
	public static OnNextPlayer onNextPlayer;

	public delegate void OnNextPhase ( Phase phase );
	public static OnNextPhase onNextPhase;

	public delegate void OnNextTurn ( int turn );
	public static OnNextTurn onNextTurn;

	public delegate void OnNextCycle ( int cycle );
	public static OnNextCycle onNextCycle;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		players.Add (new Player(PlayerColor.Blue));
		players.Add (new Player(PlayerColor.Orange));
		players.Add (new Player(PlayerColor.Green));
		players.Add (new Player(PlayerColor.Yellow));

		soilMats = Resources.LoadAll<Material> ("SoilMats");

		StartGame ();
	}
//	void Update () {
//		if (Input.GetKeyDown (KeyCode.UpArrow))
//			EndPlayer ();
//	}

	#region player
	void StartPlayer ( ) {

		if (currentPlayer != null)
			previousPlayer = currentPlayer;
		currentPlayer = players[playerIndex];

		if (onNextPlayer != null)
			onNextPlayer (currentPlayer);
		//
	}
	public void EndPlayer () {
		
		++playerIndex;

		if (playerIndex == players.Count)
			playerIndex = 0;

		if (playerIndex == firstPlayerIndex) {
			EndPhase ();
		} else {
			StartPlayer ();
		}
	}
	#endregion

	#region phase
	void StartPhase ( Phase _phase ) {
		
		phase = _phase;

		StartPlayer ();

		if (onNextPhase != null)
			onNextPhase (_phase);
	}
	void EndPhase () {
		
		switch (phase) {
		case Phase.Photosynthesis:
			StartPhase (Phase.LifeCycle);
			break;
		case Phase.LifeCycle:
			EndTurn ();
			break;
		default:
			break;
		}

	}
	#endregion

	#region turn
	void StartTurn ()
	{
		StartPhase (Phase.Photosynthesis);

		if (onNextTurn != null)
			onNextTurn (turn);

	}
	void EndTurn ()
	{
		NextFirstPlayer ();

		NextTurn ();
	}
	void NextFirstPlayer () {
		firstPlayerIndex++;
		if (firstPlayerIndex == players.Count)
			firstPlayerIndex = 0;

		playerIndex = firstPlayerIndex;
	}
	void NextTurn () {
		
		++turn;
		if (turn == 6) {
			turn = 0;
			EndCycle ();
		} else {
			StartTurn ();
		}

	}
	#endregion

	#region cycle
	void StartCycle () {

		StartTurn ();

		if (onNextCycle != null)
			onNextCycle (cycle);

	}
	void EndCycle ()
	{
		++cycle;
		if (cycle == 3) {
			EndGame ();
		} else {
			NextCycle ();
		}
	}

	void NextCycle ()
	{
		StartCycle ();
	}
	#endregion

	#region game
	void StartGame() {
		StartCycle ();
	}
	void EndGame () {
	}
	#endregion

	public static Player GetPlayer ( PlayerColor playerColor ) {
		return players [(int)playerColor];
	}

	public static Material[] soilMats;

	public static Color[] playerColors = new Color[4] {
		Color.blue,
		new Color(0.8f,0.5f,0f),
		Color.green,
		Color.yellow
	};

	public static Color GetColor ( Player player ) {
		return playerColors[(int)player.playerColor];
	}

}

public enum PlayerColor {
	Blue,
	Orange,
	Green,
	Yellow,

	None,
}

public class Player {

	public Player ( PlayerColor _playerColor ) {

		this.playerColor = _playerColor;
	}

	public PlayerColor playerColor;

	public int score;

	public int sunPoints = 0;

	public delegate void OnAddPoints ( int _sunPoints );
	public OnAddPoints onAddPoints;
	public void AddPoint (int _sunPoints)
	{
		sunPoints += _sunPoints;

		if (onAddPoints != null)
			onAddPoints (sunPoints);
	}
}