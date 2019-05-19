using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

[System.Serializable]
public class SoldierInfo
{
	public SoldierInfo () {
		
	}

	public string name = "";

    public int colorID = 0;

    public Color GetColor
    {
        get
        {
            return InventoryManager.Instance.soldierColors[colorID];
        }
    }

    public int shootRateLevel = 0;
    public int maxShootRateLevel = 10;
    public int speedBetweenLignsLevel = 0;
    public int maxSpeedBetweenLignsLevel = 10;

}

public class Soldier : Humanoid {

    public static Soldier selected;

    public SoldierInfo soldierInfo;

    // soldier info
	public float minShootRate = 1.5f;
    public float maxShootRate = 0.2f;

    public float shootRate = 0f;

    public float minSpeedBetweenLigns = 1f;
    public float maxSpeedBetweenLigns = 0.2f;

    public float minimumDistanceKill = 5f;

    public AudioSource source;

    public float goForthDecal = 20f;

    public float retreatDecal = -1f;

	Animator animator;

	public override void Start ()
	{
		base.Start ();

		animator = GetComponentInChildren<Animator> ();

        Zone.onGoForth += HandleOnGoForth;
        Zone.onRetreat += HandleOnRetreat;
    }

    private void HandleOnRetreat()
    {
        Vector3 p = rectTransform.position + Vector3.forward * retreatDecal;

        Move(p, 1f);
    }

    private void HandleOnGoForth()
    {
        Vector3 p2 = rectTransform.position + Vector3.forward * goForthDecal;

        Move(p2, 1f);

        Invoke("HandleOnGoForthDelay" , 1f+0.1f);
    }

    private void HandleOnGoForthDelay()
    {
        Vector3 p1 = rectTransform.position;
        p1.z = retreatDecal;

        rectTransform.position = p1;

        ChangeLign(currentLignIndex);
    }

    public void SetInfo (SoldierInfo soldierInfo)
    {
        this.soldierInfo = soldierInfo;

        //image.color = soldierInfo.GetColor;
        //initColor = soldierInfo.GetColor;

        float l1 = soldierInfo.shootRateLevel / soldierInfo.maxShootRateLevel;
        shootRate = Mathf.Lerp(minShootRate , maxShootRate, l1);

        float l2 = soldierInfo.speedBetweenLignsLevel / soldierInfo.maxSpeedBetweenLignsLevel;
        speedBetweenLigns = Mathf.Lerp(minSpeedBetweenLigns, maxSpeedBetweenLigns, l2);
    }

	public override void Update ()
	{
		base.Update ();
	}
	
    #region idle
	public override void Idle_Update ()
	{
		base.Idle_Update ();

		animator.SetBool ("Aiming", false);
		animator.SetFloat ("Move", 0f);

	}
	#endregion

	#region moving
	public override void Moving_Start ()
	{
		base.Moving_Start ();

		animator.SetFloat ("Move", 1f);

        image.color = Color.gray;

	}
    public override void Moving_Exit()
    {
        base.Moving_Exit();

        image.color = initColor;
    }
    #endregion

    #region shooting
    public override void Shooting_Start ()
	{
		base.Shooting_Start ();

		animator.SetBool ("Aiming", true);

		animator.SetTrigger ("Shoot");

		canShoot = false;

		image.color = Color.gray;

        Sound.Instance.PlaySound(source , Sound.Type.Shoot, 1.05f, 1.2f);
	}
	public override void Shooting_Update ()
	{
		base.Shooting_Update ();

		if ( timeInState > shootRate ) {
			ChangeState (State.Idle);
		}
	}
	public override void Shooting_Exit ()
	{
		base.Shooting_Exit ();

        image.color = initColor;

        Tween.Bounce (transform);
		canShoot = true;
	}
	bool canShoot = true;

    public virtual void Shoot ()
	{
		if (!canShoot)
			return;

        ChangeState (State.Shooting);

	}
    #endregion

    public virtual void HandleOnSwipe(Swipe.Direction direction)
    {
        if (state != State.Idle)
            return;

        int i = currentLignIndex;

        switch (direction)
        {
            case Swipe.Direction.Left:
                --i;
                i = Mathf.Clamp(i, 0, 3);
                ChangeLign(i);
                break;
            case Swipe.Direction.Right:
                ++i;
                i = Mathf.Clamp(i, 0, 3);
                ChangeLign(i);
                break;
            default:
                break;
        }

    }

}
