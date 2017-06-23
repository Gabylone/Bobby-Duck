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

		PosX = MapData.Instance.homeIslandXPos;
		PosY = MapData.Instance.homeIslandYPos;
	}

	public override void UpdatePosition ()
	{
		base.UpdatePosition ();

		currentDirection = NavigationManager.Instance.CurrentDirection;
		PosX += (int)NavigationManager.Instance.getDir (currentDirection).x;
		PosY += (int)NavigationManager.Instance.getDir (currentDirection).y;
	}

	public void CheckForNoMansSea () {
		bool isInNoMansSea =
			PosY > (MapGenerator.Instance.MapScale / 2) - (MapGenerator.Instance.NoManSeaScale/2)
			&& PosY < (MapGenerator.Instance.MapScale / 2) + (MapGenerator.Instance.NoManSeaScale/2);

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

			if (WeatherManager.Instance.Raining)
				range--;
			if (WeatherManager.Instance.IsNight)
				range--;

			return Mathf.Clamp (range,0,10);

		}
		set {
			shipRange = value;
		}
	}

	public override int PosX {
		get {
			return base.PosX;
		}
		set {
			
			if (value < 0 || value > MapGenerator.Instance.MapScale - 1) {
				DialogueManager.Instance.ShowNarratorTimed("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
				Debug.Log ("exited map ?");
			}

			base.PosX = value;
		}
	}

	public override int PosY {
		get {
			return base.PosY;
		}
		set {


			if ( value < 0 || value > MapGenerator.Instance.MapScale-1) {
				DialogueManager.Instance.ShowNarratorTimed ("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
				Debug.Log ("exited map ?");
			}

			base.PosY = value;
		}
	}
}
