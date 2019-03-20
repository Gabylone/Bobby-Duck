using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLanguage : DisplayGroup {

    public static DisplayLanguage Instance;

    public Transform[] buttons;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
    }

	public override void Update ()
	{
		base.Update ();

		if (Input.GetKeyDown (KeyCode.M))
			Open ();
	}


    public void SetLanguage(int i)
    {
        Tween.Bounce(buttons[i]);

        Inventory.currentLanguageType = (Inventory.LanguageType)i;

        Inventory.Instance.Save();

        Close();
    }

	public override void Open ()
	{
		base.Open ();
	}

	public override void Close (bool _showBottomBar)
	{
		base.Close (_showBottomBar);

		Debug.Log ("closing lang close");

	}

	public override void Close ()
	{
		base.Close ();

		Debug.Log ("closing lang");
	}
}
