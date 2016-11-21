using UnityEngine;
using System.Collections;

public class StoryFunctions : MonoBehaviour {

	public static StoryFunctions Instance;

	string cellParams = "";

	private string[] functionNames;

	void Awake () {
		Instance = this;
	}

	public void Read ( string content ) {

		foreach ( string functionName in functionNames ) {

			if ( content.Contains (functionName) ){

				cellParams = content.Remove (0, functionName.Length);

				SendMessage (functionName);

				return;
			}

		}

		Debug.LogError (
			"cell returns no function at decal\n" + StoryReader.Instance.Decal + "\n" +
			"index : " + StoryReader.Instance.Index + "\n" +
			"qui contient : " + content);

	}

	#region random
	void RandomPercent () {

		float chance = float.Parse ( cellParams );
		int randomDecal = Random.value * 100f < chance ? 0 : 1;

		StoryReader.Instance.NextCell ();

		int decal = StoryLoader.Instance.SaveDecal > -1 ? StoryLoader.Instance.SaveDecal : randomDecal;
		StoryLoader.Instance.SaveDecal = decal;

		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();

	}
	void RandomRange () {

		int range = int.Parse (cellParams);
		int randomDecal = Random.Range (0, range);

		StoryReader.Instance.NextCell ();

		int decal = StoryLoader.Instance.SaveDecal > -1 ? StoryLoader.Instance.SaveDecal : randomDecal;
		StoryLoader.Instance.SaveDecal = decal;

		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();


	}
	#endregion

	#region character & crew
	Crew GetCrew (int amount) {
		
		int row = StoryReader.Instance.Decal;
		int col = StoryReader.Instance.Index;

		var tmp = MapManager.Instance.CurrentIsland.Crews.Find (x => x.col == col && x.row == row);

		if (tmp == null) {

			Crew newCrew = new Crew (amount, row, col);

			MapManager.Instance.CurrentIsland.Crews.Add (newCrew);

			return newCrew;

		}

		return tmp;
	}

	void NewCharacter() {

		Crews.enemyCrew.createCrew (GetCrew (1));

		StoryReader.Instance.Wait ( Crews.playerCrew.captain.Icon.MoveDuration );

	}

	void NewCrew () {
		
		int l = Crews.playerCrew.CrewMembers.Count;

		int amount = Random.Range ( l-1 , l+2 );

		Crews.enemyCrew.createCrew (GetCrew (amount));

		StoryReader.Instance.Wait (Crews.playerCrew.captain.Icon.MoveDuration);

	}
	void HideOther () {
		Crews.enemyCrew.Hide ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void DeleteOther () {
		Crews.enemyCrew.DeleteCrew ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region dialogue
	void Narrator () {

		string phrase = cellParams.Remove (0,2);

		DialogueManager.Instance.ShowNarrator (phrase);

		StoryReader.Instance.WaitForInput ();

	}
	void OtherSpeak () {

		string phrase = cellParams.Remove (0,2);

		if ( Crews.enemyCrew.CrewMembers.Count == 0 ) {
			Debug.LogError ("no enemy crew for other speak");
			return;
		}

		Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		DialogueManager.Instance.SetDialogue (phrase, Crews.enemyCrew.captain.Icon.GetTransform);

		StoryReader.Instance.WaitForInput ();

	}

	void PlayerSpeak () {
		
		string phrase = cellParams.Remove (0,2);

		DialogueManager.Instance.SetDialogue (phrase, Crews.playerCrew.captain.Icon.GetTransform);

		StoryReader.Instance.WaitForInput ();
	}

	void SetChoices () {

		// get amount
		int amount = int.Parse (cellParams);

		// get bubble content
		StoryReader.Instance.NextCell ();

		string[] choices = new string[amount];

		int tmpDecal = StoryReader.Instance.Decal;
		int a = amount;

		while ( a > 0 ) {

			if ( StoryLoader.Instance.ReadDecal (tmpDecal).Length > 0 ) {

				string choice = StoryLoader.Instance.ReadDecal (tmpDecal);
				choice = choice.Remove (0, 9);

				choices [amount - a] = choice;

				--a;
			}

			++tmpDecal;

			if ( tmpDecal > 20 ) {
				Debug.LogError ("reached while limit");
				break;
			}

			if (a <= 0)
				break;
		}

		DiscussionManager.Instance.SetChoices (amount, choices);

	}
	#endregion

	#region end
	void LaunchCombat () {
		CombatManager.Instance.StartCombat ();
	}
	void Leave () {
		IslandManager.Instance.Leave ();
	}
	void Conclude () {
		Debug.LogError ("a CODER : CONCLUDE");
		IslandManager.Instance.Leave ();
	}
	#endregion

	#region gold
	void CheckGold () {
		int amount = int.Parse (cellParams);

		if (GoldManager.Instance.CheckGold (amount)) {
			StoryReader.Instance.NextCell ();
			GoldManager.Instance.RemoveGold (amount);
		} else {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.SetDecal (1);
		}

		StoryReader.Instance.UpdateStory ();
	}
	void RemoveGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.RemoveGold (amount);
		StoryReader.Instance.Wait ( 1f );
	}
	void AddGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.AddGold (amount);
		StoryReader.Instance.Wait ( 1f );
	}
	#endregion

	#region trade & loot
	void Loot() {
		LootManager.Instance.setLoot ( Crews.Side.Enemy, LootManager.Instance.GetIslandLoot(getLootCategories()));
		OtherLoot.Instance.StartLooting ();
	}
	void Trade() {
		LootManager.Instance.setLoot ( Crews.Side.Enemy, LootManager.Instance.GetIslandLoot(getLootCategories()));
		OtherLoot.Instance.StartTrade ();
	}

	public ItemCategory[] getLootCategories () {
		string[] cellParts = cellParams.Split ('/');
		ItemCategory[] categories = new ItemCategory[cellParts.Length];

		int index = 0;

		foreach ( string cellPart in cellParts ) {

			switch (cellPart) {

			case "All":
				categories = ItemLoader.allCategories;
				break;
			case "Food":
				categories [index] = ItemCategory.Provisions;
				break;
			case "Weapons":
				categories [index] = ItemCategory.Weapon;
				break;
			case "Clothes":
				categories [index] = ItemCategory.Clothes;
				break;
			case "Shoes":
				categories [index] = ItemCategory.Shoes;
				break;
			case "Misc":
				categories [index] = ItemCategory.Mics;
				break;
			}

			++index;
		}

		return categories;
	}
	#endregion

	#region story navigation
	void GoTo () {

		string[] coords = cellParams.Split ('/');

		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		int a = 0;

		char targetChar = coords [0] [0];

		Debug.Log ("Target Char : " + targetChar);

		foreach (char c in alphabet) {
			if (c == targetChar) {
				break;
			}
			a++;
		}

		int newIndex = int.Parse (coords [1]) - 3;

		Debug.Log ("nouveau decal : " + a + " / nouveau index : " + newIndex);

		StoryReader.Instance.Decal = a;
		StoryReader.Instance.Index = newIndex;

		StoryReader.Instance.UpdateStory ();

	}
	#endregion

	#region weather 
	void SetWeather() {

		switch ( cellParams ) {
		case "Day":
			NavigationManager.Instance.IsNight = false;
			break;
		case "Night":
			NavigationManager.Instance.IsNight = true;
			break;
		case "Rain":
			NavigationManager.Instance.Raining = true;
			break;
		}

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void NextDay () {
		
		StartCoroutine (NextDayCoroutine ());
	}
	IEnumerator NextDayCoroutine () {

		if (Crews.enemyCrew.CrewMembers.Count > 0) {
			Crews.enemyCrew.Hide ();

			yield return new WaitForSeconds (0.5f);
		}

		NavigationManager.Instance.Move (Directions.None);

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration * 2);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();

	}
	void CheckDay () {

		StoryReader.Instance.NextCell ();

		if (NavigationManager.Instance.IsNight)
			StoryReader.Instance.SetDecal (1);

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region clues
	void CheckClues () {
		ClueManager.Instance.StartClue ();
	}
	void GiveClue() {

		string formula = getFormula ();

		Debug.Log (formula);


		if ( Crews.enemyCrew.CrewMembers.Count == 0 ) {
			DialogueManager.Instance.ShowNarrator (formula);
		} else {
			Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
			DialogueManager.Instance.SetDialogue (getFormula(), Crews.enemyCrew.captain.Icon.GetTransform);

		}

		StoryReader.Instance.WaitForInput ();

	}
	void GiveDirectionToClue () {

		Directions dir = NavigationManager.Instance.getDirectionToPoint (ClueManager.Instance.GetNextClueIslandPos);
		string directionPhrase = NavigationManager.Instance.getDirName (dir);
//		if ( Random.value < 0.6f ) {
//			directionPhrase = "J'en ai aucune idée";
//		}

		if ( Crews.enemyCrew.CrewMembers.Count == 0 ) {
			DialogueManager.Instance.ShowNarrator (directionPhrase);
		} else {
			Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
			DialogueManager.Instance.SetDialogue (directionPhrase, Crews.enemyCrew.captain.Icon.GetTransform);
		}

		StoryReader.Instance.WaitForInput ();
	}

	string getFormula () {
		
		int clueIndex = ClueManager.Instance.ClueIndex;

		string clue = ClueManager.Instance.Clues[clueIndex];

		bool clueAlreadyFound = false;

		int a = 0;

		foreach ( int i in ClueManager.Instance.ClueIslands ) {

			if ( i == MapManager.Instance.IslandID ) {
				Debug.Log ("already found clue in island");
				clue = ClueManager.Instance.Clues [a];
				clueIndex = a;
				clueAlreadyFound = true;
			}

			++a;

		}

		if ( clueAlreadyFound == false ) {
			Debug.Log ("first time gave clue");
			ClueManager.Instance.ClueIndex += 1;
		}

		ClueManager.Instance.ClueIslands [clueIndex] = MapManager.Instance.IslandID;

		return clue;
	}
	#endregion

	public string[] FunctionNames {
		get {
			return functionNames;
		}
		set {
			functionNames = value;
		}
	}
}
