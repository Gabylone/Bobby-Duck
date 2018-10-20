using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket
{
    public int count = 0;

    public Item item;

    public string GetWordGroup()
    {
        Debug.Log("getting word group :=for item " + item.GetWordGroup());
        if (count > 5)
        {
            return "beaucoup " + item.word.GetDescription(Word.Def.Undefined, Word.Preposition.De, Word.Number.Plural);
        }
        else if (count > 3)
        {
            return "quelques " + item.word.GetName(Word.Number.Plural);
        }
        else if (count > 1)
        {
            return count + " " + item.word.GetName(Word.Number.Plural);
        }
        else
        {
            string article = item.word.GetArticle(Word.Def.Undefined, Word.Preposition.None, Word.Number.Singular);

            return article + " " + item.GetWordGroup();
        }
    }

    public static List<ItemSocket> GetItemSockets(List<Item> items)
    {
        List<ItemSocket> itemSockets = new List<ItemSocket>();

        foreach (var item in items)
        {
            //ItemSocket itemSocket = itemSockets.Find(x => x.item == item && x.item.adjective == item.adjective);
            ItemSocket itemSocket = null;

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


}