using UnityEngine;
using System.Collections;

public class Fight_LoadSprites : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer bodySprite;
	[SerializeField]
	private SpriteRenderer hairSprite;

	public void UpdateSprites ( MemberID memberID ) {

		int hearIndex = memberID.hairSpriteID;
		if (hearIndex > -1)
			hairSprite.sprite = CrewCreator.Instance.HairSprites [hearIndex];
		else
			hairSprite.enabled = false;
		hairSprite.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		// body
		bodySprite.sprite = CrewCreator.Instance.BodySprites[memberID.male ? 0:1];

	}

}
