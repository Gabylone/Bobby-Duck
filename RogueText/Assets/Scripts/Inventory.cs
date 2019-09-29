using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory Instance;

    public int maxWeight = 15;

    public int weight = 0;

    public List<Item> items = new List<Item>();

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
        ActionManager.onAction += HandleOnAction;

        items.Add( Item.FindByName("boussole") );
    }

	void HandleOnAction (Action action)
	{
		switch (action.type) {
		case Action.Type.Take:
			PickUpCurrentItem ();
			break; 
		case Action.Type.DisplayInventory:
			DisplayInventory ();
			break;
        case Action.Type.CloseInventory:
            CloseInventory();
            break;
        case Action.Type.AddToInventory:
			AddToInventoryFromString ();
			break;
		case Action.Type.RemoveFromInventory:
			RemoveFromInventoryFromString ();
			break;
        case Action.Type.AddToTile:
            AddToTile();
            break;
        case Action.Type.RemoveFromTile:
            RemoveCurrentItemFromTile();
            break;
        case Action.Type.Require:
			CheckRequire ();
			break;
        case Action.Type.Throw:
            ThrowCurrentItem();
            break;
        case Action.Type.OpenContainer:
            OpenContainer();
            break;
        case Action.Type.CloseContainer:
            CloseContainer();
            break;
        case Action.Type.RemoveLastItem:
            RemoveLastItem();
            break;
        case Action.Type.ReplaceItem:
            ReplaceCurrentItem();
            break;
            default:
			break;
		}
	}

    public Item GetItem (string itemName)
    {
        return items.Find(x => x.word.name == itemName);
    }

    private void ThrowCurrentItem()
    {
        Item item = GetItem(Action.current.primaryItem.word.name);

        if (item == null)
        {
            DisplayFeedback.Instance.Display("Vous n'avez pas " + Action.current.primaryItem.word.GetDescription(Word.Def.Undefined));
            return;
        }

        RemoveItem(item);

        Tile.current.AddItem(Action.current.primaryItem);

        DisplayFeedback.Instance.Display("Vous posez " + Action.current.primaryItem.word.GetDescription(Word.Def.Defined) + " par terre");

    }

    void PickUpCurrentItem ()
	{
        if ( weight + Action.current.primaryItem.weight > maxWeight)
        {
            DisplayFeedback.Instance.Display(Action.current.primaryItem.word.GetName(Word.Number.Singular) + " est trop lourd pour le sac, il ne rentre pas");
            Debug.LogError("trop lourd ?");
            return;
        }

        Item.Remove(Action.current.primaryItem);

        AddItem(Action.current.primaryItem);

        DisplayFeedback.Instance.Display ("Vous avez pris : " + Action.current.primaryItem.word.GetName(Word.Number.Singular) );
	}

    #region remove item
    public void RemoveItem(int itemRow)
    {
        RemoveItem(items.Find(x => x.row == itemRow));
    }
    public void RemoveItem ( Item item ) {

        Tile.itemsChanged = true;

        weight -= item.weight;
        items.Remove(item);
	}
    void RemoveLastItem()
    {
        Item.Remove(Action.current.primaryItem);
    }

	void RemoveFromInventoryFromString ()
	{
		Item item = items.Find ( x => x.word.name.ToLower() == Action.current.contents[0].ToLower() );

		if (item == null) {
			Debug.LogError ("couldn't find item " + Action.current.contents[0] + " in inventory");
			return;
		}

		RemoveItem (item);
	}
    void RemoveCurrentItemFromTile()
    {
        Item item = Tile.current.items.Find(x => x.word.name.ToLower() == Action.current.contents[0].ToLower());

        if (item == null)
        {
            Debug.LogError("couldn't find item " + Action.current.contents[0] + " in inventory");
            return;
        }

        Tile.current.RemoveItem(item);
    }
	#endregion

	#region add
	public void AddItem ( Item item ) {
        weight += item.weight;
        items.Add(item);
    }

    void ReplaceCurrentItem()
    {
        Item item = AddToTile();

        item.SetAdjective(Action.current.primaryItem.GetAdjective);

        Item.Remove(Action.current.primaryItem);

    }

    void AddToInventoryFromString ()
	{
		Item item = Item.items.Find ( x => x.word.name.ToLower() == Action.current.contents[0].ToLower() );

		if (item == null) {
			Debug.LogError ("couldn't find item " + Action.current.contents[0] + " in item list");
			return;
		}

        int amount = 1;
        if ( Action.current.ints.Count > 0)
        {
            amount = Action.current.ints[0];
        }

        for (int i = 0; i < amount; i++)
        {
            AddItem(item);
        }
    }
    Item AddToTile()
    {
		Item item = Item.items.Find ( x => x.word.name.ToLower() == Action.current.contents[0].ToLower() );

        if (item == null)
        {
            Debug.LogError("couldn't find item " + Action.current.contents[0] + " in item list");
            return null;
        }

        int amount = 1;
        if (Action.current.ints.Count > 0)
        {
            amount = Action.current.ints[0];
        }

        for (int i = 0; i < amount; i++)
        {
            Tile.current.AddItem(item);
        }

        return item;
    }
    #endregion

    void CheckRequire ()
	{
		bool hasOnOfTheItems = false;

        foreach (var content in Action.current.contents) {

            /*Item item = Item.GetInWord(content);

			if ( item != null ){
                Debug.Log("found required item : " + item.word.name);
				hasOnOfTheItems = true;
				break;
			}*/

            if (Action.current.secundaryItem != null)
            {
                if (content.StartsWith(Action.current.secundaryItem.word.name))
                {
                    Debug.Log("found required item : " + Action.current.secundaryItem.word.name);
                    hasOnOfTheItems = true;
                    break;
                }
            }
        }

		if ( hasOnOfTheItems == false ) {

			ActionManager.Instance.BreakAction ();

            /*string phrase = "Peut pas : besoin de : ";

			foreach (var content in Action.last.contents) {
				phrase += content + ", ";
			}*/

            //DisplayFeedback.Instance.Display("Vous ne pouvez pas " + Action.last.verb.names[0] + " " + Action.last.primaryItem.word.GetDescription(Word.Def.Defined));
            //DisplayFeedback.Instance.Display("Vous avez besoin : " + Action.last.contents[0]);
        }
    }

    #region open / close inventory
    public bool opened = false;
    public void DisplayInventory ()
	{
		if ( items.Count == 0 ) {
			DisplayFeedback.Instance.Display ("Vous n'avez rien dans votre sac");
			return;
		}

        opened = true;

        string str = "Dans votre sac :\n\n";

        str += Item.ItemListString(items, true, true);

        str += "\n\nFermer le sac ?";

		DisplayDescription.Instance.Display (str);

	}
    void CloseInventory()
    {
        opened = false;
        DisplayDescription.Instance.DisplayTileDescription();
    }
    #endregion

    #region container
    private void OpenContainer()
    {
        Debug.Log("opening container");

        Container container = Tile.current.containers.Find(x => x.id == Action.current.primaryItem.row);

        if (container == null)
        {
            container = new Container();

            container.id = Action.current.primaryItem.row;

            container.item = Action.current.primaryItem;

            container.GenerateItems();

            Tile.current.containers.Add(container);

        }
        else
        {
        }

        Container.current = container;
        Container.opened = true;

        container.DisplayItemDescription();

    }

    private void CloseContainer()
    {
        if ( opened )
        {
            CloseInventory();
            return;
        }
        Container.opened = false;
        Container.current = null;

        DisplayDescription.Instance.DisplayTileDescription();
    }
    #endregion
}
