using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartButton : MonoBehaviour {

    public Image image;

    public BlobApparence.Type type;

    public static BodyPartButton current;

    private void Start()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if ( type == BlobApparence.Type.Body)
        {
            int i = DisplayCharacterCustomization.Instance.blob_Apparence.ids[(int)type];
            GetComponent<Image>().color = BlobApparenceManager.Instance.blobColors[i];
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            image.sprite = BlobApparenceManager.Instance.GetSprite(type, DisplayCharacterCustomization.Instance.blob_Apparence.ids[(int)type]);
        }
    }

    public void OnPointerClick()
    {
        Tween.Bounce(transform);

        DisplayCharacterCustomization.Instance.ShowGrid(type);

        current = this;
    }
}
