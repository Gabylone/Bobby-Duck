using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IslandButton : MonoBehaviour {

	public Coords coords;

	Image image;

	[SerializeField]
	private float highlightDuration = 1f;
	private bool highlighted = false;

	[SerializeField]
	private Sprite highlightSprite;

	public void Init (Coords c) {
		image = GetComponent<Image> ();
		coords = c;
		Visible = false;
	}

	public void OnClick() {

		string name = "";
		if (MapGenerator.Instance.GetChunk(coords).State == ChunkState.VisitedIsland)
			name = MapGenerator.Instance.GetChunk(coords).IslandData.storyManager.storyHandlers [0].Story.name;
		else
			name = "Ile inconnue";

		MapImage.Instance.showIslandInfo (name,(Vector2)transform.localPosition);
	}

	public bool Visible {
		get {
			return gameObject.activeSelf;
		}
		set {
//			gameObject.SetActive(value);
		}
	}

	public void Highlight () {
		Highlighted = true;

	}

	public bool Highlighted {
		get {
			return highlighted;
		}
		set {

			if ( value )
				Visible = true;

			highlighted = value;

			image.sprite = value ? highlightSprite : null;

			image.color = value ? Color.red : Color.clear;

			transform.localScale = value ? Vector3.one * 3f : Vector3.one;

			GetComponentInChildren<Animator> ().SetBool ("bounce", value);

			CancelInvoke ();

			Invoke ("DisableHighlight", highlightDuration);

		}
	}

	private void DisableHighlight (){
		Highlighted = false;
	}
}
