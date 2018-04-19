using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberCreatorButton : MonoBehaviour {

    public static MemberCreatorButton lastSelected;

	public MemberCreator.Apparence apparence;
	public Image image;
    public Text text;

    public int id = 0;

    public bool selected = false;

    public float scaleAmount = 2f;

    public virtual void Start()
    {
        UpdateImage();
    }

    #region select & deselect
    public void OnPointerDown () {

        if (selected)
        {
            Deselect();
        }
        else
        {
            Select();
        }

    }


    public void Deselect()
    {
        selected = false;
        Tween.Scale(transform, 0.2f, 1f);
    }

    public void Select()
    {
        if ( lastSelected != null)
        {
            if ( lastSelected.apparence == apparence)
            {
                lastSelected.Deselect();
            }
        }

		MemberCreator.Instance.ChangeApparence (apparence, id);

        Tween.Scale(transform, 0.2f, scaleAmount);

        selected = true;
        lastSelected = this;
    }
    #endregion

    #region image
    public void UpdateImage() {

		Member member = Crews.playerCrew.captain.MemberID;

		switch (apparence) {
		case MemberCreator.Apparence.genre:
			
			if (id == 0) {
				image.sprite = MemberCreator.Instance.maleSprite;
			} else {
				image.sprite = MemberCreator.Instance.femaleSprite;
			}

			break;
		case MemberCreator.Apparence.bodyColorID:
//			image.sprite = MemberCreator.Instance.bo;
			break;
		case MemberCreator.Apparence.hairSpriteID:

			if (member.Male) {

				Enable ();

				if (id >= 0) {
					image.enabled = true;
					image.sprite = CrewCreator.Instance.HairSprites_Male [id];
				} else {
					image.enabled = false;
				}

			} else {
//				image.sprite = CrewCreator.Instance.HairSprites_Female [member.hairSpriteID];
				Disable ();
			}
			break;
		case MemberCreator.Apparence.hairColorID:
			image.color = CrewCreator.Instance.HairColors [id];
			break;
		case MemberCreator.Apparence.eyeSpriteID:
			image.sprite = CrewCreator.Instance.EyesSprites [id];
			break;
		case MemberCreator.Apparence.eyeBrowsSpriteID:
			image.sprite = CrewCreator.Instance.EyebrowsSprites [id];
			break;
		case MemberCreator.Apparence.beardSpriteID:
			if (member.Male) {

				Enable ();

				if (id >= 0) {

					image.enabled = true;
					image.sprite = CrewCreator.Instance.BeardSprites [id];
				} else {
					image.enabled = false;
				}
			} else {
				Disable ();
			}
			break;
		case MemberCreator.Apparence.noseSpriteID:
			image.sprite = CrewCreator.Instance.NoseSprites [id];
			break;
		case MemberCreator.Apparence.mouthSpriteId:
			image.sprite = CrewCreator.Instance.MouthSprites [id];
			break;
        case MemberCreator.Apparence.jobID:
            image.sprite = SkillManager.jobSprites[id];
                text.text = SkillManager.jobNames[id];
            break;
		}
	}
    #endregion

    #region enable disable
    void Enable () {
		gameObject.SetActive (true);
	}

    void Disable()
    {
        gameObject.SetActive(false);
    }   
    #endregion
}
