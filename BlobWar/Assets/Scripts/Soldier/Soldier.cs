using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Blob {

	public SpriteRenderer explosionRend;

	public float _shootTimer = 0f;

	public float shootRate = 0.2f;

	public float explosionDuration = 0.1f;

	public bool shooting = false;

	public override void Start ()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	public void ShootAnim() {

		animator.SetTrigger ("shoot");
		explosionRend.gameObject.SetActive (true);
		_shootTimer = shootRate;

		CancelInvoke ("ShootDelay");
		Invoke ("ShootDelay", explosionDuration);

		shooting = true;

	}

	void ShootDelay () {
		shooting = false;
		explosionRend.gameObject.SetActive (false);

	}

	public virtual void Shoot () {

		HitEnemy ();

	}


	void HitEnemy ()
	{
		List<Enemy> enemiesInLign = Enemy.enemies.FindAll(x => x.currentLignIndex == currentLignIndex);

		if (enemiesInLign.Count > 0)
		{
			Enemy closestEnemy = enemiesInLign[0];

			foreach (var enemy in enemiesInLign)
			{
				if (enemy._transform.position.y > closestEnemy._transform.position.y)
				{
					closestEnemy = enemy;
				}

			}

			closestEnemy.Hit();
		}

	}
}
