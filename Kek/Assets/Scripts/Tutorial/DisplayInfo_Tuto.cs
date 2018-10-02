using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfo_Tuto : DisplayInfo {

	public override void Start ()
	{
		base.Start ();

		if (true) {

			Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
			Tutorial.onWaitForConfirm += HandleOnWaitForConfirm;
			Tutorial.onHideTutorial += HandleOnHideTutorial;

		}
	}
    private void OnDestroy()
    {
        Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
        Tutorial.onWaitForConfirm -= HandleOnWaitForConfirm;
        Tutorial.onHideTutorial -= HandleOnHideTutorial;
    }
    void HandleOnHideTutorial ()
	{
		Fade ();
	}

	void HandleOnWaitForConfirm ()
	{
		confirmGroup.SetActive (true);
	}

	void HandleOnDisplayTutorial (TutoStep tutoStep)
	{
		Display (tutoStep.title, tutoStep.description);

        Sound.Instance.PlaySound(Sound.Type.Menu3);

        Invoke("Rayblocker", 0.001f);
    }   

    void Rayblocker()
    {
        RegionRayBlocker.Instance.Show();
        RegionRayBlocker.onTouchRayblocker += Confirm;

    }

    public override void Confirm ()
	{
        Sound.Instance.PlaySound(Sound.Type.Menu4);
		base.Confirm ();

        if (Tutorial.onHideTutorial != null) {
			Tutorial.onHideTutorial ();
		}


    }
}
