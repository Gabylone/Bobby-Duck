using UnityEngine;
using System.Collections;

public class StoryTest : MonoBehaviour {

	int lign = 0;
	int decal = 0;

	public int storyID = 0;

	public int X1 = 0;
	public int Y1 = 0;
	public int X2 = 0;
	public int Y2 = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) ) {

			StoryLoader.Instance.CurrentIslandStory = StoryLoader.Instance.Stories [storyID];
			IslandManager.Instance.Enter ();

		}
	}

	void checkDirection () {
		Vector2 dir = (new Vector2 (X2, Y2) - new Vector2 (X1, Y1));

		for (int i = 0; i < 8; ++i ) {

			if ( Vector2.Angle ( dir , NavigationManager.Instance.getDir((Directions)i) ) < 45f ) {
				Debug.Log (NavigationManager.Instance.getDirName((Directions)(i)));
			}

		}
	}
}