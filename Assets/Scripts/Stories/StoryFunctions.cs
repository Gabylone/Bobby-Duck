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

		if (content.Length == 0) {
			Debug.LogError ("cell is empty");
			Leave ();
			return;
		}

		if ( content[0] == '[' ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();
			return;
		}

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

		float value = Random.value * 100;

		int randomDecal = value < chance ? 0 : 1;

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
	void RandomRedoPercent () {

		float chance = float.Parse ( cellParams );

		float value = Random.value * 100;

		int decal = value < chance ? 0 : 1;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();

	}
	void RandomRedoRange () {

		int range = int.Parse (cellParams);
		int randomDecal = Random.Range (0, range);

		StoryReader.Instance.NextCell ();

		StoryReader.Instance.SetDecal (randomDecal);

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

	void NewCrew () {

		StoryReader.Instance.NextCell ();

		int l = Crews.playerCrew.CrewMembers.Count;

		int amount = 0;
		if ( cellParams.Length > 0 ) {
			amount = int.Parse(cellParams);
		} else {
			amount = Random.Range ( l-1 , l+2 );
		}

		Crew islandCrew = GetCrew (amount);

		if (islandCrew.MemberIDs.Count == 0) {
			
			StoryReader.Instance.SetDecal (1);

		} else {

			Crews.enemyCrew.setCrew (islandCrew);

			if (islandCrew.hostile) {
				DialogueManager.Instance.SetDialogue ("Le revoilà !", Crews.enemyCrew.captain);
				StoryReader.Instance.SetDecal (2);
			} else {
				Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
			}

		}

		StoryReader.Instance.Wait (Crews.playerCrew.captain.Icon.MoveDuration);

	}

	void AddMember () {

		if (Crews.playerCrew.CrewMembers.Count == Crews.playerCrew.MemberCapacity) {

			string phrase = "Oh non, le bateau est trop petit";
			DialogueManager.Instance.SetDialogue (phrase, Crews.enemyCrew.captain);

			StoryReader.Instance.WaitForInput ();

		} else {

			CrewMember targetMember = Crews.enemyCrew.captain;

			CrewCreator.Instance.TargetSide = Crews.Side.Player;
			CrewMember newMember = CrewCreator.Instance.NewMember (Crews.enemyCrew.captain.MemberID);
			Crews.playerCrew.AddMember (newMember);
			Crews.enemyCrew.RemoveMember (targetMember);

			newMember.Icon.MoveToPoint (Crews.PlacingType.Map);

			StoryReader.Instance.NextCell ();
			StoryReader.Instance.Wait (0.5f);
		
		}

	}
	void RemoveMember () {

		int removeIndex = Random.Range (0,Crews.playerCrew.CrewMembers.Count);
		CrewMember memberToRemove = Crews.playerCrew.CrewMembers [removeIndex];

		Crews.playerCrew.RemoveMember (memberToRemove);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (0.5f);

	}
	#endregion

	#region hide & show
	void HideAll () {
		Crews.enemyCrew.Hide ();
		Crews.playerCrew.Hide ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	void ShowAll () {
		Crews.playerCrew.ShowCrew ();
		Crews.enemyCrew.ShowCrew ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	void HidePlayer() {
		Crews.playerCrew.Hide ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	void ShowPlayer () {
		Crews.playerCrew.ShowCrew ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	void HideOther () {
		Crews.enemyCrew.Hide ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	void ShowOther() {
		Crews.enemyCrew.ShowCrew ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	void DeleteOther () {
		Crews.enemyCrew.DeleteCrew ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	#endregion

	#region boatUpgrades
	void BoatUpgrades () {
		BoatUpgradeManager.Instance.ShowUpgradeMenu ();
		BoatUpgradeManager.Instance.Trading = true;
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

		DialogueManager.Instance.SetDialogue (phrase, Crews.enemyCrew.captain);

		StoryReader.Instance.WaitForInput ();

	}

	void PlayerSpeak () {
		
		string phrase = cellParams.Remove (0,2);

		DialogueManager.Instance.SetDialogue (phrase, Crews.playerCrew.captain);

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

			if ( tmpDecal > 60 ) {
				Debug.LogError ("set choice reached limit");
				break;
			}

			if (a <= 0)
				break;
		}

		DiscussionManager.Instance.SetChoices (amount, choices);

	}

	void GiveTip ()  {

		string[] tips = new string[6] {

			"Un grand vide sépare le nord du sud",
			"Mieux vaut bien se préparer pour aller du nord au sud !",
			"Les pirates se déplacent librement sur les mers",
			"Une bonne longue vue règle les problemes de vision la nuit",
			"Une bonne longue vue règle les problemes de vision les jours de pluie",
			"C'est en discutant avec les gens que vous saurez où chercher le trésor.",

		};

		DialogueManager.Instance.SetDialogue (tips[Random.Range (0,tips.Length)], Crews.enemyCrew.captain);

		StoryReader.Instance.WaitForInput ();

	}
	#endregion

	#region end
	void LaunchCombat () {
		Crews.enemyCrew.ManagedCrew.hostile = true;
		CombatManager.Instance.StartCombat ();
	}
	void Leave () {
		IslandManager.Instance.Leave ();
	}
	void Conclude () {
		IslandManager.Instance.Leave ();
	}
	#endregion

	#region gold
	void CheckGold () {
		int amount = int.Parse (cellParams);

		if (GoldManager.Instance.CheckGold (amount)) {
			StoryReader.Instance.NextCell ();
		} else {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.SetDecal (1);
		}

		StoryReader.Instance.UpdateStory ();
	}
	void RemoveGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.RemoveGold (amount);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait ( 1f );
	}
	void AddGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.AddGold (amount);

		StoryReader.Instance.NextCell ();
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

			categories [index] = getLootCategoryFromString(cellPart);

			++index;
		}

		return categories;
	}

	public ItemCategory getLootCategoryFromString ( string arg ) {

		switch (arg) {
		case "Food":
			return ItemCategory.Provisions;
			break;
		case "Weapons":
			return ItemCategory.Weapon;
			break;
		case "Clothes":
			return ItemCategory.Clothes;
			break;
//		case "Shoes":
//			return ItemCategory.Shoes;
//			break;
		case "Misc":
			return ItemCategory.Misc;
			break;
		}

		Debug.LogError ("getLootCategoryFromString : couldn't find category in : " + arg);

		return ItemCategory.Misc;

	}

	void RemoveFromInventory () {
		Debug.Log ("remove something from inventory");

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (0.5f);
	}
	void AddToInventory () {

		string itemName = cellParams.Split ('<')[1];

		Debug.Log ("Found name : " + itemName);

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);

		Item item = System.Array.Find (ItemLoader.Instance.getItems (targetCat), x => x.name == itemName);

		Debug.Log ( "try to add ; " + item.name + " to inventory" );

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (0.5f);
		//
	}
	#endregion

	#region story navigation
	void GoTo () {

		string[] coords = cellParams.Split ('/');

		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		int a = 0;

		char targetChar = coords [0] [0];

		foreach (char c in alphabet) {
			if (c == targetChar) {
				break;
			}
			a++;
		}

		int newIndex = int.Parse (coords [1]) - 3;

		StoryReader.Instance.Decal = a;
		StoryReader.Instance.Index = newIndex;

		StoryReader.Instance.UpdateStory ();

	}

	void Mark () {

		string markName = cellParams.Remove (0, 2);

		Story.Mark mark = StoryReader.Instance.CurrentStory.marks.Find ( x => x.name == markName);

		StoryReader.Instance.Decal = mark.x;
		StoryReader.Instance.Index = mark.y;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();


	}

	void CheckFirstVisit () {

		StoryReader.Instance.NextCell ();

		if ( MapManager.Instance.CurrentIsland.visited == true) {
			StoryReader.Instance.SetDecal (1);
		}

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region weather
	void SetWeather() {
		StartCoroutine (SetWeatherCoroutine (cellParams));
	}
	IEnumerator SetWeatherCoroutine (string weather) {

		Transitions.Instance.FadeScreen ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		switch ( weather ) {
		case "Day":
			WeatherManager.Instance.IsNight = false;
			WeatherManager.Instance.Raining = false;
			break;
		case "Night":
			WeatherManager.Instance.IsNight = true;
			WeatherManager.Instance.Raining = false;
			break;
		case "Rain":
			WeatherManager.Instance.Raining = true;
			break;
		}

		NavigationManager.Instance.UpdateTime ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	void Fade () {

		Transitions.Instance.FadeScreen ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (Transitions.Instance.ActionTransition.Duration * 2);

	}

	void CheckDay () {

		StoryReader.Instance.NextCell ();

		if (WeatherManager.Instance.IsNight)
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
			DialogueManager.Instance.SetDialogue (getFormula(), Crews.enemyCrew.captain);

		}

		StoryReader.Instance.WaitForInput ();

	}
	void GiveDirectionToClue () {

		Directions dir = NavigationManager.Instance.getDirectionToPoint (ClueManager.Instance.GetNextClueIslandPos);
		string directionPhrase = NavigationManager.Instance.getDirName (dir);

		if ( Crews.enemyCrew.CrewMembers.Count == 0 ) {
			DialogueManager.Instance.SetDialogue (directionPhrase, Crews.playerCrew.captain);
//			DialogueManager.Instance.ShowNarrator (directionPhrase);
		} else {
			Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
			DialogueManager.Instance.SetDialogue (directionPhrase, Crews.enemyCrew.captain);
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


//STORIES :
//- rajouter forêt
//- rajouter juste indice
//
//- petite grotte argent.
//- petite ferme randol loot
//
//- bandits seulement nuit
//
//- librairie qui parle de bobdy
