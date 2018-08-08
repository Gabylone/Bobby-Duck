using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenu : MonoBehaviour {

	public GameObject group;

	public bool opened = false;

    private void Awake()
    {
        onShowSkillMenu = null;
        onHideSkillMenu = null;
    }

    void Start () {
		Hide ();

		RayBlocker.onTouchRayBlocker += HandleOnTouchRayBlocker;
	}

	void HandleOnTouchRayBlocker ()
	{
		if ( opened ) {
			Close ();
		}
	}

	#region character stats
	public delegate void OnShowCharacterStats();
	public static OnShowCharacterStats onShowSkillMenu;
	public void Show () {

		opened = true;

		CrewInventory.Instance.HideMenuButtons ();

		group.SetActive (true);

		if (onShowSkillMenu != null)
			onShowSkillMenu ();
	}
	public delegate void OnHideCharacterStats ();
	public static OnHideCharacterStats onHideSkillMenu;
	public void Close () {

		opened = false;

		Hide ();

		Invoke ("CloseDelay",0.01f);

		if (onHideSkillMenu != null)
			onHideSkillMenu ();
	}
	void CloseDelay () {
		CrewInventory.Instance.ShowMenuButtons ();

	}
	void Hide () {
		group.SetActive (false);
	}
	#endregion
}
