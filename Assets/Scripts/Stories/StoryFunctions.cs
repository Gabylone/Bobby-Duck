using UnityEngine;
using System.Collections;

public class StoryFunctions : MonoBehaviour {

	public static StoryFunctions Instance;

	[SerializeField]
	private TextAsset functionData;

	string cellParams = "";

	private string[] functionNames;

	void Awake () {
		Instance = this;

		LoadFunctions ();
	}

	void LoadFunctions () {
		string[] rows = functionData.text.Split ( '\n' );

		functionNames = new string[rows.Length-1];

		for (int row = 0; row < functionNames.Length; ++row ) {

			functionNames [row] = rows [row].Split (';') [0];

		}
//
//		foreach ( string n in functionNames )
//			Debug.Log ("found function : " + n);
	}

	public void Read ( string content ) {



		foreach ( string functionName in functionNames ) {

			if ( content.Contains (functionName) ){

				cellParams = content.Remove (0, functionName.Length);

				SendMessage (functionName);
				return;

			}

		}

		Debug.LogError ("cell returns no function : " + content);

	}

	#region random
	void randomPercent () {

		float chance = float.Parse ( cellParams );
		Debug.Log (chance.ToString() );

		if (Random.value * 100f < chance) {
			StoryReader.Instance.NextCell ();
		} else {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.SetDecal (1);
		}

		StoryReader.Instance.UpdateStory ();

	}
	void randomRange () {

		int range = int.Parse (cellParams);
		int random = Random.Range (0, range);

		StoryReader.Instance.NextCell ();

		StoryReader.Instance.SetDecal (random );

		StoryReader.Instance.UpdateStory ();


	}
	#endregion

	#region character & crew
	void randomCharacter() {
		Crews.enemyCrew.CreateRandomMember ();

		StoryReader.Instance.Wait ( Crews.playerCrew.captain.Icon.MoveDuration );
	}

	void randomCrew () {
		Crews.enemyCrew.CreateRandomCrew ();
		Crews.enemyCrew.UpdateCrew (Crews.PlacingType.Combat);

		if (Crews.enemyCrew.captain.Icon.CurrentPlacingType != Crews.PlacingType.Discussion) {
			Crews.enemyCrew.UpdateCrew (Crews.PlacingType.Combat);
			Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
		}

		StoryReader.Instance.Wait (Crews.playerCrew.captain.Icon.MoveDuration);

	}
	void hideOther () {
		Crews.enemyCrew.Hide ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void deleteOther () {
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
			Crews.enemyCrew.CreateRandomCrew ();
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

	void setChoices () {

		// get amount
		int amount = int.Parse (cellParams);

		// get bubble content
		StoryReader.Instance.NextCell ();

		string[] choices = new string[amount];

		int tmpDecal = StoryReader.Instance.Decal;
		int a = amount;

		while ( a > 0 ) {

			if ( StoryLoader.Instance.ReadDecal (tmpDecal).Length > 0 ) {
				choices [amount-a] = StoryLoader.Instance.ReadDecal (tmpDecal);

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
	void launchCombat () {
		CombatManager.Instance.StartCombat ();
	}
	void leave () {
		IslandManager.Instance.Leave ();
	}
	void conclude () {
		Debug.LogError ("a CODER : CONCLUDE");
		IslandManager.Instance.Leave ();
	}
	#endregion

	#region gold
	void checkGold () {
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
	void removeGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.RemoveGold (amount);
		StoryReader.Instance.Wait ( 1f );
	}
	void addGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.AddGold (amount);
		StoryReader.Instance.Wait ( 1f );
	}
	#endregion

	#region loot
	void lootAll () {
		if ( MapManager.Instance.CurrentIslandLoot == null ) {
			MapManager.Instance.CurrentIslandLoot = new Loot ();
			MapManager.Instance.CurrentIslandLoot.Randomize (ItemLoader.allCategories);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartLooting ();
	}
	void lootFood () {
		if ( MapManager.Instance.CurrentIslandLoot == null ) {
			MapManager.Instance.CurrentIslandLoot = new Loot ();
			MapManager.Instance.CurrentIslandLoot.Randomize (ItemCategory.Provisions);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartLooting ();
	}

	void lootWeapons() {
		if ( MapManager.Instance.CurrentIslandLoot == null ) {
			MapManager.Instance.CurrentIslandLoot = new Loot ();
			MapManager.Instance.CurrentIslandLoot.Randomize (ItemCategory.Provisions);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartLooting ();

	}

	void lootClothes() {

		if ( MapManager.Instance.CurrentIslandLoot.getLoot.Length == 0 ) {
			ItemCategory[] cats = new ItemCategory[2] {ItemCategory.Clothes, ItemCategory.Shoes};
			MapManager.Instance.CurrentIslandLoot.Randomize (cats);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartLooting ();
	}

	void lootMisc() {

		if ( MapManager.Instance.CurrentIslandLoot.getLoot.Length == 0 ) {
			MapManager.Instance.CurrentIslandLoot.Randomize (ItemCategory.Mics);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartLooting ();
	}
	#endregion

	#region trade
	void tradeAll () {
		if ( MapManager.Instance.CurrentIslandLoot == null ) {
			MapManager.Instance.CurrentIslandLoot = new Loot ();
			MapManager.Instance.CurrentIslandLoot.Randomize (ItemLoader.allCategories);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartTrade ();
	}
	void tradeFood () {
		if ( MapManager.Instance.CurrentIslandLoot == null ) {
			MapManager.Instance.CurrentIslandLoot = new Loot ();
			MapManager.Instance.CurrentIslandLoot.Randomize (ItemCategory.Provisions);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartTrade ();
	}

	void tradeWeapons() {
		if ( MapManager.Instance.CurrentIslandLoot == null ) {
			MapManager.Instance.CurrentIslandLoot = new Loot ();
			MapManager.Instance.CurrentIslandLoot.Randomize (ItemCategory.Provisions);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartTrade ();
	}

	void tradeClothes() {
		
		if ( MapManager.Instance.CurrentIslandLoot.getLoot.Length == 0 ) {
			ItemCategory[] cats = new ItemCategory[2] {ItemCategory.Clothes, ItemCategory.Shoes};
			MapManager.Instance.CurrentIslandLoot.Randomize (cats);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartTrade ();
	}

	void tradeMisc() {

		if ( MapManager.Instance.CurrentIslandLoot.getLoot.Length == 0 ) {
			MapManager.Instance.CurrentIslandLoot.Randomize (ItemCategory.Mics);
		}

		LootManager.Instance.setLoot ( Crews.Side.Enemy, MapManager.Instance.CurrentIslandLoot);
		OtherLoot.Instance.StartTrade ();
	}
	#endregion

	#region story navigation
	void goTo () {

		string[] coords = cellParams.Split ('/');

		StoryReader.Instance.Decal = int.Parse (coords [0])-1;
		StoryReader.Instance.Index = int.Parse (coords [1])-1;

		StoryReader.Instance.UpdateStory ();

	}
	#endregion

	#region weather 
	void setWeatherNight () {
		NavigationManager.Instance.IsNight = true;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void setWeatherDay() {
		NavigationManager.Instance.IsNight = false;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void setWeatherRain () {
		NavigationManager.Instance.Raining = true;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void switchWeather () {
		NavigationManager.Instance.IsNight = !NavigationManager.Instance.IsNight;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void nextDay () {
		NavigationManager.Instance.Move (Directions.None);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void checkDay () {

		StoryReader.Instance.NextCell ();

		if (NavigationManager.Instance.IsNight)
			StoryReader.Instance.SetDecal (1);

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region clues
	void checkClue () {
		
	}
	#endregion
}
