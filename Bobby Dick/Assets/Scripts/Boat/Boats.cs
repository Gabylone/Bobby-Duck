using UnityEngine;
using System.Collections;

public class Boats : MonoBehaviour {

	public static Boats Instance;

	private PlayerBoatInfo playerBoatInfo;

	private BoatInfo[] boatInfos;

	[SerializeField]
	private Boat playerBoat;

	private int boatAmount = 100;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	public void Init () {

		playerBoatInfo = new PlayerBoatInfo ();
		playerBoat.BoatInfo = playerBoatInfo;

		boatInfos = new BoatInfo[boatAmount];
		for (int i = 0; i < boatInfos.Length; i++) {
			boatInfos [i] = new BoatInfo ();
		}
	}
}
