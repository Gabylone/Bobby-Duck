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

        InGameMenu.Instance.Open();
        DisplayCrew.Instance.Show(CrewMember.GetSelectedMember);

        DisplayCrew.Instance.ShowSkillMenu();

        opened = true;

		group.SetActive (true);

		if (onShowSkillMenu != null)
			onShowSkillMenu ();
	}
	public delegate void OnHideCharacterStats ();
	public static OnHideCharacterStats onHideSkillMenu;
	public void Close () {

        InGameMenu.Instance.Hide();
        DisplayCrew.Instance.Hide();

        DisplayCrew.Instance.HideSkillMenu();

        opened = false;

		Hide ();

		if (onHideSkillMenu != null)
			onHideSkillMenu ();
	}
	void Hide () {
		group.SetActive (false);
	}
	#endregion
}
