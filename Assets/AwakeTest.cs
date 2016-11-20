using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AwakeTest : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
		Debug.Log ("bonjour");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F))
			SceneManager.LoadScene (1);
	}
}
