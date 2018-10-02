using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class Tutorial : MonoBehaviour {

    public static bool show = true;

    public static int count = 0;

	public TextAsset tutoData;

	public bool debugTutorial = true;

	public delegate void OnDisplayTutorial ( TutoStep tutoStep);
	public static OnDisplayTutorial onDisplayTutorial;

	public delegate void OnHideTutorial ();
	public static OnHideTutorial onHideTutorial;

	public delegate void OnWaitForConfirm ();
	public static OnWaitForConfirm onWaitForConfirm;

	private TutoStep[] tutoSteps;

	void Start () {

        if (SaveTool.Instance.FileExists("inventory"))
        {
            show = false;
        }

        RegionManager.onStartMap += HandleOnStartMap;
    }

    private void HandleOnStartMap()
    {
        if (show)
        {
            InitTutorials();
            LoadData();
            RegionManager.onStartMap -= HandleOnStartMap;
            Invoke("GreetingsDelay", 1.5f);
        }
        
    }

    void GreetingsDelay()
    {
        tutoSteps[(int)TutorialStep.Greetings].Display();
        tutoSteps[(int)TutorialStep.Greetings].WaitForConfirm();
    }


    void LoadData ()
	{
		string[] rows = tutoData.text.Split ('&');

		int tutoIndex = 0;

		for (int i = 0; i < rows.Length-1; i++) {

            string row = rows[i].TrimStart('\r', '\n');

            string[] cells = row.Split (';');

			tutoSteps [tutoIndex].title = cells [0];
			tutoSteps [tutoIndex].description = cells [1];

			++tutoIndex;
		}
	}

	void InitTutorials ()
	{
		int stepCount = System.Enum.GetValues (typeof(TutorialStep)).Length;

		tutoSteps = new TutoStep[stepCount];

		for (int i = 0; i < stepCount; i++) {

			string classRef = "TutoStep_" + (TutorialStep)i;

			Type tutoClass = Type.GetType (classRef);

			TutoStep newTutoStep = System.Activator.CreateInstance (tutoClass) as TutoStep;

			newTutoStep.Init ();
			newTutoStep.step = (TutorialStep)i;

			tutoSteps [i] = newTutoStep;

		}

		LoadData ();

	}

}

public enum TutorialStep {

	Greetings,
    Territories,
    Battlefield,
    Enemies,
    Barricade,
    InventoryCharge,
    ItemPlacement,
    Soldiers,
    SoldierPlacement,
    CounterAttack,
    Goodbye,

}

public class TutoStep {

	public TutorialStep step;

	public DisplayInfo.Corner corner = DisplayInfo.Corner.None;

	public string title = "";

	public string description = "";

	public void Display () {
		//
		Tutorial.onDisplayTutorial ( this );

        Tutorial.count++;

        Debug.Log("displaying : " + title);

	}

	public virtual void Init () {

	}

	public void WaitForConfirm () {
		Tutorial.onWaitForConfirm ();


    }
}

public class TutoStep_Greetings : TutoStep
{
    
}

public class TutoStep_Territories: TutoStep {

	public override void Init ()
	{
		base.Init ();

        Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;

    }

    private void HandleOnDisplayTutorial(TutoStep tutoStep)
    {
        if ( tutoStep.step == TutorialStep.Greetings)
        {
            Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
            Tutorial.onHideTutorial += HandleOnHideTutorial;
        }
    }

    private void HandleOnHideTutorial()
    {
        Tutorial.onHideTutorial -= HandleOnHideTutorial;

        Display();
        WaitForConfirm();
    }
}

public class TutoStep_Battlefield : TutoStep
{
    public override void Init()
    {
        base.Init();

        ZoneManager.onStartZones += HandleEvent;
    }

    private void HandleEvent()
    {

        ZoneManager.onStartZones -= HandleEvent;

        Display();
        WaitForConfirm();
    }
}

public class TutoStep_Enemies : TutoStep
{
    public override void Init()
    {
        base.Init();

        Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;

    }

    private void HandleOnDisplayTutorial(TutoStep tutoStep)
    {
        if (tutoStep.step == TutorialStep.Battlefield)
        {
            Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
            Tutorial.onHideTutorial += HandleOnHideTutorial;
        }
    }

    private void HandleOnHideTutorial()
    {
        Tutorial.onHideTutorial -= HandleOnHideTutorial;

        Display();
        WaitForConfirm();
    }
}

public class TutoStep_Barricade : TutoStep
{
    int count = 0;

    public override void Init()
    {
        base.Init();

        RegionManager.onStartMap += HandleEvent;
    }

    void HandleEvent()
    {
        if ( count == 0)
        {
            Display();
            WaitForConfirm();

            Debug.Log("handly map");

            RegionManager.onStartMap -= HandleEvent;

        }

        ++count;
    }
}
public class TutoStep_InventoryCharge : TutoStep
{
    int count = 0;

    public override void Init()
    {
        base.Init();

        ZoneManager.onStartZones += HandleEvent;
    }

    private void HandleEvent()
    {

        if (count > 0)
        {
            Display();
            WaitForConfirm();

            ZoneManager.onStartZones -= HandleEvent;

        }

        ++count;
    }
}

public class TutoStep_ItemPlacement: TutoStep
{
    int count = 0;

    public override void Init()
    {
        base.Init();

        Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;

    }

    private void HandleOnDisplayTutorial(TutoStep tutoStep)
    {
        if (tutoStep.step == TutorialStep.InventoryCharge)
        {
            Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
            Zombie.onZombieKill += HandleOnZombieKill;
        }
    }

    private void HandleOnZombieKill()
    {
        if ( count == 3)
        {
            Zombie.onZombieKill -= HandleOnZombieKill;
            Display();
            WaitForConfirm();
        }

        ++count;
    }
}

public class TutoStep_Soldiers : TutoStep
{
    int count = 0;

    public override void Init()
    {
        base.Init();

        RegionManager.onStartMap += HandleEvent;
    }

    void HandleEvent()
    {
        if (count == 1)
        {
            Display();
            WaitForConfirm();

            RegionManager.onStartMap -= HandleEvent;

        }

        ++count;
    }
}

public class TutoStep_SoldierPlacement : TutoStep
{
    int count = 0;

    public override void Init()
    {
        base.Init();

        ZoneManager.onStartZones += HandleEvent;
    }

    private void HandleEvent()
    {
        if ( count == 2)
        {
            ZoneManager.onStartZones -= HandleEvent;

            Display();
            WaitForConfirm();
        }

        ++count;
    }
}

public class TutoStep_CounterAttack : TutoStep
{
    public override void Init()
    {
        base.Init();

        WorldMapScrollView.Instance.onCounterAttack += HandleEvent;
    }

    void HandleEvent()
    {
        Display();
        WaitForConfirm();

        WorldMapScrollView.Instance.onCounterAttack -= HandleEvent;
    }
}


public class TutoStep_Goodbye : TutoStep
{
    int count = 0;

    public override void Init()
    {
        base.Init();

        RegionManager.onStartMap += HandleEvent;
    }

    void HandleEvent()
    {
        if (count == 2)
        {
            Display();
            WaitForConfirm();

            RegionManager.onStartMap -= HandleEvent;

        }

        ++count;
    }
}