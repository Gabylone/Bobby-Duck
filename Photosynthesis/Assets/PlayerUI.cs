using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class PlayerUI : MonoBehaviour {

	public static List<PlayerUI> playerUIs = new List<PlayerUI>();

	public static PlayerUI GetPlayerUI ( PlayerColor playerColor ) {
		return playerUIs.Find (x => x.playerColor == playerColor);
	}

	public PlayerColor playerColor;

	public GameObject group;
	RectTransform rectTransform;

	public Image backgroundImage;

	public GameObject scoreGroup;
	public RectTransform scoreRectTransform;
	public Text scoreText;

	public GameObject sunGroup;
	public RectTransform sunRectTransform;
	public Text sunText;

	Vector2 initScale;
	public Vector2 targetScale = new Vector2(120, 60);

	public float tweenDur = 0.5f;


	void Start () {

		rectTransform = GetComponent<RectTransform> ();
		initScale = rectTransform.sizeDelta;

		backgroundImage.color = Game.GetColor (Game.GetPlayer (playerColor));

		playerUIs.Add (this);

		Game.onNextPlayer += HandleOnNextPlayer;
		HandleOnNextPlayer (Game.currentPlayer);

		Game.GetPlayer (playerColor).onAddPoints += HandleOnAddPoints;

		UpdateSunPoints ();
	}

	void UpdateSunPoints ()
	{
		Tween.Bounce (sunGroup.transform);

		sunText.text = "" + Game.GetPlayer(playerColor).sunPoints;

	}

	void HandleOnAddPoints (int _sunPoints)
	{
		UpdateSunPoints ();
	}

	void HandleOnNextPlayer (Player player)
	{

		if (player.playerColor == playerColor) {
			Scale ();
//			Invoke ("Scale", tweenDur);
		} else {
			Unscale ();
		}

		Canvas.ForceUpdateCanvases ();
	}

	void Scale () {
		HOTween.To (rectTransform , tweenDur , "sizeDelta" , initScale , false , EaseType.EaseOutBounce, 0f);
	}

	void Unscale () {
		HOTween.To (rectTransform , tweenDur , "sizeDelta" , targetScale , false , EaseType.Linear , 0f);
	}
}
