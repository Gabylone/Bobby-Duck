using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMinimap : MonoBehaviour {

	public GameObject minimapChunkPrefab;

	public GameObject parent;

	public Image minimapImage;

	Vector2 scale;

	public int range = 1;

	// Use this for initialization
	void Start () {
		//scale = new Vector2(minimapImage.rectTransform.rect.width,minimapImage.rectTransform.rect.height);
		scale = new Vector2(minimapChunkPrefab.GetComponent<RectTransform>().rect.width,minimapChunkPrefab.GetComponent<RectTransform>().rect.height);

		UpdateMinimap ();
	}

	void UpdateMinimap ()
	{
		for (int x = -range; x <= range; x++) {
			for (int y = -range; y <= range; y++) {
				PlaceMinimapChunk (new Coords (x, y));
			}
		}
	}

	void PlaceMinimapChunk(Coords c) {

		// INST
		GameObject minimapChunk = Instantiate (minimapChunkPrefab, parent.transform);

		minimapImage.rectTransform.sizeDelta = scale * (range * 2 + 1) / 2;

		// SCALE
		minimapChunk.transform.localScale = Vector3.one;

		// POS
		Vector3 pos;

		//Vector2 decal = scale / (Boats.PlayerBoatInfo.ShipRange * 2 + 1);
		Vector2 decal = scale;

		pos = new Vector3 ( c.x * decal.x , c.y * decal.y, 0f);

		minimapChunk.GetComponent<RectTransform>().anchoredPosition = pos;
		minimapChunk.GetComponent<RectTransform> ().sizeDelta = decal;

		Coords worldCoords = Boats.PlayerBoatInfo.CurrentCoords + new Coords(c.x,-c.y);

		minimapChunk.GetComponent<MinimapChunk> ().UpdateChunk (worldCoords);

		if (minimapChunk.GetComponentInChildren<Text> () != null) {
			minimapChunk.GetComponentInChildren<Text> ().text = "" +
				"X : " + worldCoords.x + "\n" +
				"Y : " + worldCoords.y;
		}
	}
}
