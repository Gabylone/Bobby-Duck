using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container
{
    public static bool opened = false;
    public static Container current;

    public List<Item> items = new List<Item>();

    public int id = 0;

    public Item item;

    public void DisplayItemDescription ()
    {
        string text = "";


        if ( items.Count == 0)
        {
            item.word.used = true;
            text = "Il n'y a plus rien dans " + Container.current.item.word.GetDescription(Word.Def.Defined);

        }
        else
        {
            text = "Dans " + Container.current.item.word.GetDescription(Word.Def.Defined) + ", vous voyez : ";

            text += Item.ItemListString(items, false , false);

        }

        text += "" +
            "\n" +
            "\n" +
            "Fermer " + Container.current.item.word.GetDescription(Word.Def.Defined) + " ?";

        DisplayDescription.Instance.Display(text);
    }

    public void GenerateItems()
    {
        foreach (var item in Item.items)
        {

            int id = Action.last.primaryItem.row;
            Item.AppearRate appearRate = item.appearRates.Find(x => x.type == Item.AppearRate.Type.Container && x.id == id);

            if (appearRate != null)
            {
                for (int i = 0; i < appearRate.amount; i++)
                {
                    if (Random.value * 100f < appearRate.rate)
                    {
                        items.Add(item);

                    }
                }

            }

        }
    }

    public void RemoveItem ( Item item)
    {
        items.Remove(item);

        Tile.itemsChanged = true;

        Debug.Log("removing container");
    }
}
         