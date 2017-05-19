using UnityEngine;

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

	public int PosX {
		get {
			return posX;
		}
		set {

			if (value <= 0 || value >= MapGenerator.Instance.MapScale - 1) {
				Debug.Log ("il est en dehors de la zone en " + value);
				DialogueManager.Instance.ShowNarrator ("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
			} else {
				previousPosX = posX;
			}

			posX = Mathf.Clamp (value , 0, MapGenerator.Instance.MapScale-1);

		}
	}

	public int PosY {
		get {
			return posY;
		}
		set {

			if ( value <= 0 || value >= MapGenerator.Instance.MapScale-1) {
				Debug.Log ("il est en dehors de la zone en " + value);
				DialogueManager.Instance.ShowNarrator ("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
			} else {
				previousPosY = posY;
			}

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
