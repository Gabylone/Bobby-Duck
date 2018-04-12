using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapChunk : MonoBehaviour {

	public delegate void OnTouchMinimapChunk (Chunk chunk, Transform tr);
	public static OnTouchMinimapChunk onTouchMinimapChunk;

	public Coords coords;

	public GameObject islandGroup;

	public Image image;

	public GameObject questGroup;

	public void InitChunk (Coords worldCoords)
	{
		Chunk chunk = Chunk.GetChunk (worldCoords);
		IslandData islandData = chunk.IslandData;
		image.sprite = Island.minimapSprites[islandData.storyManager.storyHandlers [0].Story.param];

		coords = worldCoords;

		if (chunk.state != ChunkState.VisitedIsland) {
			SetUnvisited ();
		} else {
			SetVisited ();
		}

		if (QuestManager.Instance.currentQuests.Find (x => x.targetCoords == worldCoords) != null) {
			ShowQuestFeedback ();
		} else {
			HideQuestFeedback ();
		}
	}


	public void ShowQuestFeedback () {
		questGroup.SetActive (true);
	}

	public void HideQuestFeedback() {
		questGroup.SetActive (false);
	}

	public void SetVisited () {
		Tween.Bounce (transform);
		image.color = Color.white;
		//
	}

	public void SetUnvisited () {
		image.color = new Color( 0.5f,0.5f,0.5f );
//		image.color = Color.black;
	}

	public void TouchMinimapChunk () {
		
		Tween.Bounce (islandGroup.transform);

		if (onTouchMinimapChunk != null) {
			Debug.Log ("coords :" + coords);
			onTouchMinimapChunk (Chunk.GetChunk (coords), transform);
		}

	}
}
