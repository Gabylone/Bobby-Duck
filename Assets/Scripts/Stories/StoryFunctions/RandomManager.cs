using UnityEngine;
using System.Collections;

public class RandomManager : MonoBehaviour {

	public static RandomManager Instance;

	void Awake( ) {
		Instance = this;
	}

	#region random
	public void RandomPercent (string cellParams) {

		float chance = float.Parse ( cellParams );

		float value = Random.value * 100;

		int randomDecal = value < chance ? 0 : 1;

		StoryReader.Instance.NextCell ();

		int decal = StoryReader.Instance.SaveDecal > -1 ? StoryReader.Instance.SaveDecal : randomDecal;
		StoryReader.Instance.SaveDecal = decal;

		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();

	}
	public void RandomRange (string cellParams) {

		int range = int.Parse (cellParams);
		int randomDecal = Random.Range (0, range);

		StoryReader.Instance.NextCell ();

		int decal = StoryReader.Instance.SaveDecal > -1 ? StoryReader.Instance.SaveDecal : randomDecal;

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
