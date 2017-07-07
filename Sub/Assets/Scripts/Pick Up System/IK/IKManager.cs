using UnityEngine;
using System.Collections;

public class IKManager : MonoBehaviour {

	[Header ("leaning params")]
	[SerializeField]
	private bool enableLean = true;
	[SerializeField]
	private Transform backTransform;
	[SerializeField]
	private float backArchSpeed = 1f;

	private float initBackAngle = 90f;
	private float backAngle = 0f;
	private bool leaning = false;

	public float yToLean = 0.5f;

	[Header("IK Controls")]
	[SerializeField]
	private IKControl leftArmIK;
	[SerializeField]
	private IKControl rightArmIK;

	[SerializeField]
	private float minDistanceToCatch = 0.5f;
	[SerializeField]
	private float maxDistanceToCatch = 3f;

	[SerializeField]
	private float timeToMaxDistance = 1f;
	[SerializeField]
	private float speedToMaxDistance = 1f;

	float timer = 0f;

	Humanoid humanoid;
	Interactable targetInteractable;

	void Start () {
		humanoid = GetComponentInParent<Humanoid> ();
	}

	// Update is called once per frame
	void LateUpdate () {
		
		if ( enableLean )
			LeanToBall ();

		if (TargetInteractable != null) {

			leftArmIK.ApplyArmsIK ();
			rightArmIK.ApplyArmsIK ();
		}
	}

	#region leaning
	public float BackAngle {
		get {
			return backAngle;
		}
		set {
			backAngle = Mathf.Clamp (value , 0f, 90f);
		}
	}

	float currentDis = 0f;

	void LeanToBall () {

		if (leaning) {

			BackAngle += backArchSpeed * Time.deltaTime;

			backTransform.localEulerAngles = new Vector3 (0f, initBackAngle - BackAngle, 0f);

		} else if (BackAngle > 0) {
			BackAngle -= backArchSpeed * Time.deltaTime;

			backTransform.localEulerAngles = new Vector3 (0f, initBackAngle - backAngle, 0f);
		}

	}
	#endregion

	public void ApplyIK () {

		Vector3 directionToPlayer = (transform.position - TargetInteractable.transform.position).normalized;
		float d = Vector3.Dot ( directionToPlayer, humanoid.BodyTransform.right );

		IKControl ikControl = RightArmIK;
		if ( humanoid.PickableManager.RightHandPickable != null ) {
			ikControl = leftArmIK;
		}

		if ( Leaning ) {
			if (TargetInteractable.transform.position.y > yToLean + 0.2f) {
				Leaning = false;
			}
		} else {
			if (TargetInteractable.transform.position.y < yToLean) {
				Leaning = true;
			}
		}

		ikControl.TargetWeight = 1f;

		if ( timer >= timeToMaxDistance && currentDis < maxDistanceToCatch){ 
			currentDis += speedToMaxDistance * Time.deltaTime;
		} else {
			currentDis = minDistanceToCatch;
		}

		timer += Time.deltaTime;

		if (TargetInteractable.Available) { 
			if (Vector3.Distance (transform.position, ikControl.Hand.position) < currentDis) {
				TargetInteractable.Interact (humanoid);
			}
		}

	}

	public void RemoveIK () {

		Leaning = false;

		timer = 0f;

		LeftArmIK.TargetWeight = 0f;
		RightArmIK.TargetWeight = 0f;

	}

	public IKControl LeftArmIK {
		get {
			return leftArmIK;
		}
	}

	public IKControl RightArmIK {
		get {
			return rightArmIK;
		}
	}

	public bool Leaning {
		get {
			return leaning;
		}
		set {
			leaning = value;
		}
	}

	public Interactable TargetInteractable {
		get {
			return targetInteractable;
		}
		set {
			targetInteractable = value;

			LeftArmIK.Target = value.transform;
			RightArmIK.Target = value.transform;
		}
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;

		if ( TargetInteractable != null )
			Gizmos.DrawWireSphere (TargetInteractable.transform.position, currentDis);
	}
}
