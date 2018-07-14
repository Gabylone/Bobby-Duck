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

	bool displaying = false;

	Transform currentTransform;

	// Use this for initialization
	void Start () {

		MinimapChunk.onTouchMinimapChunk += HandleOnTouchMinimapChunk;

		Hide ();

	}

	void Update () 
	{
		if ( displaying ) {
			UpdatePosition ();
		}
	}

	void HandleOnTouchMinimapChunk (Chunk chunk, Transform tr)
	{
		if (chunk.state == ChunkState.VisitedIsland) {

//			uiText.text = chunk.IslandData.storyManager.CurrentStoryHandler.Story.name;
			uiText.text = chunk.IslandData.storyManager.CurrentStoryHandler.Story.name;

		} else {

			uiText.text = "?";
		
		}

		Show ();

		Tween.Bounce (transform, 0.2f , 1.05f);

//		HOTween.To ( transform , 0.5f , "position" , Vector3 );


		currentTransform = tr;
		UpdatePosition ();

		CancelInvoke ();
		Invoke ("Hide" , displayDuration);
	}

	void UpdatePosition ()
	{
		transform.position = currentTransform.position + Vector3.up * decal;

	}

	public void Show () {
		displaying = true;
		obj.SetActive (true);
	}
	public void Hide() {
		displaying = false;
		obj.SetActive (false);
	}
}
