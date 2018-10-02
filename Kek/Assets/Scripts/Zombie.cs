using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Holoville.HOTween;

public class Zombie : Humanoid {

	public float speed = 1f;

	public float attackDuration = 0.8f;

	public float attackDecal  = 2f;

	public int damage = 5;

	public float scale = 0.5f;

    public float appearDelay = 2f;

    public float decalToLign = 50f;

    public Transform spriteTransform;

    public override void Start()
    {
        base.Start();

        initColor = image.color;

        Invoke("AppearDelay", appearDelay);

        Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
        Tutorial.onHideTutorial += HandleOnHideTutorial;

        DisplayHealth.onPlayerFailure += HandleOnPlayerFailure;

        if ( Random.value < 0.5f)
        {
            spriteTransform.localScale = new Vector3( -1 , 1 , 1 );
        }

        
    }

    public override void StartDelay()
    {
        base.StartDelay();

        ChangeLign(currentLignIndex);
    }

    private void HandleOnPlayerFailure()
    {
        ChangeState(State.None);
    }

    private void OnDestroy()
    {
        Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
        Tutorial.onHideTutorial -= HandleOnHideTutorial;

        DisplayHealth.onPlayerFailure -= HandleOnPlayerFailure;
    }
    private void HandleOnHideTutorial()
    {
        ChangeState(State.Idle);
    }

    private void HandleOnDisplayTutorial(TutoStep tutoStep)
    {
        Invoke("HandleOnDisplayTutorialDelay",0.001f);
    }
    void HandleOnDisplayTutorialDelay()
    {
        ChangeState(State.None);
    }

    void AppearDelay()
    {
        CurrentLign.zombies.Add(this);
    }

    public override void Update ()
	{
		base.Update ();
	}

    public override void Move(Vector3 v, float t)
    {
        v.x += Random.Range ( -decalToLign , decalToLign );

        base.Move(v, t);
    }

    #region idle
    public override void Idle_Update ()
	{
		base.Idle_Update ();

		rectTransform.Translate (Vector3.up * speed * Time.deltaTime, Space.World);

        if (rectTransform.anchoredPosition.y >= 0)
        {
            ReachPlayer();
        }
	}

    public delegate void OnReachPlayer();
    public static OnReachPlayer onReachPlayer;
    /*private void OnDestroy()
    {
        onReachPlayer = null;
        onZombieKill = null;
    }*/
    private void ReachPlayer()
    {
        if (onReachPlayer != null)
            onReachPlayer();

        Kill();
    }

    public override void ChangeLign (int i)
	{
		base.ChangeLign (i);

		float r = Random.Range (-Lign.ligns [currentLignIndex].span + scale , Lign.ligns [currentLignIndex].span - scale);

		Vector3 p = Lign.ligns [currentLignIndex].zombieAnchor.position + Vector3.right * r;

        Move(p, speedBetweenLigns);


	}
	#endregion

	#region attacking
	public override void Attacking_Start ()
	{
		base.Attacking_Start ();

		HOTween.To (rectTransform, attackDuration , "position" , rectTransform.position - Vector3.up * attackDecal , false , EaseType.EaseOutBounce , 0f );
	}
	public override void Attacking_Update ()
	{
		base.Attacking_Update ();

		if ( timeInState >= attackDuration ){
			ChangeState (State.Idle);
		}
	}
    #endregion

    public delegate void OnZombieKill();
    public static OnZombieKill onZombieKill;

    public AudioSource source;

    public override void Kill()
    {
        base.Kill();

        List<Sound.Type> types = new List<Sound.Type>();
        for (Sound.Type type = Sound.Type.Zombie1; type <= Sound.Type.Zombie13; ++type)
        {
            types.Add(type);
        }

        Sound.Instance.PlaySound(source, types);

        CurrentLign.zombies.Remove(this);

        if (onZombieKill != null)
            onZombieKill();
    }

    void OnTriggerEnter2D ( Collider2D col ) {

		if (col.tag == "Barricade" && state != State.Attacking)
		{
			Barricade barricade = col.GetComponent<Barricade> ();

			if (barricade.currentLignIndex == currentLignIndex && barricade.placing == false ) {
				barricade.Damage (damage);

				ChangeState (State.Attacking);

			}
		}
	}
}
