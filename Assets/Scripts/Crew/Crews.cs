using UnityEngine;
using System.Collections;

public class Crews : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		crews[0] 	= GetComponentsInChildren<CrewManager> () [0];	
		crews[1] 	= GetComponentsInChildren<CrewManager> () [1];

		crews[0].CreateRandomCrew ();
		crews[0].UpdateCrew (PlacingType.Map);
	}
	
	// Update is called once per frame
	void Update () {
	
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
