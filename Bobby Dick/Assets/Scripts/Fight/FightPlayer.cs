using UnityEngine;
using System.Collections;

public class FightPlayer : Humanoid {

	FightIA lastIAFight;
	[SerializeField]
	private float deadAxisX = 0.1f;
	[SerializeField]
	private float deadAxisY = 0.1f;

	public override void Start ()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	#region hit
	public override void hit_Start ()
	{
		base.hit_Start ();

		if ( lastIAFight != null ) {
			if ( Random.value < 0.5f )
				lastIAFight.ChangeState (states.guard);
		}
	}
	#endregion

	#region move

	public override void move_Update ()
	{
		base.move_Update ();

		bool pressingX = InputManager.Instance.GetHorizontalAxis () > deadAxisX|| InputManager.Instance.GetHorizontalAxis () < -deadAxisX;
//		bool pressingY = InputManager.Instance.GetVerticalAxis () > deadAxis || InputManager.Instance.GetVerticalAxis() < -deadAxis;

//		Animator.SetFloat ("move" , (pressingX || pressingY) ? 1 : 0);
		Animator.SetFloat ("move" , (pressingX) ? 1 : 0);

		transform.Translate ( Direction * InputManager.Instance.GetHorizontalAxis() * Speed * Time.deltaTime);

		if (PressHit())
			ChangeState (states.hit);

//		if (Input.GetKeyDown (KeyCode.F))
//			ChangeState (states.blocked);
//
//		if (Input.GetKeyDown (KeyCode.G))
//			ChangeState (states.getHit);

		if (PressGuard ())
			ChangeState (states.guard);
		
	}
	#endregion

	#region input
	private bool PressHit () {
		if ( InputManager.Instance.OnMobile || InputManager.Instance.mobileTest) {
			return InputManager.Instance.OnInputDown (0, InputManager.ScreenPart.Right) || InputManager.Instance.OnInputDown (1, InputManager.ScreenPart.Right);
		} else {
			return Input.GetKeyDown (KeyCode.D);
		}
	}
	public bool PressGuard () {
		return InputManager.Instance.GetVerticalAxis () < -deadAxisY;
//		if ( InputManager.Instance.OnMobile ) {
//			
//		} else {
//			return Input.GetKeyDown (KeyCode.DownArrow);
//		}
	}
	#endregion

	#region guard
	public override void guard_Update ()
	{
		base.guard_Update ();

		if (InputManager.Instance.GetVerticalAxis () > -0.3f)
			ChangeState (states.move);

	}
	#endregion

	#region collision

	public override void OnTriggerEnter2D (Collider2D other)
	{
		base.OnTriggerEnter2D (other);

		lastIAFight = other.GetComponentInParent<FightIA> ();


	}
	#endregion


}
