using UnityEngine;

[System.Serializable]
public class BoatInfo {

	public string Name = "bateau";

	public Coords previousCoords;
	private Coords currentCoords;

	public Directions currentDirection;

	public BoatInfo () {
		
	}

	public virtual void Init () {
		//
	}

	public virtual void Randomize () {
		//
	}

	public virtual void UpdatePosition () {

	}

	public virtual Coords coords {
		get {
			return currentCoords;
		}
		set {

			previousCoords = currentCoords;

			currentCoords = value;

			currentCoords.x = Mathf.Clamp (value.x , 0, MapGenerator.Instance.MapScale-1);
			currentCoords.y = Mathf.Clamp (value.y , 0, MapGenerator.Instance.MapScale-1);
		}
	}

	public void Move ( Directions dir ) {
		currentDirection = dir;
		coords += NavigationManager.Instance.getNewCoords (currentDirection);
	}
}
