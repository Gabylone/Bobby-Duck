using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ApparenceItem
{
    public Blob_Apparence.Type type;
    public List<int> ids = new List<int>();
}

public class ApparenceItemButton : MonoBehaviour {

    public int id = 0;

    public Image image;

    public Button button;

    public Blob_Apparence.Type type;

    public int price = 100;

    public GameObject lockedGroup;
    public Text diamond_Text;
    public Image diamond_Image;

    public GameObject diamondGroup;

    public void UpdateSprite ( Blob_Apparence.Type type , int id)
    {
        image.sprite = BlobApparenceManager.Instance.GetSprite(type, id);
        image.SetNativeSize();

        this.type = type;

        if (!Inventory.Instance.aquiredApparenceItems[(int)type].ids.Contains(id))
        {
            button.interactable = true;

            lockedGroup.SetActive(true);

            diamondGroup.SetActive(true);
            diamond_Text.text = "" + price;

            if( Inventory.Instance.diamonds < price)
            {
                diamond_Image.color = Color.red;
                diamond_Text.color = Color.white;
            }
            else
            {
                diamond_Text.color = Color.black;
                diamond_Image.color = Color.white;
            }

        }
        else
        {

            lockedGroup.SetActive(false);

            diamondGroup.SetActive(false);

            if (DisplayCharacterCustomization.Instance.blob_Apparence.ids[(int)type] == id)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }

        }

        
    }

    public void OnPointerClick()
    {
        Tween.Bounce(transform);

        if (!Inventory.Instance.aquiredApparenceItems[(int)type].ids.Contains(id))
        {
            DisplayApparenceItemPurchase.Instance.Display(type, id , price);
            return;
        }

        DisplayCharacterCustomization.Instance.blob_Apparence.ids[(int)type] = id;
        DisplayCharacterCustomization.Instance.blob_Apparence.UpdateSprites();

        Inventory.Instance.SaveBlobApparence();

        DisplayCharacterCustomization.Instance.UpdateGrid(type);
        BodyPartButton.current.UpdateSprite();


    }
}
