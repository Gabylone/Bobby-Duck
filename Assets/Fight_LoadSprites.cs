using UnityEngine;
using System.Collections;

public class Fight_LoadSprites : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer bodySprite;
	[SerializeField]
	private SpriteRenderer hairSprite;
	[SerializeField]
	private SpriteRenderer eyesSprite;
	[SerializeField]
	private SpriteRenderer eyebrowsSprite;
	[SerializeField]
	private SpriteRenderer noseSprite;
	[SerializeField]
	private SpriteRenderer mouthSprite;
	[SerializeField]
	private SpriteRenderer beardSprite;

	public void UpdateSprites ( MemberID memberID ) {

		if (memberID.hairSpriteID > -1)
			hairSprite.sprite = memberID.male ? CrewCreator.Instance.HairSprites_Male [memberID.hairSpriteID] : CrewCreator.Instance.HairSprites_Female [memberID.hairSpriteID];
		else
			hairSprite.enabled = false;
		hairSprite.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		if (memberID.beardSpriteID > -1)
			beardSprite.sprite = CrewCreator.Instance.BeardSprites [memberID.beardSpriteID];
		else
			beardSprite.enabled = false;
		beardSprite.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		eyesSprite.sprite = CrewCreator.Instance.EyesSprites [memberID.eyeSpriteID];
		eyebrowsSprite.sprite = CrewCreator.Instance.EyebrowsSprites [memberID.eyebrowsSpriteID];
		eyebrowsSprite.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		noseSprite.sprite = CrewCreator.Instance.NoseSprites [memberID.noseSpriteID];
//		noseSprite.color = CrewCreator.Instance.HairColors [memberID.bodyColorID];

		mouthSprite.sprite = CrewCreator.Instance.MouthSprites [memberID.mouthSpriteID];

		// body
		bodySprite.sprite = CrewCreator.Instance.BodySprites[memberID.male ? 0:1];

	}

}
