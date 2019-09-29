using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DisplayTileItems : TextTyper {

	public override void Start ()
	{
		base.Start ();

		Player.onPlayerMove += HandleOnPlayerMove;

		ActionManager.onAction += HandleOnAction;

	}

	void HandleOnAction (Action action)
	{
        // called every time an action is made
        Invoke("UpdateUI", 0.01f);
    }

    void UpdateUI()
    {
        if (Tile.itemsChanged)
        {
            if (Container.current != null)
            {
                Container.current.DisplayItemDescription();
            }
            else if (Inventory.Instance.opened)
            {
                Inventory.Instance.DisplayInventory();
            }
            else
            {
                UpdateAndDisplay();
            }

            Tile.itemsChanged = false;

        }
    }

    void HandleOnPlayerMove (Coords prevCoords, Coords newCoords)
	{
        //UpdateCurrentTileDescription();
	}

    public override void UpdateCurrentTileDescription()
    {
        base.UpdateCurrentTileDescription();

        if (Container.opened)
        {
            Container.current.DisplayItemDescription();
            return;
        }

        DisplayItems();
    }

    public class Phrase
    {
        public List<ItemSocket> itemSockets = new List<ItemSocket>();

        public string itemPosition = "";

        public string GetText()
        {
            string itemText = Item.ItemListString(itemSockets, false ,false );

            // phrases de vision ( vous voyez, remarquez etc... )
            string visionPhrase = LocationLoader.Instance.visionPhrases[Random.Range(0, LocationLoader.Instance.visionPhrases.Length)];
            // phrases de location ( se trouve, se tient etc ... )
            string locationVerbs = LocationLoader.Instance.locationPhrases[Random.Range(0, LocationLoader.Instance.locationPhrases.Length)];

            // PHRASE ORDER 

            // le type de la phrase ( noms, verbe de vision et position de l'objet
            // , ou position de l'objet, verbe de location et noms
            // etc... )
            int phraseType = Random.Range(0, 5);

            string text = "";

            switch (phraseType)
            {
                case 0:
                    text = itemText + " " + locationVerbs + " " + itemPosition;
                    break;
                case 1:
                    text = itemPosition + " " + locationVerbs + " " + itemText;
                    break;
                case 2:
                    text = itemPosition + ", " + visionPhrase + " " + itemText;
                    break;
                case 3:
                    text = visionPhrase + " " + itemPosition + " " + itemText;
                    break;
                case 4:
                    text = itemPosition + ", " + itemText;
                    break;
                default:
                    break;
            }

            // mettre la phrase en majuscule
            return TextManager.WithCaps(text);


        }
    }

    public void DisplayItems ()
	{
		Clear ();

		if (Tile.current.items.Count == 0) {
            //Display("Il n'y a rien à voir");
            Display("");
            return;
		}

        // item AND item counts
        List<ItemSocket> itemSockets = ItemSocket.GetItemSockets(Tile.current.items);

        List<Phrase> phrases = new List<Phrase>();

        // dans l'évier... près du mur... etc...
        List<string> itemPositions = new List<string>();
 
        for (int i = 0; i < itemSockets.Count; i++)
        {
            ItemSocket itemSocket = itemSockets[i];

            // retourne la phrase de position appropriée
            string itemPosition = itemSocket.item.GetItemPosition();

            if (itemPosition.StartsWith("relative"))
            {
                itemPosition = GetRelativeItemPositionPhrase(itemSocket.item.word.name);
            }

            // si la position a déjà été trouve ( pour éviter : près du mur, une armoire, près de mur, une fenêtre )
            // et donc addictioner les noms ( près du mur, une armoire ET une fenêtre )
            Phrase matchingPhrase = phrases.Find(x => x.itemPosition == itemPosition);

            if (matchingPhrase != null)
            {
                matchingPhrase.itemSockets.Add(itemSocket);
                continue;
            }


            Phrase newPhrase = new Phrase();

            newPhrase.itemSockets.Add(itemSocket);
            newPhrase.itemPosition = itemPosition;

            phrases.Add(newPhrase);
            
        }

        string text = "";

        // afficher toutes les phrases
        int a = 0;
        foreach (var phrase in phrases)
        {
            text += phrase.GetText();

            if (a < itemSockets.Count - 1)
            {
                text += "\n";
            }

            ++a;
        }

        textToType = text;
        //Display(text);
	}

    string GetRelativeItemPositionPhrase (string itemName)
    {
        string itemPosition = "";
        char dirChar = itemName[itemName.Length - 2];

        Player.Facing fac = Player.Facing.None;

        switch (dirChar)
        {
            case 'n':
                fac = Player.Instance.GetFacing(Direction.North);
                break;
            case 'e':
                fac = Player.Instance.GetFacing(Direction.East);
                break;
            case 's':
                fac = Player.Instance.GetFacing(Direction.South);
                break;
            case 'w':
                fac = Player.Instance.GetFacing(Direction.West);
                break;
            default:
                break;
        }

        switch (fac)
        {
            case Player.Facing.Front:
                itemPosition = "devant vous";
                break;
            case Player.Facing.Right:
                itemPosition = "à droite";
                break;
            case Player.Facing.Back:
                itemPosition = "derrière vous";
                break;
            case Player.Facing.Left:
                itemPosition = "à gauche";
                break;
            default:
                break;
        }

        return itemPosition;
    }
}
