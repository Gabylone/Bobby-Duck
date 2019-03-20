using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugText : MonoBehaviour
{
    public Text uiText;

    public static DebugText Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        uiText.text = "";
    }

    public void Add ( string str)
    {
        uiText.text += str + "\n";
    }
}
