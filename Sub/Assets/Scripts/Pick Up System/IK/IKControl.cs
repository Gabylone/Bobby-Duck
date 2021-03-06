﻿using UnityEngine;
using System.Collections;

public class IKControl : MonoBehaviour {

	[SerializeField]
	private Transform upperarm;
	[SerializeField]
	private Transform forearm;
	[SerializeField]
	private Transform hand;
	[SerializeField]
	private Transform target;

	[SerializeField]
	private float weight = 1.0f;
	[SerializeField]
	private float elbowAngle = 1f;

	private Transform armIK;
	private Transform armRotation;

	private float upperArmLength = 0f;
	private float forearmLength = 0f;
	private float armLength = 0f;

	float targetWeight = 0f;
	[SerializeField]
	private float speedToWeight = 1f;

	void Start () {
		var armIKGameObject = new GameObject("Arm IK");
		armIK = armIKGameObject.transform;
		armIK.parent = upperarm;

		var armRotationGameObject = new GameObject("Arm Rotation");
		armRotation = armRotationGameObject.transform;
		armRotation.parent = armIK;

		upperArmLength = Vector3.Distance(upperarm.position, forearm.position);
		forearmLength = Vector3.Distance(forearm.position, hand.position);
		armLength = upperArmLength + forearmLength;
	}

	public void ApplyArmsIK () {

		Quaternion storeUpperArmRotation = upperarm.rotation;
		Quaternion storeForearmRotation = forearm.rotation;

		//Upper Arm looks target.
		armIK.position = upperarm.position;
		armIK.LookAt(forearm);
		armRotation.position = upperarm.position;
		armRotation.rotation = upperarm.rotation;
		armIK.LookAt(target);
		upperarm.rotation = armRotation.rotation;

		//Upper Arm IK angle.
		float targetDistance = Vector3.Distance(upperarm.position, target.position);	
		targetDistance = Mathf.Min(targetDistance, armLength - 0.00001f);		
		float adjacent = ((upperArmLength*upperArmLength) - (forearmLength*forearmLength) + (targetDistance*targetDistance))/(2*targetDistance);
		float angle = Mathf.Acos(adjacent/upperArmLength) * Mathf.Rad2Deg;
		upperarm.RotateAround(upperarm.position, upperarm.forward, -angle);

		//Forearm looks target.
		armIK.position = forearm.position;
		armIK.LookAt(hand);
		armRotation.position = forearm.position;
		armRotation.rotation = forearm.rotation;
		armIK.LookAt(target);
		forearm.rotation = armRotation.rotation;

		//Elbow angle.
		upperarm.RotateAround(upperarm.position, target.position - upperarm.position, elbowAngle);

		//weight IK rotations with animation rotation.
		weight = Mathf.MoveTowards(weight, targetWeight, speedToWeight * Time.deltaTime);

		upperarm.rotation = Quaternion.Slerp(storeUpperArmRotation, upperarm.rotation, weight);
		forearm.rotation = Quaternion.Slerp(storeForearmRotation, forearm.rotation, weight);
	}

	public float TargetWeight {
		get {
			return targetWeight;
		}
		set {
			targetWeight = value;
		}
	}

	public Transform Hand {
		get {
			return hand;
		}
		set {
			hand = value;
		}
	}

	public Transform Target {
		get {
			return target;
		}
		set {
			target = value;
		}
	}
}
