using UnityEngine;
using System.Collections;

public class RandomManager : MonoBehaviour {

	public static RandomManager Instance;

	public int targetDecal = 0;

	public bool overrideRandom = false;

	void Awake( ) {
		Instance = this;


	}

	void Update () {
		if ( overrideRandom ) {
			KeyCode[] keyCodes = new KeyCode[9] {
				KeyCode.Alpha1,
				KeyCode.Alpha2,
				KeyCode.Alpha3,
				KeyCode.Alpha4,
				KeyCode.Alpha5,
				KeyCode.Alpha6,
				KeyCode.Alpha7,
				KeyCode.Alpha8,
				KeyCode.Alpha9,
			};

			int a = 0;
			foreach (KeyCode keyCode in keyCodes) {
				if (Input.GetKeyDown (keyCode))
					targetDecal = a;
				++a;
			}
		}
	}

	#region random
	public void RandomPercent (string cellParams) {

		float chance = float.Parse ( cellParams );

		float value = Random.value * 100;

		int randomDecal = value < chance ? 0 : 1;

		StoryReader.Instance.NextCell ();

		int decal = 0;

		if (overrideRandom) {
			decal = Mathf.Clamp (targetDecal, 0, 2);
		} else {
			if (StoryReader.Instance.SaveDecal > -1) {
				decal = StoryReader.Instance.SaveDecal;
			} else {
				decal = randomDecal;
			}
		}

		StoryReader.Instance.SaveDecal = decal;

		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();

	}
	public void RandomRange (string cellParams) {

		int range = int.Parse (cellParams);
		int randomDecal = Random.Range (0, range);

		StoryReader.Instance.NextCell ();

		int decal = 0;

		if (overrideRandom) {
			decal = Mathf.Clamp (targetDecal, 0, range-1);	
		} else {
			if (StoryReader.Instance.SaveDecal > -1) {
				decal = StoryReader.Instance.SaveDecal;
			} else {
				decal = randomDecal;
			}
		}

		StoryReader.Instance.SaveDecal = decal;
		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();

	}
	public void RandomRedoPercent (string cellParams) {

		float chance = float.Parse ( cellParams );

		float value = Random.value * 100;

		int decal = value < chance ? 0 : 1;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();

	}
	public void RandomRedoRange (string cellParams) {

		int range = int.Parse (cellParams);
		int randomDecal = Random.Range (0, range);

		StoryReader.Instance.NextCell ();

		StoryReader.Instance.SetDecal (randomDecal);

		StoryReader.Instance.UpdateStory ();

	}
	#endregion

}
