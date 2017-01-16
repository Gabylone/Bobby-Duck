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
		crews [0] 	= GetComponentsInChildren<CrewManager> () [0];	
		crews [1] 	= GetComponentsInChildren<CrewManager> () [1];

		CrewParams crewParams = new CrewParams (5, false, false);
		Crew playerCrew = new Crew (crewParams,0,0);
		crews [0].setCrew (playerCrew);
		crews [0].UpdateCrew (PlacingType.Map);
	}

	public static CrewManager getCrew ( Crews.Side attackingCrew ) {
		return crews [(int)attackingCrew];
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

	public void SavePlayerCrew () {
		SaveManager.Instance.CurrentData.playerCrew = playerCrew.ManagedCrew;
	}

	public void LoadPlayerCrew () {
		playerCrew.ManagedCrew = SaveManager.Instance.CurrentData.playerCrew;

		crews [0].setCrew (playerCrew.ManagedCrew);
		crews [0].UpdateCrew (PlacingType.Map);
	}
}
