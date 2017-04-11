using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Story {

	public string 	name 			= "";
	public int 		freq 			= 0;
	public string 	fallbackNode 	= "";
	public int 		storyID 		= 0;
	public string 	fallbackStory 	= "";

	public List<List<string>> content 	= new List<List<string>>();
	public List<List<int>> contentDecal = new List<List<int>>();
	public List<Node> nodes 			= new List<Node> ();

	public Story ()
	{

	}

	public Story (
		int _storyID,
		string _name
	)
	{
		storyID = _storyID;
		name = _name;
	}

}