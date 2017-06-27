using UnityEngine;
using System.Collections;

public class IslandButton : MonoBehaviour {

	public int x;
	public int y;

	public void OnClick() {

		string name = "";
		if (MapGenerator.Instance.Chunks [x, y].State == ChunkState.VisitedIsland)
			name = MapGenerator.Instance.Chunks [x, y].IslandData.storyManager.storyHandlers [0].Story.name;
		else
			name = "Ile inconnue";

		MapImage.Instance.showIslandInfo (name,(Vector2)transform.localPosition);
	}

	public bool Visible {
		get {
			return gameObject.activeSelf;
		}
		set {
			gameObject.SetActive(value);
		}
	}
}
