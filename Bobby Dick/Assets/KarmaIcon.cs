using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarmaIcon : MonoBehaviour {

    public GameObject group;

	// Use this for initialization
	void Start () {

        CrewInventory.Instance.onOpenInventory += Show;
        CrewInventory.Instance.onCloseInventory += Hide;

        SkillMenu.onShowSkillMenu += Hide;
        SkillMenu.onHideSkillMenu += Show;

        Hide();
    }

    void Show()
    {
        group.SetActive(true);
    }

    void Show(CrewMember member)
    {
        Show();
    }

    void Hide()
    {
        group.SetActive(false);
    }
}
