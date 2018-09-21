using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartButton : MonoBehaviour {

    public Image image;

    public Blob_Apparence.Type type;

    public static BodyPartButton current;

    private void Start()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        image.sprite = BlobApparenceManager.Instance.GetSprite(type, DisplayCharacterCustomization.Instance.blob_Apparence.ids[(int)type]);
    }

    public void OnPointerClick()
    {
        Tween.Bounce(transform);

        DisplayCharacterCustomization.Instance.ShowGrid(type);

        current = this;
    }
}
