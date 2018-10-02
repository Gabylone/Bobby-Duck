using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public static List<Item> items = new List<Item>();

    public int row;

    public int weight = 0;

    public int param1 = 0;

    public Word word;

    public bool usableAnytime = false;

    public static void Remove (Item item)
    {
        if (Container.opened )
        {
            if (Container.current.items.Contains(item))
            {
                Container.current.RemoveItem(item);
                //Container.current.DisplayItemDescription();
            }
            return;
        }

        if (Inventory.Instance.items.Contains(item))
        {
            Inventory.Instance.RemoveItem(item);
            return;
        }
        
        if ( Tile.current.items.Contains(item))
        {
            Debug.Log("removed " + item.word.name + " from tile");
            Tile.current.RemoveItem(item);
            return;
        }
    }

    // les positions sauvegardées de l'objet ( pour qu'il soit toujours au même endroit )
    public List<string> itemPositions = new List<string>();

    public class AppearRate
    {

        public enum Type
        {
            Container,
            Tile,
        }

        public Type type;

        public int id = 0;
        public int rate = 0;
        public int amount = 0;
    }

    public List<AppearRate> appearRates = new List<AppearRate>();



    #region list
    public static string ItemListString(List<ItemSocket> _itemSockets)
    {
        List<Item> tmpItems = new List<Item>();

        foreach (var itemSocket in _itemSockets)
        {
            tmpItems.Add(itemSocket.item);
        }

        return ItemListString(tmpItems, false, false);
    }

    public static string ItemListString(List<Item> _items)
    {
        return ItemListString(_items, false, false);
    }
    public static string ItemListString(List<Item> _items, bool separateWithLigns, bool displayWeight)
    {
        string text = "";


        int i = 0;

        foreach (var itemSocket in ItemSocket.GetItemSockets(_items) )
        {
            text += itemSocket.GetWordGroup();
            if (displayWeight)
            {
                text += " (w:" + (itemSocket.item.weight * itemSocket.count) + ")";
            }

            if (_items.Count > 1 && i < _items.Count - 1)
            {
                if ( separateWithLigns)
                {
                    text += "\n";
                }
                else
                {
                    if (_items.Count > 2)
                    {
                        if (i == _items.Count - 2)
                        {
                            text += " et ";
                        }
                        else
                        {
                            text += ", ";
                        }
                    }
                    else
                    {
                        text += " et ";
                    }
                }
               
            }

            ++i;
        }

        return text;
    }
    #endregion


    #region search
    public static Item GetInWord ( string str )
    {
        Item item = null;

        if (item == null)
        {
            Player.Facing facing = DisplaySurroundingTiles.Instance.GetFacingWithTile(str);

            if (facing != Player.Facing.None)
            {
                switch (facing)
                {
                    case Player.Facing.Current:
                        str = "porte";
                        break;
                    case Player.Facing.Front:
                        str = "devant";
                        break;
                    case Player.Facing.Right:
                        str = "droite";
                        break;
                    case Player.Facing.Left:
                        str = "gauche";
                        break;
                    default:
                        break;
                }
            }
        }

        if (Container.opened)
        {
            item = Container.current.items.Find(x => x.word.name.StartsWith(str));
        }

        if (item == null)
        {
            item = Item.FindInTile(str);
        }

        if (item == null)
        {
            item = Item.FindUsableAnytime(str);
        }

        if (item == null)
        {
            item = Item.FindInInventory(str);
        }

        return item;
    }

    public static Item FindInTile(string str)
    {
        return Tile.current.items.Find(x => x.word.name.StartsWith(str));
    }

    public static Item FindUsableAnytime(string str)
    {
        return items.Find(x => x.word.name.StartsWith(str) && x.usableAnytime);
    }

    public static Item FindInInventory(string str)
    {

        Item item = Inventory.Instance.items.Find(x => x.word.name.StartsWith(str.ToLower()));

        if (item == null)
            return null;

        return item;

    }

    public static Item GetInWords(string[] words)
    {
        foreach (var part in words)
        {
            Item item = GetInWord(part);

            if (item != null)
            {
                return item;
            }
        }

        return null;
    }

    public static Item FindByName(string s)
    {
        Item item = items.Find(x => x.word.name.ToLower() == s.ToLower());

        return item;
    }
    #endregion


}

