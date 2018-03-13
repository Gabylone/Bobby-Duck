using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerBoatInfo : BoatInfo {
	
	public int shipRange = 1;
	public int crewCapacity = 2;
	public int cargoLevel = 1;
	public int level = 1;

	public int GetCargoCapacity () {
		return 100 + ( (cargoLevel-1) * 50);
	}

	// seulement lors d'une novelle partie
	public override void Randomize ()
	{
		base.Randomize ();

		coords = SaveManager.Instance.GameData.homeCoords;
//		coords = Coords.Zero;

	}

	public override Coords coords {
		get {
			return base.coords;
		}
		set {
			base.coords = value;

			if (value.x < 0 || value.x > MapGenerator.Instance.MapScale - 1 || value.y < 0 || value.y > MapGenerator.Instance.MapScale - 1) {
				Narrator.Instance.ShowNarratorTimed("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
			}
		}
	}
}
