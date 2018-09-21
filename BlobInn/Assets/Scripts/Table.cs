using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : Interactable
{

    public static List<Table> tables = new List<Table>();

    public Client client;

    public SpriteRenderer tableSpriteRenderer;
    public SpriteRenderer tableNumberBGSpriteRenderer;
    public SpriteRenderer tableNumberSpriteRenderer;

    public Transform goldSackAnchor;

    public int id = 0;

    public GameObject group;

    public bool waited = false;

    int initA1 = 0;
    int initA2 = 0;
    int initA3 = 0;
    public int spriteDecal = 10;

    public override void Start()
    {
        base.Start();

        HandleOnPlayerMove();
    }

    public void Show()
    {
        group.SetActive(true);
    }

    public void Hide()
    {
        group.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            tableSpriteRenderer.sortingOrder = initA1; ;
            tableNumberBGSpriteRenderer.sortingOrder = initA2;
            tableNumberSpriteRenderer.sortingOrder = initA3;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            tableSpriteRenderer.sortingOrder = initA1 + spriteDecal;
            tableNumberBGSpriteRenderer.sortingOrder = initA2 + spriteDecal;
            tableNumberSpriteRenderer.sortingOrder = initA3 + spriteDecal;
        }
    }

    public void Init()
    {
        base.Start();

        Interactable.interactables.Add(coords, this);

        tables.Add(this);

        tableNumberSpriteRenderer.sprite = OrderBubbleManager.Instance.numberSprites[id];

        initA1 = tableSpriteRenderer.sortingOrder;
        initA2 = tableNumberBGSpriteRenderer.sortingOrder;
        initA3 = tableNumberSpriteRenderer.sortingOrder;

        Player.Instance.onMove += HandleOnPlayerMove;
    }


    bool under = false;
    bool changed = false;

    public void HandleOnPlayerMove()
    {

        if (Player.Instance.coords.y > coords.y)
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
            tableSpriteRenderer.sortingOrder = initA1; ;
            tableNumberBGSpriteRenderer.sortingOrder = initA2;
            tableNumberSpriteRenderer.sortingOrder = initA3;
        }
        else
        {
            tableSpriteRenderer.sortingOrder = initA1 + spriteDecal;
            tableNumberBGSpriteRenderer.sortingOrder = initA2 + spriteDecal;
            tableNumberSpriteRenderer.sortingOrder = initA3 + spriteDecal;
        }


    }

    public void SetClient(Client _client)
    {
        client = _client;
    }

    public void Clear()
    {
        client.targetTable = null;
        client = null;

        foreach (var client in Client.clients)
        {
            if ( client.currentState == Client.State.LookForTable)
            {
                client.LookForAvailableTable();
                return;
            }
        }
    }

    public override void Contact(Waiter waiter)
    {
        base.Contact(waiter);

        Tween.Bounce(transform);

        if (client != null)
        {
            switch (client.currentState)
            {
                case Client.State.LookForTable:
                    break;
                case Client.State.GoToTable:
                    break;
                case Client.State.WaitForOrder:
                    client.TakeOrder();
                    break;
                case Client.State.WaitForDish:
                    client.CheckForOrder(waiter);
                    break;
                case Client.State.Eating:
                    break;
                case Client.State.WaitForBill:
                    client.TakeBill();
                    break;
                case Client.State.Leaving:
                    break;
                case Client.State.None:
                    break;
                default:
                    break;
            }
        }
    }

    public delegate void OnTouchTable(Table table);
    public static OnTouchTable onTouchTable;

    bool trigger = false;

    private void OnMouseExit()
    {
        trigger = false;
    }

    private void OnMouseDown()
    {
        trigger = true;

        Invoke("OnMouseExit",0.1f);
    }

    private void OnMouseUp()
    {
        if ( trigger)
        {
            Select();
        }
    }

    public void EnterWait()
    {
        waited = true;

        tableNumberBGSpriteRenderer.color = Color.magenta;
    }

    public void ExitWait()
    {
        waited = false;

        tableNumberBGSpriteRenderer.color = Color.white;
    }

    void Select()
    {
        if (waited)
        {
            return;
        }

        foreach (var waiter in WaiterAI.waiterAIs)
        {
            if (!waiter.moving)
            {

                waiter.HandleOnTouchTable(this);
                Tween.Bounce(transform);

                break;
            }
        }
    }
}

