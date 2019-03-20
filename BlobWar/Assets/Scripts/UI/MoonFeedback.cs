using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonFeedback : MonoBehaviour {

    public static MoonFeedback Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
