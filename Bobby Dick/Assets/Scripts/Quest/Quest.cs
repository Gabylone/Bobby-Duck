﻿public class Quest {

	public enum QuestState {
		Started,
		Returning,
		Finished
	}

	public QuestState questState = QuestState.Started;

	public int questID = 0;

	public int goldValue = 0;

	public int level = 0;

	public Coords originCoords;
	public Coords targetCoords;

	public Quest () {

	}

	public Story Story {
		get {
			return StoryLoader.Instance.Quests [questID];
		}
	}

	public void ShowOnMap ()
	{
		MapImage.Instance.OpenMap ();
		MapImage.Instance.CenterOnCoords (targetCoords);
		MapImage.Instance.HighlightPixel (targetCoords);
	}

	//
}