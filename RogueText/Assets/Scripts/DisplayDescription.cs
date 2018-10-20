using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDescription : MonoBehaviour {
	
	public static DisplayDescription Instance;
	public RectTransform verticalLayoutGroup;

	TextTyper[] textTypers;

	public GameObject textTyperParent;

	void Awake () {
		Instance = this;
	}

	void Start ()
	{
		textTypers = textTyperParent.GetComponentsInChildren<TextTyper> ();

		Player.onPlayerMove += HandleOnPlayerMove;

	}

	void HandleOnPlayerMove (Coords previousCoords, Coords newCoords)
	{
        DisplayTileDescription();

    }

    public void DisplayTileDescription()
    {
        foreach (var item in textTypers)
        {
            item.Hide();
        }

        StartCoroutine(DisplayCoroutine());

    }

    IEnumerator DisplayCoroutine ()
	{
        DisplayFeedback.Instance.Clear();

        LayoutRebuilder.ForceRebuildLayoutImmediate(verticalLayoutGroup);

        DisplayInput.Instance.Hide ();

        foreach (var item in textTypers)
        {
            item.Clear();
        }

        foreach (var item in textTypers)
        {
            item.UpdateCurrentTileDescription();
        }

		foreach (var item in textTypers) {

            item.Display();

			while (!item.finished)
				yield return null;

		}

		yield return new WaitForEndOfFrame ();

		DisplayInput.Instance.Show ();

	}

    public void Display( string text )
    {
        foreach (var item in textTypers)
        {
            item.Clear();
            item.Hide();
        }

        textTypers[0].Display(text);

    }

    public void ClearAll()
    {
        foreach (var item in textTypers)
        {
            item.Clear();
        }
    }

}
