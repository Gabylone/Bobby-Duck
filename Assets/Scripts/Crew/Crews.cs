using UnityEngine;
using System.Collections;

public class Crews : MonoBehaviour {

	public static Crews Instance;

	public enum Side {
		Player,
		Enemy,
	}

	public enum PlacingType {
		Map,
		Combat,
		SoloCombat,
		Discussion,
		Hidden
	}

	public static CrewManager[] crews = new CrewManager[2];

	void Awake () {
		Instance = this;
		Init ();
	}

	private void Init () {

		crews [0] = GetComponentsInChildren<CrewManager> () [0];
		crews [1] = GetComponentsInChildren<CrewManager> () [1];

		CrewParams crewParams = new CrewParams (1, false, false);
		Crew playerCrew = new Crew (crewParams,0,0);
		crews [0].setCrew (playerCrew);
		crews [0].UpdateCrew (PlacingType.Map);

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
				DialogueManager.Instance.SetDialogue ("Le revoilà !", Crews.enemyCrew.captain);
				StoryReader.Instance.SetDecal (2);
			} else {
				Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
				Crews.enemyCrew.captain.Icon.ShowBody ();
			}

		}

		StoryReader.Instance.Wait (Crews.playerCrew.captain.Icon.MoveDuration);
	}

	public Crew GetCrewFromCurrentCell () {

		int row = StoryReader.Instance.Decal;
		int col = StoryReader.Instance.Index;

		var tmp = MapManager.Instance.CurrentIsland.Crews.Find (x => x.col == col && x.row == row);

		if (tmp == null) {

			CrewParams crewParams = GetCrewFromText (StoryFunctions.Instance.CellParams);

			Crew newCrew = new Crew (crewParams, row, col);

			MapManager.Instance.CurrentIsland.Crews.Add (newCrew);

			return newCrew;

		}

		return tmp;

	}

	public CrewParams GetCrewFromText (string text) {

		int l = Crews.playerCrew.CrewMembers.Count;

		CrewParams crewParams = new CrewParams ();

		if ( text.Length > 0 ) {

			if (text.Contains ("/")) {

				string[] parms = text.Split ('/');

				crewParams.amount = int.Parse (parms[0]);
				crewParams.overideGenre = true;
				crewParams.male = parms[1][0] == 'M';

			} else {
				crewParams.amount = int.Parse (text);

			}

		} else {
			crewParams.amount = Random.Range ( l-1 , l+2 );
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
}
