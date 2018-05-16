using UnityEngine;
using System.Collections;

public class KeepOnLoad : MonoBehaviour {

	void Start () {
		DontDestroyOnLoad (gameObject);
	}

	public static int dataToLoad = -1;

	public static bool displayTuto = false;

    public static string mapName = "";

    public static int pearls = 0;
}
