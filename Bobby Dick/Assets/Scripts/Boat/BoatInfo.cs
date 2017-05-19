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

	public void SavePosition () {
		SaveManager.Instance.CurrentData.boatPosX = PosX;
		SaveManager.Instance.CurrentData.boatPosY = PosY;
	}

	public void LoadPosition () {
		PosX = SaveManager.Instance.CurrentData.boatPosX;
		PosY = SaveManager.Instance.CurrentData.boatPosY;
	}

	public int PosX {
		get {
			return posX;
		}
		set {

			if (value <= 0 || value >= MapGenerator.Instance.MapScale - 1) {
				DialogueManager.Instance.ShowNarrator ("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
			} else {
				previousPosY = posY;
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
				DialogueManager.Instance.ShowNarrator ("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
			} else {
				previousPosX = posX;
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
