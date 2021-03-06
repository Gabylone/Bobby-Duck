﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	public Vector2 worldPosition;
    public float worldRotation = 0f;

	public StoryManager storyManager;

	public IslandData ()
	{

	}

    public IslandData (StoryType storyType )
	{
		storyManager = new StoryManager ();

		storyManager.InitHandler (storyType);

		worldPosition = Island.Instance.GetRandomPosition ();
        worldRotation = Random.Range( 0, 360f );
	}
}
