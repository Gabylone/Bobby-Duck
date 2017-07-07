using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IconVisual : MonoBehaviour {
	
	public void UpdateVisual (MemberID memberID) {

		FaceImage.color = CrewCreator.Instance.Beige;

		// hair
		if (memberID.HairSpriteID > -1) {
			HairImage.sprite = memberID.Male ? CrewCreator.Instance.HairSprites_Male [memberID.HairSpriteID] : CrewCreator.Instance.HairSprites_Female [memberID.HairSpriteID];
			HairImage.enabled = true;
		} else {
			HairImage.enabled = false;
		}

		HairImage.color = CrewCreator.Instance.HairColors [memberID.HairColorID];

		if (memberID.BeardSpriteID > -1) {
			BeardImage.enabled = true;
			BeardImage.sprite = CrewCreator.Instance.BeardSprites [memberID.BeardSpriteID];
		} else {
			BeardImage.enabled = false;

		}
		BeardImage.color = CrewCreator.Instance.HairColors [memberID.HairColorID];

		// eyes
		EyesImage.sprite = CrewCreator.Instance.EyesSprites [memberID.EyeSpriteID];
		EyebrowsImage.sprite = CrewCreator.Instance.EyebrowsSprites [memberID.EyebrowsSpriteID];
		EyebrowsImage.color = CrewCreator.Instance.HairColors [memberID.HairColorID];

		// nose
		NoseImage.sprite = CrewCreator.Instance.NoseSprites[memberID.NoseSpriteID];

		// mouth
		MouthImage.sprite = CrewCreator.Instance.MouthSprites [memberID.MouthSpriteID];
		MouthImage.color = CrewCreator.Instance.Beige;

		// body
		BodyImage.sprite = CrewCreator.Instance.BodySprites[memberID.Male ? 0:1];
	}

	[Header("BobyParts")]
	[SerializeField]
	private Image faceImage;
	[SerializeField]
	private Image beardImage;
	[SerializeField]
	private Image bodyImage;
	[SerializeField]
	private Image hairImage;
	[SerializeField]
	private Image eyesImage;
	[SerializeField]
	private Image eyebrowsImage;
	[SerializeField]
	private Image mouthImage;
	[SerializeField]
	private Image noseImage;

	public Image FaceImage {
		get {
			return faceImage;
		}
	}

	public Image BeardImage {
		get {
			return beardImage;
		}
	}

	public Image HairImage {
		get {
			return hairImage;
		}
	}

	public Image EyesImage {
		get {
			return eyesImage;
		}
	}

	public Image EyebrowsImage {
		get {
			return eyebrowsImage;
		}
	}

	public Image MouthImage {
		get {
			return mouthImage;
		}
	}

	public Image NoseImage {
		get {
			return noseImage;
		}
	}

	public Image BodyImage {
		get {
			return bodyImage;
		}
	}
}
