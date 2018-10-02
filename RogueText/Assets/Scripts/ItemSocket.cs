using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket
{
    public int count = 0;

    public Item item;

    public string GetWordGroup()
    {
        string text = "";

        if (count > 5)
        {
            text += "beaucoup " + item.word.GetDescription(Word.Def.Undefined, Word.Preposition.De, Word.Number.Plural);
        }
        else if (count > 3)
        {
            text += "quelques " + item.word.GetName(Word.Number.Plural);
        }
        else if (count > 1)
        {
            text +=
            count + " " + item.word.GetName(Word.Number.Plural);
        }
        else
        {
            text +=
            item.word.GetArticle(Word.Def.Undefined, Word.Preposition.None, Word.Number.Singular)
            + " " + item.word.GetName(Word.Number.Singular);
        }

        return text;
    }

    public static List<ItemSocket> GetItemSockets(List<Item> items)
    {
        List<ItemSocket> itemSockets = new List<ItemSocket>();

        foreach (var item in items)
        {
            ItemSocket itemSocket = itemSockets.Find(x => x.item == item);

            if (itemSocket == null)
            {
                itemSocket = new ItemSocket();

                itemSocket.item = item;

                itemSocket.count = 1;

                itemSockets.Add(itemSocket);

            }
            else
            {
                itemSocket.count++;
            }
        }

        return itemSockets;
    }

    public string GetItemPosition()
    {
        // si l'objet a une position prédéfinie dans la tile ( ex : armoire => prés du mur etc... )
        if (item.itemPositions.Count > 0)
        {
            return item.itemPositions[Random.Range(0, item.itemPositions.Count)];
        }
        // si le lieu a des phrases attitrées ( ex : salle de bain => évier , chambre => lit etc... )
        else if (Location.GetLocation(Tile.current.type).itemPositions.Count > 0)
        {
            return Location.GetLocation(Tile.current.type).GetItemPosition();
        }

        // prendre une position générique ( ex : à quelques pas , non loin etc... )
        return LocationLoader.Instance.positionPhrases[Random.Range(0, LocationLoader.Instance.positionPhrases.Length)];
    }


}