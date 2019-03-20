using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Holoville.HOTween;

public class Lign : MonoBehaviour {

	public int id = 0;

	public Transform playerAnchor;
	public Transform enemyAnchor;
	public Transform soldierAnchor;

	public int soldierPerLign = 1;

	public float distanceBetweenSoldiers_X = 0.2f;
	public float distanceBetweenSoldiers_Y = 0.5f;

	public float moveDuration = 0.5f;

	public List<SoldierAI> soldiers = new List<SoldierAI>();
	public List<Enemy> enemies = new List<Enemy>();



	void Start () {

		UpdateSoldierAnchors ();

	}

	#region soldiers
	public void AddEnemy(Enemy enemy)
	{
		enemies.Add(enemy);

	}
	public void RemoveEnemy ( Enemy enemy )
	{
		if (enemies.Contains(enemy) == false)
			return;

		enemies.Remove(enemy);
	}
	#endregion

	#region soldiers
	public void AddSoldier(SoldierAI soldier)
	{
		soldiers.Add(soldier);
	}
	public void RemoveSoldier ( SoldierAI soldier )
	{
		if (soldiers.Contains(soldier) == false)
			return;

		soldiers.Remove(soldier);
	}

	public void UpdateSoldierAnchors()
	{
		int a = 0;
		int y = 0;

		while (a < soldiers.Count)
		{
			for (int i = 0; i < soldierPerLign; i++)
			{
				//Vector3 decal = new Vector3( (distanceBetweenSoldiers_X/2f) + i * distanceBetweenSoldiers_X, y * distanceBetweenSoldiers_Y , 0f );
				Vector3 decal = new Vector3( y % 2 == 1 ? -distanceBetweenSoldiers_X : distanceBetweenSoldiers_X , -y * distanceBetweenSoldiers_Y , 0f );
				Vector3 v = soldierAnchor.position + decal;

				HOTween.To(soldiers[a]._transform, moveDuration, "position", v);

				soldiers [a].apparence.SetRenderingOrder (y);

				++a;

				if ( a == soldiers.Count)
				{
					break;
				}

			}


			y++;
		}

		++y;

		playerAnchor.position = soldierAnchor.position + new Vector3( 0, -y * distanceBetweenSoldiers_Y, 0f );




	}


	#endregion
}
