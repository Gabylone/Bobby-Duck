using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayTreasure : MonoBehaviour {

    public Animator animator;

    public GameObject group;

    public GameObject pearlPrefab;

    public Transform pearlAppearAnchor;

    public float showPearlsDelay = 1f;
    public int pearlAmount = 10;

    public float pearlDuration = 0.2f;

    public Transform pearlDestination;

    public DisplayPearls displayPearls;

	// Use this for initialization
	void Start () {
        StoryFunctions.Instance.getFunction += HandleOnGetFunction;
	}

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.L ) )
        {
            ShowTreasure();
        }
    }

    private void HandleOnGetFunction(FunctionType func, string cellParameters)
    {
        if (func == FunctionType.EndGame)
        {

            ShowTreasure();

        }
    }

    private void ShowTreasure()
    {
        group.SetActive(true);
    }

    public void OpenChest()
    {
        animator.SetTrigger("open");

        displayPearls.transform.SetParent(this.transform);

        Debug.Log("open chest");

        Invoke("ShowPearls", showPearlsDelay );
    }

    void ShowPearls()
    {
        StartCoroutine(ShowPearlsCoroutine());
    }

    IEnumerator ShowPearlsCoroutine()
    {
        for (int i = 0; i < pearlAmount; i++)
        {
            GameObject pearl = Instantiate(pearlPrefab, transform) as GameObject;

            pearl.transform.position = pearlAppearAnchor.position;

            HOTween.To(pearl.transform, pearlDuration, "position", pearlDestination.position);

            yield return new WaitForSeconds(pearlDuration);

            PlayerInfo.Instance.AddPearl(1);

        }


        PlayerInfo.Instance.AddApparenceItem();


        PlayerInfo.Instance.Save();


        yield return new WaitForSeconds(pearlDuration);

        Transitions.Instance.ScreenTransition.FadeIn(1f);

    }
}
