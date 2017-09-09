using UnityEngine;
using System.Collections;

public class Crews : MonoBehaviour {

	public static Crews Instance;

	public enum Side {
		Player,
		Enemy,
	}

	private Side[] sides = new Side[2] {Side.Player,Side.Enemy};
	public Side[] Sides {get {return sides;}}

	public enum PlacingType {
		Map,
		Combat,
		SoloCombat,
		Discussion,
		Hidden
	}

	public static CrewManager[] crews = new CrewManager[2];

	public int startMemberAmount = 1;

	void Awake () {
		Instance = this;
	}

	public void Init () {
		crews [0] = GetComponentsInChildren<CrewManager> () [0];
		crews [1] = GetComponentsInChildren<CrewManager> () [1];

		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.NewCrew:
			CreateNewCrew ();
			break;
		case FunctionType.AddMember:
			AddMemberToCrew ();
			break;
		case FunctionType.RemoveMember:
			RemoveMemberFromCrew ();
			break;
		case FunctionType.AddHealth:
			AddHealth ();
			break;
		case FunctionType.RemoveHealth:
			RemoveHealth();
			break;
		case FunctionType.HideOther:
			enemyCrew.UpdateCrew (PlacingType.Hidden);
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();
			break;
		}
	}

	#region get crews
	public static CrewManager getCrew ( Crews.Side targetSide ) {
		return crews [(int)targetSide];
	}

	public static CrewManager playerCrew {
		get {
			return crews[0];
		}
	}

	public static CrewManager enemyCrew {
		get {
			return crews[1];
		}
	}
	#endregion

	#region save / load crews
	public void SavePlayerCrew () {
		SaveManager.Instance.CurrentData.playerCrew = playerCrew.ManagedCrew;
	}
	public void RandomizePlayerCrew () {
		CrewParams crewParams = new CrewParams ();
		crewParams.amount = startMemberAmount;
		crewParams.overideGenre = false;
		crewParams.male = false;
		crewParams.level = 1;


		Crew playerCrew = new Crew (crewParams,0,0);
		crews [0].setCrew (playerCrew);
		crews [0].UpdateCrew (PlacingType.Map);
	}
	public void LoadPlayerCrew () {
		playerCrew.ManagedCrew = SaveManager.Instance.CurrentData.playerCrew;

		crews [0].setCrew (playerCrew.ManagedCrew);
		crews [0].UpdateCrew (PlacingType.Map);
	}
	#endregion

	#region crew tools
	public void CreateNewCrew () {
		
		StoryReader.Instance.NextCell ();

		Crew islandCrew = Crews.Instance.GetCrewFromCurrentCell ();

		// set decal
		if (islandCrew.MemberIDs.Count == 0) {
			StoryReader.Instance.SetDecal (1);
		} else {

			Crews.enemyCrew.setCrew (islandCrew);

			if (islandCrew.hostile) {
				DialogueManager.Instance.SetDialogueTimed ("Le revoilà !", Crews.enemyCrew.captain);
				StoryReader.Instance.SetDecal (2);
			}

			Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
		}

		StoryReader.Instance.Wait (Crews.playerCrew.captain.Icon.MoveDuration);
	}

	public Crew GetCrewFromCurrentCell () {

		int row = StoryReader.Instance.Decal;
		int col = StoryReader.Instance.Index;

		var tmp = StoryReader.Instance.CurrentStoryHandler.GetCrew (row, col);

		if (tmp == null) {

			CrewParams crewParams = GetCrewFromText (StoryFunctions.Instance.CellParams);

			Crew newCrew = new Crew (crewParams, row, col);

			StoryReader.Instance.CurrentStoryHandler.SetCrew (newCrew);

			return newCrew;

		}


		return tmp;

	}

	public CrewParams GetCrewFromText (string text) {

		CrewParams crewParams = new CrewParams ();

		string[] parms = text.Split ('/');

			// crew amount
		if ( parms.Length > 0 ) {

			int parmAmount = 0;
			bool parsable = int.TryParse(parms[0],out parmAmount);

			crewParams.amount = parmAmount;
		}

			// genre
		if ( parms.Length > 1 ) {
			
			crewParams.overideGenre = true;

			if ( parms[1][0] == 'M') 
				crewParams.male = true;
			else if (parms[1][0] == 'F' )
				crewParams.male = false;
			else
				crewParams.male = Random.value > 0.5f;

		}

		if ( parms.Length > 2 ) {

			crewParams.level = int.Parse ( parms[2] );

		}

		return crewParams;

	}

	public void AddMemberToCrew () {
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

	public void RemoveMemberFromCrew () {
		int removeIndex = Random.Range (0,Crews.playerCrew.CrewMembers.Count);
		CrewMember memberToRemove = Crews.playerCrew.CrewMembers [removeIndex];

		memberToRemove.Kill ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (0.5f);
	}
	#endregion

	#region health
	private void AddHealth () {

		string cellParams = StoryFunctions.Instance.CellParams;
		int health = int.Parse ( cellParams );
		Crews.getCrew (Crews.Side.Player).captain.Health += health;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	private void RemoveHealth () {
		string cellParams = StoryFunctions.Instance.CellParams;
		int health = int.Parse ( cellParams );
		Crews.getCrew (Crews.Side.Player).captain.Health -= health;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	#endregion
}
