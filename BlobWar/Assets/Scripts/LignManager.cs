using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LignManager : MonoBehaviour {

	public static LignManager Instance;

	public Lign[] ligns;

    public List<Enemy> enemies = new List<Enemy>();

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
