using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoatManager : MonoBehaviour {

	public static BoatManager Instance;

	[Header("Boat")]
	[SerializeField]
	private Transform boatTransform;
	[SerializeField]
	private Vector2 boatBounds = new Vector2 ( 350f, 164f );
	[SerializeField]
	private float boatSpeed = 0.3f;
	[SerializeField]
	private Image boatLightImage;

	void Awake () {
		Instance = this;
	}

	public Transform BoatTransform {
		get {
			return boatTransform;
		}
	}

	public void SetBoatPos () {
		Vector2 getDir =NavigationManager.Instance.getDir(NavigationManager.Instance.CurrentDirection);
		BoatTransform.localPosition = new Vector2(-getDir.x * boatBounds.x, -getDir.y * boatBounds.y);
	}
}
