using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Fight_LoadSprites : MonoBehaviour {

	public enum SpriteIndex {
		body,
		rightArm,
		leftArm,
		weapon,
		head,
		eyes,
		eyebrows,
		hair,
		nose,
		mouth,
		beard,
	}

	SpriteRenderer[] allSprites;
	float fade_Duration;
	Color[] fade_InitColors;
	bool fading = false;
	float timer = 0f;

	public void Init ()
	{
		allSprites = GetComponentsInChildren<SpriteRenderer> (true);
		GetSpriteColors ();
	}

	public void UpdateSprites ( Member memberID ) {

		ResetColors ();

		if (memberID.hairSpriteID > -1)
			allSprites[(int)SpriteIndex.hair].sprite = memberID.Male ? CrewCreator.Instance.HairSprites_Male [memberID.hairSpriteID] : CrewCreator.Instance.HairSprites_Female [memberID.hairSpriteID];
		else
			allSprites[(int)SpriteIndex.hair].enabled = false;
		
		allSprites[(int)SpriteIndex.hair].color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		if (memberID.beardSpriteID > -1)
			allSprites[(int)SpriteIndex.beard].sprite = CrewCreator.Instance.BeardSprites [memberID.beardSpriteID];
		else
			allSprites[(int)SpriteIndex.beard].enabled = false;
		allSprites[(int)SpriteIndex.beard].color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		allSprites[(int)SpriteIndex.eyes].sprite = CrewCreator.Instance.EyesSprites [memberID.eyeSpriteID];
		allSprites[(int)SpriteIndex.eyebrows].sprite = CrewCreator.Instance.EyebrowsSprites [memberID.eyebrowsSpriteID];
		allSprites[(int)SpriteIndex.eyebrows].color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		allSprites[(int)SpriteIndex.nose].sprite = CrewCreator.Instance.NoseSprites [memberID.noseSpriteID];

		allSprites[(int)SpriteIndex.mouth].sprite = CrewCreator.Instance.MouthSprites [memberID.mouthSpriteID];

		// body
		allSprites[(int)SpriteIndex.body].sprite = CrewCreator.Instance.BodySprites[memberID.Male ? 0:1];
	}



	#region sprite colors
	void GetSpriteColors () {
		fade_InitColors = new Color[allSprites.Length];

		for (int i = 0; i < fade_InitColors.Length; i++) {
			fade_InitColors [i] = allSprites [i].color;
		}
	}

	void ResetColors ()
	{
		int a = 0;
		foreach ( SpriteRenderer sprite in allSprites ) {
			sprite.color = fade_InitColors [a];
			++a;
		}
	}
	#endregion

	#region fade
	public void FadeSprites (float dur) {

		foreach ( SpriteRenderer sprite in allSprites ) {
			HOTween.To (sprite , dur , "color" , Color.clear);
		}
	}
	#endregion

	#region sprite order
	public void UpdateOrder (int fighterIndex)
	{
		foreach ( SpriteRenderer sprite in allSprites ) {
			sprite.sortingOrder += 11 * (fighterIndex+1);
		}

	}
	#endregion
}
