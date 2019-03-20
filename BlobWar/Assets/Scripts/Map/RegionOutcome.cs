using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class RegionOutcome : MonoBehaviour {

    public Region region;
    public static RegionOutcome Instance;

    public bool loadOnStart = false;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        Instance = this;
    }

    void Start()
    {
        if ( loadOnStart )
            SceneManager.LoadScene("Menu");
    }

    public Outcome outcome = Outcome.None;

    public enum Outcome
    {
        None,

        Lost,
        Won
    }

}