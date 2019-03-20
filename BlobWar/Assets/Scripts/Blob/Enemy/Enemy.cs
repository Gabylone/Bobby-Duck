using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Enemy : Blob {

    BlobApparence blob_Apparence;

	int maxHealth = 10;
	public int health = 10;

	public float hit_Duration = 0.2f;
	float moveBack_Decal = 1f;
	public float hit_Decal = 0.5f;
	public float hitBarricade_Decal = 0.12f;

	public float deathDuration = 0.1f;

    public float disappearDecal = 1f;

	public float speed = 1f;

	public struct Info {
		public string[] names;
	}
	public static Info[] infos;
	public static Info GetInfo ( Type type ) {
		return infos [(int)type];
	}

    public enum Type
    {
        Regular,
		FlagBlob,

		None,
    }

    public enum State
    {
        Moving,
		Hit,

        None,
    }

	public State currentState = State.Moving;
	public State previousState;

    public Type type;

    public SpriteRenderer bodyRenderer;

    public static List<Enemy> enemies = new List<Enemy>();

    public override void Start()
    {
        base.Start();

        blob_Apparence = GetComponentInChildren<BlobApparence>();
        blob_Apparence.SetSpriteID(BlobApparence.Type.Eyes, 0);

		if ( type == Type.FlagBlob)
        {
            blob_Apparence.SetSpriteID(BlobApparence.Type.Head, 5);
        }

        ChangeState(currentState);

        enemies.Add(this);

		maxHealth = health;

		UpdateRend ();

		Move (Swipe.Direction.None);


    }


	public override void Update ()
	{
		base.Update ();

    }

    #region moving
	void Moving_Start()
    {
        
    }
	void Moving_Update()
    {
		_transform.Translate ( Vector3.up * speed * Time.deltaTime );

        if (_transform.position.y >= disappearDecal)
        {
			ReachDefence ();
        }
	}
	void Moving_Exit()
    {

    }
    public override void Move(Swipe.Direction direction)
    {
		CurrentLign.RemoveEnemy(this);

		base.Move(direction);

		CurrentLign.AddEnemy (this);

		Tween.Bounce (transform);

		HOTween.To(_transform, moveDuration, "position", LignManager.Instance.ligns[currentLignIndex].enemyAnchor.position, false, move_EaseType, 0f);


    }

	void MoveBack (float decal )
	{
		moveBack_Decal = decal;

		Tween.Bounce (transform);
		ChangeState (State.Hit);
	}
    #endregion

	#region hit
	public void Hit ()
	{
		--health;

		MoveBack (hit_Decal);

		UpdateRend ();

		if ( health == 0 ) {
			Kill ();
			return;
		}


	}
	void Hit_Start()
	{

	}
	void Hit_Update()
	{
		_transform.Translate (Vector3.down * moveBack_Decal * Time.deltaTime);

		if ( timeInState >= hit_Duration ) {

			ChangeState (State.Moving);

		}
	}
	void Hit_Exit()
	{

	}

	void ReachDefence() {
		Inventory.Instance.RemoveLife(1);
		Inventory.Instance.Save ();

		if (Inventory.Instance.lifes == 0)
		{
			DisplayEndOfDay.Instance.lost = true;
			DisplayEndOfDay.Instance.Open();
		}

		Remove ();
	}

	public void Kill () {
		
		SoundManager.Instance.Play (audioSource, SoundManager.SoundType.Purchase);

		DisplayCharge.Instance.HandleOnEnemyKill ();

		if (type == Type.FlagBlob) {
			Inventory.Instance.AddDiamonds (1);
		} else {
			if (Inventory.multiplyGold) {
				Inventory.Instance.AddGold (LevelInfo.Instance.goldPerClient * 2);
			} else {
				Inventory.Instance.AddGold (LevelInfo.Instance.goldPerClient);
			}
		}

		Remove ();
	}

    void Remove()
    {
        Tween.Fade(transform, deathDuration);
        enemies.Remove(this);
		CurrentLign.RemoveEnemy(this);

		if (enemies.Count == 0)
		{
			if (LevelManager.Instance.dayEnded)
			{
				DisplayEndOfDay.Instance.Open();
			}
		}

        Destroy(gameObject, deathDuration);
        Destroy(this);
    }

	void UpdateRend ()
	{
		float f = (float)health / (float)maxHealth;

		bodyRenderer.color = Color.Lerp ( Color.red , Color.green , f);
	}
	void OnTriggerEnter2D ( Collider2D col ) {

		if (col.tag == "Barricade" && currentState == State.Moving)
		{
			Barricade barricade = col.GetComponent<Barricade> ();

			if (barricade.currentLignIndex == currentLignIndex && barricade.placing == false ) {

				barricade.Damage (1);

				MoveBack (hitBarricade_Decal);

			}
		}
	}
	#endregion

    #region state
    public delegate void OnChangeState(State newState);
    public OnChangeState onChangeState;
    public void ChangeState(State targetState)
    {
        previousState = currentState;
        currentState = targetState;

        EnterNewState();
        ExitPreviousState();

        timeInState = 0f;

        if ( onChangeState != null)
        {
            onChangeState( currentState );
        }
    }

    private void ExitPreviousState()
    {
        switch (previousState)
        {
			case State.Moving:
                Moving_Exit();
                break;
		case State.Hit:
			Hit_Exit();
			break;
            default:
                break;
        }
    }

    private void EnterNewState()
    {
        switch (currentState)
        {
		case State.Moving:
                updateState = Moving_Update;
                Moving_Start();
                break;
		case State.Hit:
			updateState = Hit_Update;
			Hit_Start();
			break;
            default:
                break;
        }
    }
    #endregion
}
