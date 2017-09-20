using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapChunk : MonoBehaviour {

	public GameObject islandGroup;


	public void UpdateChunk (Coords worldCoords)
	{
		IslandData islandData = Chunk.GetChunk (worldCoords).IslandData;

		if (islandData != null) {
			islandGroup.SetActive (true);
			islandGroup.GetComponent<Image> ().sprite = Island.sprites [islandData.SpriteID];
		} else {
			islandGroup.SetActive (false);
		}
	}
}
