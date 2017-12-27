using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Soil : MonoBehaviour {

	public GameObject seedPrefab;
	public GameObject smallTreePrefab;
	public GameObject mediumTreePrefab;
	public GameObject tallTreePrefab;

	public GameObject shadowObj;

	public Player associatedPlayer;

	Color initColor;

	public float darkAmount = 0.7f;

	public int str = 0;

//	public enum Type {
//		Light,
//		Med,
//	}

	public enum Content {
		Empty,
		Seed,
		SmallTree,
		MediumTree,
		TallTree,
	}

	public enum State {
		Light,
		Dark,
	}

	public State state {
		get {
			return shadowSources.Count > 0 ? State.Dark : State.Light;
		}
	}
	public int shadowLevel = 0;
	public Content currentContent = Content.Empty;
	public Content previousContent;

	Renderer renderer;

	public Coord coord;
	public int x;
	public int y;

	void Start () {
		
		Game.onNextPhase += HandleOnNextPhase;
		Game.onNextPlayer += HandleOnNextPlayer;

		renderer = GetComponentInChildren<Renderer> ();

		renderer.material = Game.soilMats [(int)str];
		initColor = renderer.material.color;

		coord = new Coord (x+1,y+1);

		soils.Add (this);

		UpdateShadowVisual ();
		SetContent (Content.Empty);

//		Print ("?");

	}



	public void Print (string str) {
		GetComponentInChildren<TextMesh> (true).gameObject.SetActive (true);
//		GetComponentInChildren<TextMesh> ().text = "X " + coord.x + "\nY " + coord.y;
		GetComponentInChildren<TextMesh> ().text = str;
	}

	void HandleOnNextPhase (Game.Phase phase)
	{
		if ( phase == Game.Phase.Photosynthesis ) {
			
		}
	}

	public GameObject sunPointPrefab;

	void HandleOnNextPlayer (Player player)
	{

	}

	public float timeBetweenSunPoints = 1f;

	public IEnumerator AddSunPointsCoroutine () {

		yield return new WaitForSeconds (timeBetweenSunPoints);

		Tween.Bounce (transform);

		int sunPoints = (int)currentContent - 1;

//		print ("soil creating : " + sunPoints);

		for (int i = 0; i < sunPoints; i++) {

			// ATTENTION : un point à la fois
			associatedPlayer.AddPoint (1);

			// sun point instance
			GameObject sunPointObj = Instantiate (sunPointPrefab, Game.Instance.canvasObj.transform) as GameObject;
			sunPointObj.transform.position = transform.position + Vector3.up * 1f;
			sunPointObj.transform.localScale = Vector3.one;

			HOTween.To (sunPointObj.transform, timeBetweenSunPoints, "position", PlayerUI.GetPlayerUI(associatedPlayer.playerColor).sunRectTransform.position  );

			Destroy (sunPointObj, timeBetweenSunPoints);

			yield return new WaitForSeconds (timeBetweenSunPoints);

		}
	}

	#region shadow
	public void CheckShadow () {
		
		switch (currentContent) {

		case Content.Empty:
			RemoveShadow ();
			break;

		case Content.SmallTree:
		case Content.MediumTree:
		case Content.TallTree:
			CastShadow ();
			break;
		default:
			break;
		
		}
	}
	void CastShadow ()
	{
		int range = 1;

		switch (currentContent) {
//		case Content.SmallTree:
//			break;
		case Content.MediumTree:
			range = 2;
			break;
		case Content.TallTree:
			range = 3;
			break;
		default:
			break;
		}

		for (int i = 0; i < range; i++) {
			Soil soil = GetSoil (coord + Coord.GetCoord (Sun.direction, i + 1));
			if (soil != null) {
				soil.AddShadow (coord);
				Tween.Bounce (soil.transform);
			}
		}
	}

	void RemoveShadow ()
	{
		int range = 1;

		switch (previousContent) {
		//		case Content.SmallTree:
		//			break;
		case Content.MediumTree:
			range = 2;
			break;
		case Content.TallTree:
			range = 3;
			break;
		default:
			break;
		}

		for (int i = 0; i < range; i++) {
			Soil soil = GetSoil (coord + Coord.GetCoord (Sun.direction, i + 1));
			if (soil != null)
				soil.RemoveShadow (coord);
		}
	}


	public List<Coord> shadowSources = new List<Coord> ();

	public void ClearShadows () {
		shadowSources.Clear ();
		UpdateShadowVisual ();
	}

	public void AddShadow ( Coord _coord ) {

//		print (" found coord : " + shadowSources.Find (x => x.x == _coord.x && x.y == _coord.y).ToString() );

		if (shadowSources.Find (x => x.x == _coord.x && x.y == _coord.y).IsNull() ) {
			shadowSources.Add (_coord);
		}

		UpdateShadowVisual ();

	}
	public void RemoveShadow ( Coord _coord ) {

		if (shadowSources.Find (x => x == _coord) != null) {
			shadowSources.Remove (_coord);
		}

		UpdateShadowVisual ();

	}
	void UpdateShadowVisual () {
		
		switch (state) {

		case State.Light:
			
			if (currentContent != Content.Empty) {
				Renderer rend = contentObj.GetComponentsInChildren<Renderer> () [0];
				rend.material.color = Game.GetColor (associatedPlayer);
			}

			shadowObj.SetActive (false);

			break;

		case State.Dark:
			//			renderer.material.color = Color.Lerp ( initColor , Color.black , darkAmount );
			if (currentContent != Content.Empty) {
				if (ReachesLight () == false) {
					Renderer rend = contentObj.GetComponentsInChildren<Renderer> () [0];
					rend.material.color = Color.Lerp (Game.GetColor (associatedPlayer), Color.black, darkAmount);
				}
			}

			shadowObj.SetActive (true);

			break;

		default:
			break;
		}
	}
	#endregion

	#region content
	public GameObject contentObj;
	public delegate void OnSetContent (Content content);
	public static OnSetContent onSetContent;
	public void SetContent ( Content targetContent ) {

		previousContent = currentContent;
		currentContent = targetContent;

		if (targetContent == Content.Empty) {
			SetPlayer (null);
		} else {
			if (associatedPlayer == null) {
				SetPlayer (Game.currentPlayer);
			}
		}

		UpdateContent (targetContent);
		CheckShadow ();
		UpdateShadowVisual ();

		if (onSetContent != null)
			onSetContent (targetContent);

	}
	void UpdateContent ( Content content ) {

		if (contentObj != null)
			Destroy (contentObj);

		if ( content != Content.Empty ) {
			
			GameObject prefab = seedPrefab;

			switch (content) {
			case Content.Seed:
				prefab = seedPrefab;
				break;
			case Content.SmallTree:
				prefab = smallTreePrefab;
				break;
			case Content.MediumTree:
				prefab = mediumTreePrefab;
				break;
			case Content.TallTree:
				prefab = tallTreePrefab;
				break;
			}

			contentObj = Instantiate (prefab, transform) as GameObject;
			contentObj.transform.position = transform.position;

			Renderer rend = contentObj.GetComponentsInChildren<Renderer> () [0];
			rend.material.color = Game.GetColor (Game.currentPlayer);
		}

	}
	#endregion

	#region player
	void SetPlayer ( Player player ) {
		associatedPlayer = player;
	}
	#endregion

	public void OnMouseDown () {

		switch (currentContent) {
		case Content.Empty:
			SetContent (Content.Seed);
			break;
		case Content.Seed:
			SetContent (Content.SmallTree);
			break;
		case Content.SmallTree:
			SetContent (Content.MediumTree);
			break;
		case Content.MediumTree:
			SetContent (Content.TallTree);
			break;
		case Content.TallTree:
			SetContent (Content.Empty);
			break;
		default:
			break;
		}

		Tween.Bounce (transform);

	}

	public bool ReachesLight () {
		foreach (var item in shadowSources) {
			Soil soilSource = GetSoil (item);

			if ( (int)soilSource.currentContent >= (int)currentContent ) {
				return false;
			}
		}

		return true;

	}

	public static List<Soil> soils = new List<Soil>();

	public static Soil GetSoil ( Coord _coord ) {
//		if (_coord.x < -3 || _coord.x > 3 || _coord.y < -6 || _coord.y > 6)
//			return null;
		return soils.Find (x => x.coord == _coord);
	}

}

