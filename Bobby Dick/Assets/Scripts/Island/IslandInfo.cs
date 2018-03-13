using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class IslandInfo : MonoBehaviour {

	[Header("Island Info")]
	[SerializeField]
	private GameObject obj;
	[SerializeField]
	private Text uiText;

	public float decal = 2f;

	public float displayDuration = 1f;

	// Use this for initialization
	void Start () {

		MinimapChunk.onToucnMinimapChunk += HandleOnTouchMinimapChunk;

		Hide ();

	}

	void HandleOnTouchMinimapChunk (Chunk chunk, Vector3 pos)
	{
		if (chunk.state == ChunkState.VisitedIsland) {

//			uiText.text = chunk.IslandData.storyManager.CurrentStoryHandler.Story.name;
			uiText.text = chunk.IslandData.storyManager.CurrentStoryHandler.Story.name;

		} else {

			uiText.text = "Ile inconnue";
		
		}

		Show ();

		Tween.Bounce (transform, 0.2f , 1.05f);

//		HOTween.To ( transform , 0.5f , "position" , Vector3 );

		transform.position = pos + Vector3.up * decal;

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
