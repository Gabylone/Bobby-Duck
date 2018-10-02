using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;
using System;

public class Lign : MonoBehaviour {

	public int id = 0;
	public static List<Lign> ligns = new List<Lign>();

	public List<Zombie> zombies = new List<Zombie>();

	public Transform soldierAnchor;

	public Transform zombieAnchor;

    public Transform playerAnchor;

    public float distanceBetweenSoldiers_X = 50f;
    public float distanceBetweenSoldiers_Y = 50f;

	public float span = 1.5f;

    public List<Soldier> soldiers = new List<Soldier>();

    public GameObject swarmFeedback;
    public float swarmDuration = 2f;

    // Use this for initialization
    void Start()
    {

        ZombieSpawn.Instance.onKwakSwarm += ShowSwarmWarning;

        if (ligns.Count != 4)
        {
            for (int i = 0; i < 4; i++)
                ligns.Add(null);
        }

        ligns[id] = this;

        HideSwarmWarning();
    }


	public Zombie ClosestZombie () {

		Zombie closestZombie = zombies [0];

		foreach (var item in zombies) {

            if (closestZombie == null)
                continue;

			if (item.transform.position.z > closestZombie.transform.position.z) {
				closestZombie = item;
			}

		}

		return closestZombie;

	}

    public void AddSoldier(Soldier soldier)
    {
        soldiers.Add(soldier);

        UpdateSoldierPositions();
    }

    private void UpdateSoldierPositions()
    {
        int a = 0;
        int y = 0;

        while (a < soldiers.Count)
        {
            for (int i = 0; i <= 2; i++)
            {
                Vector3 decal = new Vector3( (distanceBetweenSoldiers_X/2f) + i * distanceBetweenSoldiers_X, y * distanceBetweenSoldiers_Y , 0f );

                Vector3 v = soldierAnchor.position + decal;
                soldiers[a].Move(v, 0.5f);


                ++a;

                if ( a == soldiers.Count)
                {
                    break;
                }

            }

            y++;
        }


        
    }

    public void RemoveSoldier ( Soldier soldier )
    {
        if (soldiers.Contains(soldier) == false)
            return;

        soldiers.Remove(soldier);
    }

    void ShowSwarmWarning( int i )
    {
        if (id != i)
            return;

        swarmFeedback.SetActive(true);
        Invoke("HideSwarmWarning",swarmDuration);
    }
    void HideSwarmWarning()
    {
        swarmFeedback.SetActive(false);
    }

}
