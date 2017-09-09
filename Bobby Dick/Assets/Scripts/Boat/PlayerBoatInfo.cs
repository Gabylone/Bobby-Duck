using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerBoatInfo : BoatInfo {

	private int shipRange = 1;

	public bool isInNoMansSea = false;
	public bool hasBeenWarned = false;

	public override void Randomize ()
	{
		base.Randomize ();

		CurrentCoords = MapData.Instance.homeIslandCoords;
//		CurrentCoords = MapData.Instance.treasureIslandCoords;
	}

	public override void UpdatePosition ()
	{
		base.UpdatePosition ();

		currentDirection = NavigationManager.Instance.CurrentDirection;
		CurrentCoords += NavigationManager.Instance.getNewCoords (currentDirection);
	}

	public void CheckForNoMansSea () {
		bool isInNoMansSea =
			CurrentCoords.y > (MapGenerator.Instance.MapScale / 2) - (MapGenerator.Instance.NoManSeaScale/2)
			&& CurrentCoords.y < (MapGenerator.Instance.MapScale / 2) + (MapGenerator.Instance.NoManSeaScale/2);

		if (isInNoMansSea ) {

			if (!hasBeenWarned) {
				DialogueManager.Instance.ShowNarratorTimed ("Le bateau entre dans la Grande Mer... Pas de terres en vue à des lieus d'ici. Mieux vaut être bien préparé, la traversée sera longue.");
				hasBeenWarned = true;
			}

		} else {
			if (hasBeenWarned) {
				DialogueManager.Instance.ShowNarratorTimed ("Le bateau quitte les eaux de la Grande Mer... Déjà les premières îles apparaissent à l'horizon. Ouf...");
			}

			hasBeenWarned = false;

		}
	}

	public int ShipRange {
		get {

			int range = shipRange;

			if (TimeManager.Instance.Raining)
				range--;
			if (TimeManager.Instance.IsNight)
				range--;

			return Mathf.Clamp (range,0,10);

		}
		set {
			shipRange = value;
		}
	}

	public override Coords CurrentCoords {
		get {
			return base.CurrentCoords;
		}
		set {
			base.CurrentCoords = value;

			if (value.x < 0 || value.x > MapGenerator.Instance.MapScale - 1 || value.y < 0 || value.y > MapGenerator.Instance.MapScale - 1) {
				DialogueManager.Instance.ShowNarratorTimed("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
			}
		}
	}

}
