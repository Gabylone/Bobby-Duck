using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlobApparence_UI : BlobApparence
{
    public Image[] images;

    public override void Start()
    {
        base.Start();
    }

    public override void UpdateSprites()
    {
        base.UpdateSprites();

        for (int i = 0; i < 4; i++)
        {
            if (i >= ids.Length)
            {
                Debug.LogError("client update sprites : id " + (Type)i + " is superior than " + ids.Length);
                break;
            }

            SetImage((Type)i, ids[i]);
        }

    }

    public Image GetImage(Type type)
    {
        return images[(int)type];
    }

    public void SetImage(Type type, int id)
    {
        if (type == Type.Body)
        {
            GetImage(Type.Body).color = BlobApparenceManager.Instance.blobColors[id];
        }
        else
        {
            GetImage(type).sprite = BlobApparenceManager.Instance.sprites[(int)type][id];
        }
    }
}
