using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment {

    public static Equipment Instance;

	public enum Part
    {
        Weapon,
        Head,
        Top,
        Bottom,
        Feet,
        Hands,
        Misc,

        None,
    }

    List<Item> items = new List<Item>();

    public void Init()
    {
        Instance = this;

        ActionManager.onAction += HandleOnAction;

        InitItems();
    }

    private void InitItems()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Part)).Length; i++)
        {
            //Debug.Log("init equipement part : " + (Part)i);

            items.Add(null);
        }
    }

    void HandleOnAction(Action action)
    {
        switch (action.type)
        {
            case Action.Type.Equip:
                Action_Equip();
                break;
            case Action.Type.Unequip:
                Action_Unequip();
                break;
            default:
                break;
        }
    }

    void Action_Equip()
    {
        Part part = GetPartFromString(Action.current.contents[0]);

        if ( GetEquipement(part) != null)
        {
            Inventory.Instance.AddItem(GetEquipement(part));
        }

        SetEquipment(part, Action.current.primaryItem);

        Item.Remove(Action.current.primaryItem);


        DisplayFeedback.Instance.Display("Vous avez équipé " + Action.current.primaryItem.word.GetDescription(Word.Def.Undefined) + " à " + part.ToString());

    }

    void Action_Unequip()
    {
        Part part = GetPartFromString(Action.current.contents[0]);

        if ( GetEquipement(part) != Action.current.primaryItem)
        {
            DisplayFeedback.Instance.Display("Vous n'avez pas " + Action.current.primaryItem.word.GetDescription(Word.Def.Defined, Word.Preposition.De) + " sur vous");
            return;
        }

        Inventory.Instance.AddItem(Action.current.primaryItem);

        DisplayFeedback.Instance.Display("Vous enlevez " + Action.current.primaryItem.word.GetDescription(Word.Def.Defined) );

        SetEquipment(part, null);
    }

    public Part GetPartFromString (string str)
    {
        Part part = Part.None;

        for (int i = 0; i < System.Enum.GetNames(typeof(Part)).Length; i++)
        {
            Part tmpPart = (Part)i;
            if (tmpPart.ToString().ToLower() == str)
            {
                Debug.Log("found part : " + tmpPart);
                part = tmpPart;
                break;
            }
        }

        if (part == Part.None)
        {
            Debug.LogError("did not find part in : " + str);
        }

        return part;
    }

    public void SetEquipment (Part part, Item item)
    {
        items[(int)part] = item;
    }

    public Item GetEquipement(Part part)
    {
        return items[(int)part];
    }


}
