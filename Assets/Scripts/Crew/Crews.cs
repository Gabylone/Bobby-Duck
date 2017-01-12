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
		crews [0] 	= GetComponentsInChildren<CrewManager> () [0];	
		crews [1] 	= GetComponentsInChildren<CrewManager> () [1];

		CrewParams crewParams = new CrewParams (1, false, false);
		Crew playerCrew = new Crew (crewParams,0,0);
		crews [0].setCrew (playerCrew);
		crews [0].UpdateCrew (PlacingType.Map);

		SaveManager.Instance.CurrentData.guyName = Crews.playerCrew.captain.MemberName;
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
}
