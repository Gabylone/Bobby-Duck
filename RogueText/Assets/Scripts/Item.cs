using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public static List<Item> items = new List<Item>();

    public int row;

    public int weight;

    public int value;

    public Word word;
    Adjective adjective;
    public Adjective Adjective {
        get {
            if (adjective == null)
                SetAdjective(Adjective.GetRandom(Adjective.Type.Item));
            return adjective;
        }
    }
    public void SetAdjective ( Adjective adj)
    {
        adjective = adj;
    }

    public string GetWordGroup()
    {
        string adjStr = Adjective.GetName(word.genre, Word.Number.Singular);

        string wordStr = word.GetName(Word.Number.Singular);

        string wordGroup = wordStr + " " + adjStr;

        if (adjStr.Contains("("))
        {
            //Debug.Log("contains par, replacing");
            adjStr = adjStr.Replace("(", "");

            wordGroup = adjStr + " " + wordStr;
        }

        return wordGroup;
    }

    public bool usableAnytime = false;

    #region remove
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
        
        if ( Tile.current.items.Contains(item))
        {
            Tile.current.RemoveItem(item);
            return;
        }

        if (Inventory.Instance.items.Contains(item))
        {
            Inventory.Instance.RemoveItem(item);
            return;
        }
    }
    #endregion

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
    public static string ItemListString(List<Item> _items, bool separate , bool displayWeight)
    {
        string text = "";

        int i = 0;

        foreach (var item in _items)
        {
            text += item.GetWordGroup();

            if (displayWeight)
            {
                text += " (w:" + (item.weight) + ")";
            }

            if (_items.Count > 1 && i < _items.Count - 1)
            {
                if (separate)
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
    public static string ItemListString(List<ItemSocket> _itemSockets, bool separateWithLigns, bool displayWeight)
    {
        string text = "";

        int i = 0;

        foreach (var itemSocket in _itemSockets)
        {
            text += itemSocket.GetWordGroup();

            if (displayWeight)
            {
                text += " (w:" + (itemSocket.item.weight * itemSocket.count) + ")";
            }

            if (_itemSockets.Count > 1 && i < _itemSockets.Count - 1)
            {
                if ( separateWithLigns)
                {
                    text += "\n";
                }
                else
                {
                    if (_itemSockets.Count > 2)
                    {
                        if (i == _itemSockets.Count - 2)
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
            item = FindInTile(str);
        }

        if (item == null)
        {
            item = FindUsableAnytime(str);
        }

        if (item == null)
        {
            item = FindInInventory(str);
        }

        return item;
    }

    public static Item FindInTile(string str)
    {
        // with adjective
        //Item item = Tile.current.items.Find(x => x.GetWordGroup().StartsWith(str));

        List<Item> items = Tile.current.items.FindAll(x => x.word.name.StartsWith(str));

        if (items.Count == 1)
            return items[0];

        if ( items.Count > 1)
        {
            Debug.Log("found more than one item of the same type");

            foreach (var inputPart in DisplayInput.Instance.inputParts)
            {

                foreach (var item in items)
                {
                    Debug.Log("searhcing in input part : " + inputPart + " for adjective : " + item.Adjective.GetName(item.word.genre, Word.Number.Singular));

                    if (item.Adjective.GetName(item.word.genre, Word.Number.Singular) == inputPart)
                    {
                        Debug.Log("found matching adjective for item : " + item.GetWordGroup());
                        return item;
                    }
                }

                Debug.Log("no matching adjective for items");
                return items[0];

            }

        }

        return null;
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
        return GetInWords(words, -1);
    }

    public static Item GetInWords(string[] words, int forbiddenRow)
    {
        foreach (var part in words)
        {
            Item item = GetInWord(part);

            if (item != null && item.row != forbiddenRow)
            {
                return item;
            }
        }

        return null;
    }

    public static Item FindByName(string s)
    {
        Item item = items.Find(x => x.word.name.ToLower() == s.ToLower());

        if ( item == null ){
            Debug.LogError("couldn't find item by name : " + s);
            return null;
        }

        return item;
    }
    #endregion

    public string GetItemPosition()
    {
        // si l'objet a une position prédéfinie dans la tile ( ex : armoire => prés du mur etc... )
        if (itemPositions.Count > 0)
        {
            return itemPositions[Random.Range(0, itemPositions.Count)];
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

