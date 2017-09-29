using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IslandInfo : MonoBehaviour {

	[Header("Island Info")]
	[SerializeField]
	private GameObject obj;
	[SerializeField]
	private Text uiText;

	public float displayDuration = 1f;

	// Use this for initialization
	void Start () {

		MinimapChunk.onToucnMinimapChunk += HandleOnTouchMinimapChunk;

		Hide ();

	}

	void HandleOnTouchMinimapChunk (Chunk chunk)
	{
		if (chunk.State == ChunkState.VisitedIsland) {

			uiText.text = chunk.IslandData.storyManager.CurrentStoryHandler.Story.name;

		} else {

			uiText.text = "Ile inconnue";
		
		}

		Show ();

		Tween.Bounce (transform);

		CancelInvoke ();
		Invoke ("Hide" , displayDuration);
	}

	public void Show () {
		obj.SetActive (true);
	}
	public void Hide() {
		obj.SetActive (false);
	}
}
