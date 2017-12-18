using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberCreatorButton : MonoBehaviour {

	public MemberCreator.Apparence apparence;
	public Image image;


	public void OnPointerDown () {

		MemberCreator.Instance.ChangeApparence (apparence);

		Tween.Bounce (image.transform);

	}

	public void UpdateImage() {

		Member member = Crews.playerCrew.captain.MemberID;

		switch (apparence) {
		case MemberCreator.Apparence.genre:
			
			if (Crews.playerCrew.captain.MemberID.Male) {
				image.sprite = MemberCreator.Instance.maleSprite;
			} else {
				image.sprite = MemberCreator.Instance.femaleSprite;
			}

			break;
		case MemberCreator.Apparence.bodyColorID:
//			image.sprite = MemberCreator.Instance.bo;
			break;
		case MemberCreator.Apparence.hairSpriteID:
			if (Crews.playerCrew.captain.MemberID.Male) {
				if (member.hairSpriteID >= 0) {
					image.enabled = true;
					if (member.hairSpriteID >= CrewCreator.Instance.HairSprites_Male.Length)
						print ("ID : " + member.hairSpriteID + " /  LEN : " + CrewCreator.Instance.HairSprites_Male.Length);
					image.sprite = CrewCreator.Instance.HairSprites_Male [member.hairSpriteID];
				} else {
					image.enabled = false;
				}
			} else {
				image.sprite = CrewCreator.Instance.HairSprites_Female [member.hairSpriteID];
			}
			break;
		case MemberCreator.Apparence.hairColorID:
			image.color = CrewCreator.Instance.HairColors [member.hairColorID];
			break;
		case MemberCreator.Apparence.eyeSpriteID:
			image.sprite = CrewCreator.Instance.EyesSprites [member.eyeSpriteID];
			break;
		case MemberCreator.Apparence.eyeBrowsSpriteID:
			image.sprite = CrewCreator.Instance.EyebrowsSprites [member.eyebrowsSpriteID];
			break;
		case MemberCreator.Apparence.beardSpriteID:
			if (member.Male) {

				GetComponent<Button> ().enabled = true;

				if (member.beardSpriteID >= 0) {

					image.enabled = true;
					image.sprite = CrewCreator.Instance.BeardSprites [member.beardSpriteID];
				} else {
					image.enabled = false;
				}
			} else {
				GetComponent<Button> ().enabled = false;
				image.enabled = false;
			}
			break;
		case MemberCreator.Apparence.noseSpriteID:
			image.sprite = CrewCreator.Instance.NoseSprites [member.noseSpriteID];
			break;
		case MemberCreator.Apparence.mouthSpriteId:
			image.sprite = CrewCreator.Instance.MouthSprites [member.mouthSpriteID];
			break;
		}
	}
}
