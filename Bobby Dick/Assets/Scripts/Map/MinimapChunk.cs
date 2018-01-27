using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapChunk : MonoBehaviour {

	public delegate void OnTouchMinimapChunk (Chunk chunk);
	public static OnTouchMinimapChunk onToucnMinimapChunk;

	public Coords coords;

	public GameObject islandGroup;

	public void InitChunk (Coords worldCoords)
	{
		coords = worldCoords;

		IslandData islandData = Chunk.GetChunk (worldCoords).IslandData;

		if (islandData == null) {
			Debug.Log ("AAAAHAHAHA CEES TNUL");
			return;
		}
		islandGroup.GetComponent<Image> ().sprite = Island.minimapSprites[islandData.SpriteID];

	}

	public void TouchMinimapChunk () {
		
		Tween.Bounce (islandGroup.transform);

		if (onToucnMinimapChunk != null) {
			onToucnMinimapChunk (Chunk.GetChunk (coords));
		}

	}
}
