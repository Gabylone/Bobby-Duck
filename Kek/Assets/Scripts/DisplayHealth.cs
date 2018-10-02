using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayHealth : MonoBehaviour {

    public delegate void OnPlayerFailure();
    public static OnPlayerFailure onPlayerFailure;

    public GameObject[] healthGroups;

	// Use this for initialization
	void Start () {

        Inventory.Instance.health = 3;

        Zombie.onReachPlayer += HandleOnReachPlayer;

        UpdateDisplay();
	}
    private void OnDestroy()
    {
        Zombie.onReachPlayer -= HandleOnReachPlayer;
    }

    private void HandleOnReachPlayer()
    {
        --Inventory.Instance.health;

        Sound.Instance.PlaySound(Sound.Type.UI_Wrong);

        UpdateDisplay();

        if (Inventory.Instance.health <= 0 )
        {
            RegionOutcome.Instance.outcome = RegionOutcome.Outcome.Lost;
            MessageDisplay.Instance.Display("Région invahie");

            Invoke("FailureDelay", 1f);

            if (onPlayerFailure != null)
                onPlayerFailure();
        }
    }

    void FailureDelay()
    {
        SceneManager.LoadScene("Main (map)");
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < healthGroups.Length; i++)
        {
            if (i < Inventory.Instance.health)
            {
                healthGroups[i].SetActive(true);
                Tween.Bounce(healthGroups[i].transform);
            }
            else
            {
                healthGroups[i].SetActive(false);

            }


        }
    }
}
