using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayCrew : MonoBehaviour {

    public static DisplayCrew Instance;

	public GameObject targetGameObject;

	public RectTransform rectTransform;

	public float duration = 1f;
	public float decal = 200f;

	Vector2 initPos;

    public GameObject skillMenuObj;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

		Hide ();
        HideSkillMenu();

		initPos = rectTransform.anchoredPosition;
	}

    public void ShowSkillMenu()
    {
        skillMenuObj.SetActive(true);
    }

    public void HideSkillMenu()
    {
        skillMenuObj.SetActive(false);
    }

    public void Show(CrewMember member)
    {
        member.ShowInInventory();

        HOTween.To(rectTransform, duration, "anchoredPosition", initPos, false);

        targetGameObject.SetActive(true);

        CancelInvoke("Hide");
        CancelInvoke("ShowDelay");
        Invoke("ShowDelay", duration);
    }
    void ShowDelay()
    {
        Tween.Bounce(transform, 0.05f, 1.03f);

    }

    public void Hide ()
	{
		CancelInvoke ("HideDelay");
		CancelInvoke ("ShowDelay");
		HOTween.To (rectTransform, duration, "anchoredPosition", initPos + Vector2.up * decal, false);
		Invoke ("HideDelay", duration);
	}

	void HideDelay () {
		targetGameObject.SetActive (false);
	}

}
