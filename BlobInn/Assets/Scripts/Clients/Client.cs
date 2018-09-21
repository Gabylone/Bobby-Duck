using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Client : Movable {

    Blob_Apparence blob_Apparence;

    bool sitted = false;

    public static Sprite[] bodySprites = new Sprite[0];

    public static Sprite[] GetBodySprites
    {
        get
        {
            if (bodySprites.Length == 0)
            {
                bodySprites = Resources.LoadAll<Sprite>("Graphs/Clients/Client_BodySprites");
            }
            return bodySprites;
        }
    }



    public static Sprite[] clotheSprites = new Sprite[0];

    public static Sprite[] GetClotheSprites
    {
        get
        {
            if (clotheSprites.Length == 0)
            {
                clotheSprites = Resources.LoadAll<Sprite>("Graphs/Clients/Client_ClotheSprites");
            }
            return clotheSprites;
        }
    }

    public enum Type
    {
        Regular,
        Booze,
        Gourmet,
        Fisherman,  
        Old,
        Religious,
        Baby,
        Sport,
        Brunch,

        Prince,

        None,
    }

    public enum State
    {
        LookForTable,
        GoToTable,
        WaitForOrder,
        WaitForDish,
        Eating,
        WaitForBill,
        Leaving,

        None,
    }

    public State currentState = State.LookForTable;
    public State previousState;

    public delegate void UpdateState();
    public UpdateState updateState;

    OrderBubble orderBubble;

    public float timeInState = 0f;

    /// <summary>
    ///  table
    /// </summary>
    public Table targetTable;
    public float tableDecal = 0.3f;

    public float patienceTimer = 0f;
    public float maxPatienceTime = 2f;

    /// <summary>
    /// order
    /// </summary>
    public List<Ingredient.Type> order = new List<Ingredient.Type>();
    public int minOrderSize = 2;
    public int maxOrderSize = 5;
    public GameObject handObj;

    public List<Ingredient.Type> desiredIngredients = new List<Ingredient.Type>();


    /// <summary>
    /// eating
    /// </summary>
    public float eatingDuration = 10f;

    GameObject plateObj;

    /// <summary>
    /// bill
    /// </summary>
    public GameObject goldCoin_Prefab;
    public GameObject diamant_Prefab;
    GameObject payement_Obj;
    public float payement_FadeDuration = 0.3f;
    public float payement_FadeDecal = 1f;

    public float patience_WaitForTableMult = 2.5f;

    public Type type;

    public SpriteRenderer bodyRenderer;
    public SpriteRenderer clotheRenderer;

    bool payedBill = false;

    public static List<Client> clients = new List<Client>();

    public override void Start()
    {
        base.Start();

        Player.Instance.onMove += HandleOnPlayerMove;

        orderBubble = OrderBubbleManager.Instance.NewOrderBubble();
        orderBubble.Init(this);

        blob_Apparence = GetComponentInChildren<Blob_Apparence>();

        //GetComponentInChildren<Blob_Apparence>().Randomize();
        blob_Apparence.SetSprite(Blob_Apparence.Type.Eyes, 0);

        if ( type == Type.Prince)
        {
            blob_Apparence.SetSprite(Blob_Apparence.Type.Head, 5);
        }

        ChangeState(currentState);

        desiredIngredients = IngredientManager.Instance.GetDesiredIngredients(type);

        clients.Add(this);

        handObj.SetActive(false);

        UpdatePatience(0);

        /*bodyRenderer.sprite = GetBodySprites[(int)type];
        clotheRenderer.sprite = GetClotheSprites[(int)type];*/

        HandleOnPlayerMove();

    }

    private void OnDestroy()
    {
        Player.Instance.onMove -= HandleOnPlayerMove;
    }

    private void Update()
    {
        if (updateState != null)
        {
            updateState();

            timeInState += Time.deltaTime;
        }
    }

    bool under = false;
    bool changed = false;

    void HandleOnPlayerMove()
    {
        if (Player.Instance.coords.y+ 1 > coords.y)
        {
            if (under)
            {
                changed = true;
            }
            under = false;
        }
        else
        {
            if (!under)
            {
                changed = true;
            }

            under = true;
        }

        if (under)
        {
            blob_Apparence.LowerSortingOrder();
        }
        else
        {
            blob_Apparence.RaiseSortingOrder();
        }

        if ( targetTable != null)
        {
            targetTable.HandleOnPlayerMove();
        }

    }


    #region patience
    void UpdatePatience(float mult)
    {
        float lerp = patienceTimer / maxPatienceTime;

        Color spriteColor = Color.Lerp(ClientManager.Instance.patienceColor_Full, ClientManager.Instance.patienceColor_Mid, lerp * 2f);

        if (lerp > 0.5f)
        {
            spriteColor = Color.Lerp(ClientManager.Instance.patienceColor_Mid, ClientManager.Instance.patienceColor_Empty, (lerp - 0.5f) * 2f);
        }

        bodyRenderer.color = spriteColor;

        if (patienceTimer >= maxPatienceTime)
        {
            LosePatience();
        }
        else
        {
            patienceTimer += Time.deltaTime * mult;
        }
    }

    void LosePatience()
    {
        SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Client_Mad);

        ChangeState(State.Leaving);
    }
    #endregion

    #region movement
    public override void ReachTargetCoords()
    {
        base.ReachTargetCoords();

        switch (currentState)
        {
            case State.LookForTable:
                break;
            case State.GoToTable:
                ChangeState(State.WaitForOrder);
                break;
            case State.WaitForOrder:
                break;
            case State.WaitForDish:
                break;
            case State.Eating:
                break;
            case State.WaitForBill:
                break;
            case State.Leaving:
                Leave();
                break;
            case State.None:
                break;
            default:
                break;
        }
    }
    #endregion

    #region LookForTable
    void LookForTable_Start()
    {
        LookForAvailableTable();
    }
    void LookForTable_Update()
    {
        UpdatePatience(patience_WaitForTableMult);
    }
    void LookForTable_Exit()
    {

    }

    bool didntFindTable = false;
    public void LookForAvailableTable()
    {
        targetTable = Table.tables.Find(x => x.client == null);

        if ( targetTable != null)
        {
            targetTable.SetClient(this);
            ChangeState(State.GoToTable);
        }
    }

    void HandleOnClearTable()
    {
        LookForAvailableTable();

    }
    #endregion

    #region GoToTable
    void GoToTable_Start()
    {
        Coords c1 = new Coords(targetTable.coords.x + 1, targetTable.coords.y + 1);
        Coords c2 = new Coords(targetTable.coords.x, targetTable.coords.y + 1);

        GoToCoords(c1, c2);
    }
    void GoToTable_Update()
    {

    }

    void GoToTable_Exit()
    {

    }
    #endregion

    #region WaitForOrder
    void WaitForOrder_Start()
    {
        HOTween.To(transform, moveDuration, "position", transform.position - Vector3.up * tableDecal);

        sitted = true;

        handObj.SetActive(true);

        animator.SetBool("ordering", true);

        Tutorial.Instance.Show(TutorialStep.Order);

        HandleOnPlayerMove();
    }
    void WaitForOrder_Update()
    {
        UpdatePatience(1f);

    }
    void WaitForOrder_Exit()
    {
        handObj.SetActive(false);

        animator.SetBool("ordering", false);

    }

    public void TakeOrder()
    {
        int orderSize = Random.Range(minOrderSize, maxOrderSize);

        for (int i = 0; i < orderSize; i++)
        {
            Ingredient.Type newType = desiredIngredients[Random.Range(0, desiredIngredients.Count)];

            order.Add(newType);
        }


        ChangeState(Client.State.WaitForDish);
    }
    #endregion

    #region WaitForDish
    void WaitForDish_Start()
    {
        SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Client_Happy);
        Invoke("WaitForDish_StartDelay", 1f);
    }

    void WaitForDish_StartDelay()
    {
        Tutorial.Instance.Show(TutorialStep.Ingredients);
    }
    void WaitForDish_Update()
    {
        UpdatePatience(1f);

    }
    void WaitForDish_Exit()
    {

    }

    public void CheckForOrder(Waiter waiter)
    {
        List<Ingredient.Type> tmpOrder = new List<Ingredient.Type>();

        foreach (var item in order)
        {
            tmpOrder.Add(item);
        }

        if (waiter.CurrentPlate.ingredientTypes.Count != tmpOrder.Count)
        {
            orderBubble.WaitingForDish();
            return;
        }

        foreach (var item in waiter.CurrentPlate.ingredientTypes)
        {
            if ( tmpOrder.Contains(item))
            {
                tmpOrder.Remove(item);

                continue;
            }
        }

        if (tmpOrder.Count == 0)
        {

            plateObj = Instantiate(Plate_World.Current.group, targetTable.goldSackAnchor) as GameObject;

            Tween.Bounce(plateObj.transform);

            ChangeState(State.Eating);

            waiter.CurrentPlate.Clear();

            SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Plate);



        }
        else
        {
            orderBubble.WaitingForDish();
        }

    }
    #endregion

    #region Eating
    void Eating_Start()
    {


        ingredientsInPlate = plateObj.GetComponentsInChildren<Ingredient_World>(true).Length;
        ingredientEatenIndex = 0;

        Invoke("Eating_StartDelay", 1f);
    }

    void Eating_StartDelay()
    {
        Tutorial.Instance.Show(TutorialStep.Service);
    }

    int ingredientsInPlate = 0;
    int ingredientEatenIndex = 0;

    void Eating_Update()
    {

        float ingredientEatingDuration = eatingDuration / ingredientsInPlate;
        if (timeInState >= (ingredientEatingDuration * ingredientEatenIndex +1) && ingredientEatenIndex < ingredientsInPlate)
        {
            Ingredient[] ingredients = plateObj.GetComponentsInChildren<Ingredient_World>(true);

            ingredients[ingredients.Length-1- ingredientEatenIndex].gameObject.SetActive(false) ;

            ++ingredientEatenIndex;

            SoundManager.Instance.Play(movementAudioSource,SoundManager.SoundType.Eat);


        }

        if (timeInState >= eatingDuration)
        {
            ChangeState(State.WaitForBill);
        }
    }
    void Eating_Exit()
    {
        Tutorial.Instance.Show(TutorialStep.Gold);

    }
    #endregion

    #region WaitForBill
    void WaitForBill_Start()
    {
        Destroy(plateObj);

        GameObject prefab = type == Type.Prince ? diamant_Prefab : goldCoin_Prefab;
        payement_Obj = Instantiate(prefab, targetTable.goldSackAnchor) as GameObject;

        payement_Obj.transform.localPosition = Vector3.zero;

        Tween.Bounce(payement_Obj.transform);

        SoundManager.Instance.Play(audioSource, SoundManager.SoundType.MoneyBag);

    }
    void WaitForBill_Update()
    {
        //UpdatePatience();
    }
    void WaitForBill_Exit()
    {

    }

    public void TakeBill()
    {

        HOTween.To(transform, moveDuration, "position", transform.position + Vector3.up * tableDecal);

        Invoke("TakeBillDelay", moveDuration);

        Tween.Bounce(payement_Obj.transform);

        HOTween.To(payement_Obj.transform, payement_FadeDuration, "position", payement_Obj.transform.position + Vector3.up * payement_FadeDecal);

        foreach (var item in payement_Obj.GetComponentsInChildren<SpriteRenderer>())
        {
            HOTween.To(item, payement_FadeDuration, "color", Color.clear);
        }

        payedBill = true;

        SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Star);

    }

    void TakeBillDelay()
    {
        HandleOnPlayerMove();

        ChangeState(State.Leaving);

        Destroy(payement_Obj);

        SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Purchase);

        if ( type == Type.Prince)
        {
            Inventory.Instance.AddDiamonds(1);
        }
        else
        {
            if ( Inventory.multiplyGold)
            {
                Inventory.Instance.AddGold(LevelInfo.Instance.goldPerClient * 2);
            }
            else
            {
                Inventory.Instance.AddGold(LevelInfo.Instance.goldPerClient);
            }
        }
    }
    #endregion

    #region Leave
    void Leaving_Start()
    {
        if (sitted)
        {
            HOTween.To(transform, moveDuration, "position", transform.position + Vector3.up * tableDecal);
            sitted = false;

            Invoke("GoToDoor", moveDuration);
        }
        else
        {
            GoToDoor();
        }

        if (targetTable != null)
            targetTable.Clear();

        if (orderBubble != null)
        {
            OrderBubbleManager.Instance.UpdateLayoutGroup();
            Destroy(orderBubble.gameObject);
        }

        onChangeState = null;

        
    }

    void GoToDoor()
    {
        Invoke("PatienceTutorial", 0.7f);

        Coords c = new Coords(ClientManager.Instance.appearCoords.x - 1, ClientManager.Instance.appearCoords.y);

        GoToCoords(c , ClientManager.Instance.appearCoords);
    }
    void PatienceTutorial()
    {
        Tutorial.Instance.Show(TutorialStep.Patience1);
    }
    void Leaving_Update()
    {

    }
    void Leaving_Exit()
    {
        
    }
    public void Leave()
    {

        Door.Instance.Open();

        Invoke("LeaveMovement", Door.Instance.openTime);
        
    }

    void LeaveMovement()
    {
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            HOTween.To(spriteRenderer, moveDuration, "color", Color.clear);
        }

        animator.SetTrigger("move");

        HOTween.To(transform, moveDuration, "position", transform.position + Vector3.right * 1f);

        Invoke("LeaveDelay", moveDuration);

    }

    void LeaveDelay()
    {
        clients.Remove(this);
        Door.Instance.Close();

        if (payedBill == false)
        {
            Inventory.Instance.RemoveLife(1);
            ClientManager.Instance.lostClientAmount++;

            if (Inventory.Instance.lifes == 0)
            {
                DisplayEndOfDay.Instance.lost = true;
                DisplayEndOfDay.Instance.Open();
            }
        }

        if (clients.Count == 0)
        {
            if (LevelManager.Instance.dayEnded)
            {
                DisplayEndOfDay.Instance.Open();
            }
        }
        
        Destroy(gameObject);

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
            case State.LookForTable:
                LookForTable_Exit();
                break;
            case State.GoToTable:
                GoToTable_Exit();
                break;
            case State.WaitForOrder:
                WaitForOrder_Exit();
                break;
            case State.WaitForDish:
                WaitForDish_Exit();
                break;
            case State.Eating:
                Eating_Exit();
                break;
            case State.WaitForBill:
                WaitForBill_Exit();
                break;
            case State.Leaving:
                Leaving_Exit();
                break;
            default:
                break;
        }
    }

    private void EnterNewState()
    {
        switch (currentState)
        {
            case State.LookForTable:
                updateState = LookForTable_Update;
                LookForTable_Start();
                break;
            case State.GoToTable:
                updateState = GoToTable_Update;
                GoToTable_Start();
                break;
            case State.WaitForOrder:
                updateState = WaitForOrder_Update;
                WaitForOrder_Start();
                break;
            case State.WaitForDish:
                updateState = WaitForDish_Update;
                WaitForDish_Start();
                break;
            case State.Eating:
                updateState = Eating_Update;
                Eating_Start();
                break;
            case State.WaitForBill:
                updateState = WaitForBill_Update;
                WaitForBill_Start();
                break;
            case State.Leaving:
                updateState = Leaving_Update;
                Leaving_Start();
                break;
            default:
                break;
        }
    }
    #endregion
}
