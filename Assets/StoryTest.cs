using UnityEngine;
using System.Collections;

public class StoryTest : MonoBehaviour {

	int lign = 0;
	int decal = 0;

	public int storyID = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) ) {

			StoryReader.Instance.CurrentStory = StoryLoader.Instance.Stories [storyID];
			IslandManager.Instance.Enter ();

//			Debug.Log (StoryReader.Instance.CurrentStory.content[1][4]);

		}

		if ( Input.GetKeyDown (KeyCode.UpArrow )) {
			--lign;
			Debug.Log ( StoryReader.Instance.CurrentStory.content[decal][lign]);
		}

		if ( Input.GetKeyDown (KeyCode.DownArrow )) {
			++lign;
			Debug.Log ( StoryReader.Instance.CurrentStory.content[decal][lign]);
		}

		if ( Input.GetKeyDown (KeyCode.LeftArrow )) {
			--decal;
			Debug.Log ( StoryReader.Instance.CurrentStory.content[decal][lign]);
		}

		if ( Input.GetKeyDown (KeyCode.RightArrow )) {
			++decal;
			Debug.Log ( StoryReader.Instance.CurrentStory.content[decal][lign]);
		}
	}
}
