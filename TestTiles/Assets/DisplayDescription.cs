using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDescription : TextTyper {
	public static DisplayDescription Instance;

	void Awake () {
		Instance = this;
	}

	public override void Start ()
	{
		base.Start ();

		Player.onPlayerMove += HandleOnPlayerMove;

		ActionManager.onAction += HandleOnAction;

	}

	void HandleOnAction (Action action)
	{
		if ( action.type == Action.Type.Describe ) {
			DisplayAllSurroundingTiles ();
		}
	}

	void HandleOnPlayerMove (Coords previousCoords, Coords newCoords)
	{
		Clear ();

		DisplayCurrentTileDescription ();

//		DisplayTileItems ();
//
//		DisplaySurroundingTiles();
//
//		DisplayWeather ();
//
//		DisplayCharacterStates ();
//
//		DisplayOtherCharactersDescription ();
//
//		DisplayZombies ();

		UpdateText ();
	}

	#region currenttile description 
	void DisplayCurrentTileDescription ()
	{
		Tile currentTile = Tile.current;

		string intro = "Vous êtes";

		Word word = Word.GetLocationWord (currentTile.type);
		string str = intro + " " + word.GetDescription(Word.Def.Undefined , Word.Preposition.Loc, Word.Number.Singular);

		AddToText (str);

		SkipLign (2);
	}
	#endregion

	#region tile surrounding description 
	struct SurroudingTile {

		public Tile tile;

		public List<Player.Facing> facings;

	}
	void DisplaySurroundingTiles()
	{
		List<SurroudingTile> surroundingTiles = new List<SurroudingTile> ();

//		for (int i = 0; i < 8; ++i) {
//		for (int i = 0; i < 8; i += 2) {

		List<Player.Facing> facings = new List<Player.Facing> ();
		facings.Add (Player.Facing.Front);
		facings.Add (Player.Facing.Right);
		facings.Add (Player.Facing.Left);

		foreach (var facing in facings) {

			Direction dir = Player.Instance.GetDirection (facing);
			Coords targetCoords = Player.Instance.coords + (Coords)dir;

			Tile tile = Tile.GetTile (targetCoords);

			if (tile == null)
				continue;

			SurroudingTile surroundingTile = new SurroudingTile ();

			surroundingTile.tile = tile;

			surroundingTile.facings = new List<Player.Facing>();
			surroundingTile.facings.Add (facing);

			surroundingTiles.Add (surroundingTile);

		}

		foreach (var surroundingTile in surroundingTiles) {

//			Debug.Log (surroundingTile);
//			Debug.Log (surroundingTile.facings);
			string direction_str = Coords.GetPhraseDirecton(surroundingTile.facings[0]);

			string visionPhrase = visionPhrases [Random.Range (0, visionPhrases.Length)];

			if (surroundingTile.tile.type == Tile.current.type ) {

				string description = surroundingTile.tile.word.GetDescription (Word.Def.Defined, Word.Preposition.None, Word.Number.Singular);

				AddToText (direction_str + ", " + description + " continue");

			} else {

				string description = surroundingTile.tile.word.GetDescription (Word.Def.Undefined, Word.Preposition.None, Word.Number.Singular);

//				AddToText (direction_str + ", " + visionPhrase + " " + description);
				AddToText (direction_str + ", " + description);
			}

			SkipLign (2);

		}

		SkipLign ();	
	}
	void DisplayAllSurroundingTiles ()
	{
		Clear ();
		List<SurroudingTile> surroundingTiles = new List<SurroudingTile> ();

		List<Player.Facing> facings = new List<Player.Facing> ();
		facings.Add (Player.Facing.Front);
		facings.Add (Player.Facing.FrontRight);
		facings.Add (Player.Facing.Right);
		facings.Add (Player.Facing.BackRight);
		facings.Add (Player.Facing.Back);
		facings.Add (Player.Facing.BackLeft);
		facings.Add (Player.Facing.Left);
		facings.Add (Player.Facing.FrontLeft);

		foreach (var facing in facings) {

			Direction dir = Player.Instance.GetDirection (facing);
			Coords targetCoords = Player.Instance.coords + (Coords)dir;

			Tile tile = Tile.GetTile (targetCoords);

			if (tile == null)
				continue;

			SurroudingTile surroundingTile = new SurroudingTile ();

			surroundingTile.tile = tile;

			surroundingTile.facings = new List<Player.Facing>();
			surroundingTile.facings.Add (facing);

			surroundingTiles.Add (surroundingTile);

		}

		foreach (var surroundingTile in surroundingTiles) {

			string description = surroundingTile.tile.word.GetDescription (Word.Def.Undefined, Word.Preposition.None, Word.Number.Singular);

			string direction_str = Coords.GetPhraseDirecton(surroundingTile.facings[0]);

			string visionPhrase = visionPhrases [Random.Range (0, visionPhrases.Length)];

			if (surroundingTile.tile.type == Tile.current.type && surroundingTile.tile.word.adjType == Adjective.Type.Rural ) {

				AddToText (direction_str + ", " + description + " continue");

			} else {

				AddToText (direction_str + ", " + visionPhrase + " " + description);
			}

			SkipLign (2);

		}

		SkipLign ();
	}
	#endregion

	#region tile items description 

	public string[] positionPhrases;
	public string[] visionPhrases;
	void DisplayTileItems ()
	{
		int typeCount = 0;

		if (Tile.current.items.Count == 0) {
			AddToText ("Il n'y a rien à voir");
			return;
		}


		foreach (var item in Tile.current.items ) {

			string positionPhrase = positionPhrases [Random.Range (0, positionPhrases.Length)];
			AddToText (positionPhrase + ", ");

			string visionPhrase = visionPhrases [Random.Range (0, visionPhrases.Length)];
			AddToText (visionPhrase + " ");

			string word = item.word.GetDescription (Word.Def.Undefined);

			AddToText (word);

			SkipLign ();

		}
	}
	#endregion

	#region weather description
	void DisplayWeather ()
	{
		
	}
	#endregion

	#region player states description
	void DisplayCharacterStates ()
	{
		SkipLign ();
		if ( Player.Instance.sleep == Player.Instance.maxSleep ) {
			SkipLign ();
			AddToText ("Vos paupières se ferment toutes seules, vous êtes épuisé");
		} else if ( Player.Instance.sleep > (float)(Player.Instance.maxSleep * 0.8f) ) {
			SkipLign ();
			AddToText ("Vous vous sentez légerement fatigué...");
		}

		if ( Player.Instance.hunger == Player.Instance.maxHunger ) {
			SkipLign ();
			AddToText ("Votre ventre est vide, vous avez faim");
		} else if ( Player.Instance.hunger > (float)(Player.Instance.maxHunger * 0.8f) ) {
			SkipLign ();
			AddToText ("Votre ventre commence à gargouiller...");
		}

		if ( Player.Instance.thirst == Player.Instance.maxThirst ) {
			SkipLign ();
			AddToText ("Votre gorge est sêche, vous avez soif");
		} else if ( Player.Instance.thirst > (float)(Player.Instance.maxThirst * 0.8f) ) {
			SkipLign ();
			AddToText ("Vos lévres se durcissent, vous ressentez une légère soif...");
		}
	}
	#endregion

	#region other character description
	void DisplayOtherCharactersDescription ()
	{

	}
	#endregion

	#region zombi descriptions
	void DisplayZombies ()
	{

	}
	#endregion
}
