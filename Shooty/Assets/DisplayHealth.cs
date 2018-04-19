using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour {

	Soldier soldier;

	public GameObject group;

	public Image image;

	public float displayTime = 0.5f;

	void Start () {
		soldier = GetComponentInParent<Soldier> ();

		soldier.onHit += HandleOnHit;
		soldier.onSelect += Show;
		soldier.onDeselect += Hide;

		Hide ();
		UpdateUI ();
	}

	void HandleOnHit (Gun gun)
	{
		Show ();
		UpdateUI ();

		Tween.Bounce (transform);

		Invoke ("Hide", displayTime);
	}

	void Update () {
		transform.forward = (Camera.main.transform.position - transform.position).normalized;
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide() {
		group.SetActive (false);
	}

	// Update is called once per frame
	void UpdateUI () {
		image.fillAmount = (float)soldier.health / (float)100;
	}
}
