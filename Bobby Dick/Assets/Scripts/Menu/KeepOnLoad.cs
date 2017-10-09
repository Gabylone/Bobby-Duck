using UnityEngine;
using System.Collections;

public class KeepOnLoad : MonoBehaviour {

	void Start () {
		DontDestroyOnLoad (gameObject);
	}

	public static int dataToLoad = -1;
}
