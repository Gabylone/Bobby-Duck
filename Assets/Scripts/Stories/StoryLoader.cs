using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLoader : MonoBehaviour {

	public static StoryLoader Instance;

	List<Story> stories = new List<Story>();

	List<List<string>> content = new List<List<string>>();

	[SerializeField]
	private TextAsset data;

	void Awake () {
		Instance = this;

		LoadStories ();

	}

	void Start () {
		
	}

	void Update () {
		if ( Input.GetKeyDown (KeyCode.I) ) {
//			Debug.Log ( content[6][4] );
		}
	}

	void LoadStories ()
	{
		string[] rows = data.text.Split ('\n');

		int rowIndex 		= 0;
		int collumnIndex 	= 0;

		foreach (string row in rows) {


			string[] rowContent = row.Split (';');

			collumnIndex 	= 0;


			foreach (string cellContent in rowContent) {

				if ( rowIndex == 0 ) {
					// create missions

					if (cellContent.Length > 0) {
						stories.Add ( new Story (collumnIndex , cellContent) );
					}

					content.Add (new List<string> ());


				} else {
					// set mission contents

					content [collumnIndex].Add (cellContent);

//					Debug.Log ( cellContent );

				}



				++collumnIndex;
			}

			++rowIndex;

		}
	}

	#region properties
	public void UpdateContent () {


		int rowIndex = 0;
		int collIndex = 0;


		foreach (List<string> row in content) {

			foreach ( string cell in row ) {

				collIndex++;
			}

			collIndex = 0;

			++rowIndex;


		}

	}
	public string GetContent {
		get {
			return content
				[StoryReader.Instance.CurrentStory.index + StoryReader.Instance.Decal]
				[StoryReader.Instance.Index];
		}
	}
	public string ReadDecal (int decal) {

		return content
			[StoryReader.Instance.CurrentStory.index + decal]
			[StoryReader.Instance.Index]; 

	}
	public List<Story> Stories {
		get {
			return stories;
		}
		set {
			stories = value;
		}
	}
	#endregion
}

[System.Serializable]
public class Story {

	public int 		index 	= 0;
	public string 	name 	= "";


	public Story (
		int _index,
		string _name
	)
	{
		index = _index;
		name = _name;
	}

}