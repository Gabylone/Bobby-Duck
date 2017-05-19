using UnityEngine;
using System.Collections;

public class PlayerBoatInfo : BoatInfo {

	public static PlayerBoatInfo Instance;

	private int shipRange = 1;

	public bool isInNoMansSea = false;
	public bool hasBeenWarned = false;

	public override void Init ()
	{
		Instance = this;

		base.Init ();

		NavigationManager.Instance.EnterNewChunk += UpdatePosition;

		PosX = MapData.Instance.homeIslandXPos;
		PosY = MapData.Instance.homeIslandYPos;

	}

	public override void UpdatePosition ()
	{
		base.UpdatePosition ();
	}

	public void CheckForNoMansSea () {
		bool isInNoMansSea =
			PosY > (MapGenerator.Instance.MapScale / 2) - (MapGenerator.Instance.NoManSeaScale/2)
			&& PosY < (MapGenerator.Instance.MapScale / 2) + (MapGenerator.Instance.NoManSeaScale/2);

		if (isInNoMansSea ) {

			if (!hasBeenWarned) {
				DialogueManager.Instance.ShowNarrator ("Le bateau entre dans la Grande Mer... Pas de terres en vue à des lieus d'ici. Mieux vaut être bien préparé, la traversée sera longue.");
				hasBeenWarned = true;
			}

		} else {
			if (hasBeenWarned) {
				DialogueManager.Instance.ShowNarrator ("Le bateau quitte les eaux de la Grande Mer... Déjà les premières îles apparaissent à l'horizon. Ouf...");
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
}
