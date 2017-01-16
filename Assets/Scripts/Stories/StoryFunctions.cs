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
			Debug.LogError ("cell is empty at index : " + StoryReader.Instance.Index + " and decal " + StoryReader.Instance.Decal);
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
	Crew GetCrew (CrewParams crewParams) {
		
		int row = StoryReader.Instance.Decal;
		int col = StoryReader.Instance.Index;

		var tmp = MapManager.Instance.CurrentIsland.Crews.Find (x => x.col == col && x.row == row);

		if (tmp == null) {

			Crew newCrew = new Crew (crewParams, row, col);

			MapManager.Instance.CurrentIsland.Crews.Add (newCrew);

			return newCrew;

		}

		return tmp;
	}

	void NewCrew () {

		StoryReader.Instance.NextCell ();

		int l = Crews.playerCrew.CrewMembers.Count;

		CrewParams crewParams = new CrewParams ();

		if ( cellParams.Length > 0 ) {

			if (cellParams.Contains ("/")) {
			
				string[] parms = cellParams.Split ('/');

				crewParams.amount = int.Parse (parms[0]);
				crewParams.overideGenre = true;
				crewParams.male = parms[1][0] == 'M';

			} else {
				crewParams.amount = int.Parse (cellParams);

			}

		} else {
			crewParams.amount = Random.Range ( l-1 , l+2 );
		}

		Crew islandCrew = GetCrew (crewParams);

		if (islandCrew.MemberIDs.Count == 0) {
			
			StoryReader.Instance.SetDecal (1);

		} else {

			Crews.enemyCrew.setCrew (islandCrew);

			if (islandCrew.hostile) {
				DialogueManager.Instance.SetDialogue ("Le revoilà !", Crews.enemyCrew.captain);
				StoryReader.Instance.SetDecal (2);
			} else {
				Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
				Crews.enemyCrew.captain.Icon.ShowBody ();
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

		DiscussionManager.Instance.ResetColors ();

		// get amount
		int amount = int.Parse (cellParams);

		// get bubble content
		StoryReader.Instance.NextCell ();

		string[] choices = new string[amount];

		int tmpDecal = StoryReader.Instance.Decal;
		int a = amount;

		int index = 0;
		while ( a > 0 ) {

			if ( StoryLoader.Instance.ReadDecal (tmpDecal).Length > 0 ) {

				string choice = StoryLoader.Instance.ReadDecal (tmpDecal);

				choice = choice.Remove (0, 9);

				int i = 0;

				string[] stats = new string[] { "(str)", "(dex)", "(cha)", "(con)" };
				foreach ( string stat in stats ) {

					if ( choice.Contains ( stat ) ) {

						DiscussionManager.Instance.TaintChoice (index, i);

					}

					++i;

				}

				choices [amount - a] = choice;

				--a;
				++index;
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

		string[] tips = new string[10] {

			"Un grand vide sépare le nord du sud",
			"Mieux vaut bien se préparer pour aller du nord au sud !",
			"Les pirates se déplacent librement sur les mers",
			"Une bonne longue vue règle les problemes de vision la nuit",
			"Une bonne longue vue règle les problemes de vision les jours de pluie",
			"C'est en discutant avec les gens que vous saurez où chercher le trésor.",
			"le charme du capitaine est important, il permet de se sortir de situations coquasses",
			"La dextérité détermine si un membre attaque en premier, et ses chances d'esquiver.",
			"Vous êtes à l'étroit sur votre navire ? Aggrandissez le pont dans un hangar",
			"Aggrandissez le cargo dans un hangar.Vous pouvez porter plus de choses."

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
		GoldManager.Instance.GoldAmount -= amount;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait ( 0.5f );
	}
	void AddGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.GoldAmount += amount;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (0.5f);
	}
	#endregion

	#region trade & loot
	void Loot() {
		LootManager.Instance.setLoot ( Crews.Side.Enemy, LootManager.Instance.GetIslandLoot(getLootCategories()));
		OtherLoot.Instance.StartLooting ();
	}
	void Trade() {

		ItemLoader.Instance.Mult = 3;

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

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);
		StoryReader.Instance.NextCell ();

		if ( LootManager.Instance.PlayerLoot.getLoot[(int)targetCat].Length == 0 ) {
			
			StoryReader.Instance.SetDecal (1);

		} else {

			Item item = LootManager.Instance.PlayerLoot.getLoot [(int)targetCat] [0];

			LootManager.Instance.PlayerLoot.RemoveItem (item);

			Debug.Log ("removed item : " + item.name);

		}

		StoryReader.Instance.UpdateStory ();
	}
	void AddToInventory () {

		string itemName = cellParams.Split ('<')[1];

		itemName = itemName.Remove (itemName.Length - 6);

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);

		Item item = System.Array.Find (ItemLoader.Instance.getItems (targetCat), x => x.name == itemName);

		LootManager.Instance.PlayerLoot.AddItem (item);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
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
			DialogueManager.Instance.SetDialogue (formula, Crews.enemyCrew.captain);

		}

		StoryReader.Instance.WaitForInput ();

	}
	void GiveDirectionToClue () {

		Directions dir = NavigationManager.Instance.getDirectionToPoint (ClueManager.Instance.GetNextClueIslandPos);
		string directionPhrase = NavigationManager.Instance.getDirName (dir);

		if ( cellParams.Length == 0 ) {
			DialogueManager.Instance.SetDialogue (directionPhrase, Crews.enemyCrew.captain);
		} else {
			DialogueManager.Instance.SetDialogue (directionPhrase, Crews.playerCrew.captain);
		}

		StoryReader.Instance.WaitForInput ();
	}

	string getFormula () {
		
		int clueIndex = ClueManager.Instance.ClueIndex;

		string clue = "";

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
			clue = ClueManager.Instance.Clues[clueIndex];
			ClueManager.Instance.ClueIndex += 1;
		}

		ClueManager.Instance.ClueIslands [clueIndex] = MapManager.Instance.IslandID;

		return clue;
	}
	#endregion

	#region dice
	private void CheckStat () {

		StartCoroutine (CheckStat_Coroutine ());

	}

	IEnumerator CheckStat_Coroutine () {

		DiceManager.Instance.ThrowDirection = 1;

		switch ( cellParams ) {
		case "SRT":
			DiceManager.Instance.ThrowDice (DiceTypes.STR, Crews.playerCrew.captain.Strenght);
			break;
		case "DEX" :
			DiceManager.Instance.ThrowDice (DiceTypes.DEX, Crews.playerCrew.captain.Dexterity);
			break;
		case "CHA" :
			DiceManager.Instance.ThrowDice (DiceTypes.CHA, Crews.playerCrew.captain.Charisma);
			break;
		case "CON" :
			DiceManager.Instance.ThrowDice (DiceTypes.CON, Crews.playerCrew.captain.Constitution);
			break;
		}


		yield return new WaitForSeconds ( DiceManager.Instance.settlingDuration + DiceManager.Instance.ThrowDuration);

		int captainHighest = DiceManager.Instance.getHighestThrow;

		DiceManager.Instance.ThrowDirection = -1;

		switch ( cellParams ) {
		case "SRT":
			DiceManager.Instance.ThrowDice (DiceTypes.STR, Crews.enemyCrew.captain.Strenght);
			break;
		case "DEX" :
			DiceManager.Instance.ThrowDice (DiceTypes.DEX, Crews.enemyCrew.captain.Dexterity);
			break;
		case "CHA" :
			DiceManager.Instance.ThrowDice (DiceTypes.CHA, Crews.enemyCrew.captain.Charisma);
			break;
		case "CON" :
			DiceManager.Instance.ThrowDice (DiceTypes.CON, Crews.enemyCrew.captain.Constitution);
			break;
		}

		yield return new WaitForSeconds ( DiceManager.Instance.settlingDuration + DiceManager.Instance.ThrowDuration);

		int otherHighest = DiceManager.Instance.getHighestThrow;

		StoryReader.Instance.NextCell ();

		StoryReader.Instance.SetDecal (otherHighest > captainHighest ? 0 : 1);

		StoryReader.Instance.UpdateStory ();
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