using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerBoatInfo : BoatInfo {
	
	public int shipRange = 1;

	public bool isInNoMansSea = false;
	public bool hasBeenWarned = false;

	public override void Init ()
	{
		base.Init ();

		coords = MapData.Instance.homeIslandCoords;
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;;
	}

	void HandleChunkEvent ()
	{
		UpdatePosition ();
	}

	public override void UpdatePosition ()
	{
		base.UpdatePosition ();
	}

	public void CheckForNoMansSea () {
		
		bool isInNoMansSea =
			coords.y > (MapGenerator.Instance.MapScale / 2) - (MapGenerator.Instance.NoManSeaScale/2)
			&& coords.y < (MapGenerator.Instance.MapScale / 2) + (MapGenerator.Instance.NoManSeaScale/2);

		if (isInNoMansSea ) {

			if (!hasBeenWarned) {
				Narrator.Instance.ShowNarratorTimed ("Le bateau entre dans la Grande Mer... Pas de terres en vue à des lieus d'ici. Mieux vaut être bien préparé, la traversée sera longue.");
				hasBeenWarned = true;
			}

		} else {
			if (hasBeenWarned) {
				Narrator.Instance.ShowNarratorTimed ("Le bateau quitte les eaux de la Grande Mer... Déjà les premières îles apparaissent à l'horizon. Ouf...");
			}

			hasBeenWarned = false;

		}
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
