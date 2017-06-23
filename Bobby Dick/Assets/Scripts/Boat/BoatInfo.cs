using UnityEngine;

[System.Serializable]
public class BoatInfo {

	public string Name = "bateau";

	private int previousPosX = 0;
	private int previousPosY = 0;

	public int posX = 0;
	public int posY = 0;

	public Directions currentDirection;

	public BoatInfo () {
		
	}

	public virtual void Randomize () {
		//
	}

	public virtual void UpdatePosition () {

	}

	public virtual int PosX {
		get {
			return posX;
		}
		set {

			previousPosX = posX;
			posX = Mathf.Clamp (value , 0, MapGenerator.Instance.MapScale-1);

		}
	}

	public virtual int PosY {
		get {
			return posY;
		}
		set {
			
			previousPosY = posY;
			posY = Mathf.Clamp (value , 0, MapGenerator.Instance.MapScale-1);
		}
	}

	public int PreviousPosX {
		get {
			return previousPosX;
		}
	}

	public int PreviousPosY {
		get {
			return previousPosY;
		}
	}
}
