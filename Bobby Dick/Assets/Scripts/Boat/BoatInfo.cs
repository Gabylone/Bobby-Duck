using UnityEngine;

[System.Serializable]
public class BoatInfo {

	private int previousPosX = 0;
	private int previousPosY = 0;

	private int posX = 0;
	private int posY = 0;

	public Directions currentDirection;

	public BoatInfo () {
		Init ();
	}

	public virtual void Init () {
		
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
