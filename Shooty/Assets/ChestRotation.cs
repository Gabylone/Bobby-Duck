using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRotation : MonoBehaviour {

	Transform target;

	public Vector3 decal = new Vector3();

	bool aiming = false;

	public void TurnToTarget ( Transform target ) {
		this.target = target;

		aiming = true;

	}

	public void Stop () {
		aiming = false;
	}

	// Update is called once per frame
	void LateUpdate () {

		if (aiming)
			AimUpdate ();
	}

	void AimUpdate ()
	{
		Vector3 dirToTarget = (transform.position - target.position).normalized;

		float difAngle = Vector3.Angle ( dirToTarget,transform.forward );

		float targetAngle = (difAngle + decal.x);

		if ( Vector3.Dot (dirToTarget,Vector3.up) > 0 ) {
			targetAngle = -(difAngle - decal.x);
		}

	}
}
