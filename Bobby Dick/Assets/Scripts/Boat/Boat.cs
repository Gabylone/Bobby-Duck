﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class Boat : MonoBehaviour {

	public bool moving = false;

	[Space]
	[Header ("Boat Elements")]
	[SerializeField]
	private Transform boatMesh;
	public Transform getTransform;

	[Space]
	[Header ("Boat Position Parameters")]
	public float startSpeed = 5f;

    public Vector3 targetPos;
	private Vector2 targetDir;

    public NavMeshAgent agent;

	public virtual void Start () {
		
		getTransform = GetComponent<Transform> ();
        SetSpeed(startSpeed);

	}

	public virtual void Update () {
		if ( moving ) {

            if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
            {
                EndMovenent();
                Debug.Log("reached destination");
            }

            /*UpdateBoatPosition ();
			SetBoatRotation ();*/
		}
	}

	void CheckForBounds ()
	{
		Vector2 p = (Vector2)getTransform.position;

	}

    public float rotationSpeed = 10f;

	private void SetBoatRotation () {

		Vector3 targetDir = (targetPos - getTransform.position).normalized;

		float targetAngle = Vector3.Angle (targetDir, Vector3.forward);
		if (Vector3.Dot (Vector3.right, targetDir) < 0)
			targetAngle = -targetAngle;

		Quaternion targetRot = Quaternion.Euler (0, targetAngle, 0);

		boatMesh.localRotation = Quaternion.RotateTowards(boatMesh.localRotation , targetRot , rotationSpeed * Time.deltaTime );

	}

    #region moving
    public virtual void SetTargetPos(Vector3 p)
    {
        agent.SetDestination(p);

        moving = true;
        targetPos = p;
    }
    public virtual void SetTargetPos ( Transform t ) {
        SetTargetPos(t.position);
	}

	/*private void UpdateBoatPosition () {

		Vector3 targetDir = (targetPos - getTransform.position).normalized;

		// translate boat
		getTransform.Translate (targetDir * speed * Time.deltaTime, Space.World);

	}*/

	public virtual void EndMovenent() {
		moving = false;
	}
	#endregion

	#region map position 
	public virtual void UpdatePositionOnScreen () {
		foreach (TrailRenderer renderer in GetComponentsInChildren<TrailRenderer>())
			renderer.Clear ();
	}
    public void SetSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
    }
    #endregion

}
