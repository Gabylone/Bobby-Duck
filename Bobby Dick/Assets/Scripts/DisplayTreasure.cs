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
    public float rangeX = 0f;
    public float rangeY = 0f;

    public float halfWayDecal = 1f;

    public Transform pearlDestination;

    public DisplayPearls displayPearls;

	// Use this for initialization
	void Start () {
        StoryFunctions.Instance.getFunction += HandleOnGetFunction;
	}

    private void HandleOnGetFunction(FunctionType func, string cellParameters)
    {
        if (func == FunctionType.EndGame)
        {

            ShowTreasure();

        }
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShowTreasure();
        }
    }*/

    private void ShowTreasure()
    {
        group.SetActive(true);
    }

    public void OpenChest()
    {
        if (KeepOnLoad.Instance.mapName != "")
        {
            pearlAmount = KeepOnLoad.Instance.price;
        }
        else
        {
            pearlAmount = 100;
        }

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
        int a = pearlAmount;
        int r = 20;

        while (a > 0)
        {

            for (int i = 0; i < r; i++)
            {
                GameObject pearl = Instantiate(pearlPrefab, pearlAppearAnchor) as GameObject;

                Vector3 p = new Vector3(Random.Range(-rangeX, rangeX), 0f, Random.Range(-rangeY, rangeY));

                pearl.GetComponent<RectTransform>().localPosition = p;

                Vector3 halfway = ( p + (pearlDestination.position - p) / 2f ) + Random.insideUnitSphere * halfWayDecal;

                HOTween.To(pearl.transform, pearlDuration, "position", halfway, false , EaseType.Linear , 0f);
                HOTween.To(pearl.transform, pearlDuration, "position", pearlDestination.position, false , EaseType.Linear , pearlDuration);

                yield return new WaitForEndOfFrame();

                PlayerInfo.Instance.AddPearl(r);


            }

            yield return new WaitForSeconds(pearlDuration);


            a -= r;
        }


        PlayerInfo.Instance.Save();


        yield return new WaitForSeconds(pearlDuration);

        Transitions.Instance.ScreenTransition.FadeIn(1f);

    }
}
