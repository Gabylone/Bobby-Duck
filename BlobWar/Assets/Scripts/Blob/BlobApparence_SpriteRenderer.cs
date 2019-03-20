using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobApparence_SpriteRenderer : BlobApparence {

    public SpriteRenderer[] spriteRenderers;

	public int minSpriteOrderLayer = 10;
    public int initBodySortingOrder = 0;
    private int[] initSortingOrders;

    public override void Start()
    {
        base.Start();

        initSortingOrders = new int[spriteRenderers.Length];
        int a = 0;
        foreach (var rend in spriteRenderers)
        {
            initSortingOrders[a] = rend.sortingOrder;

            ++a;
        }
        initBodySortingOrder = GetSpriteRenderer(Type.Body).sortingOrder;
    }

    public SpriteRenderer GetSpriteRenderer(Type type)
    {
        return spriteRenderers[(int)type];
    }

    public void SetSpriteRenderer(Type type, int id)
    {
        if (type == Type.Body)
        {
            GetSpriteRenderer(Type.Body).color = BlobApparenceManager.Instance.blobColors[id];
        }
        else
        {
            GetSpriteRenderer(type).sprite = BlobApparenceManager.Instance.sprites[(int)type][id];
        }
    }

    public void SetRenderingOrder(int i)
    {
        int a = minSpriteOrderLayer + i * (int)Type.None;

        foreach (var sprite in spriteRenderers)
        {
            sprite.sortingOrder = a;

            ++a;
        }

    }

    public override void UpdateSprites()
    {
        base.UpdateSprites();

        for (int i = 0; i < (int)4; i++)
        {
            if (i >= ids.Length)
            {
                Debug.LogError("client update sprites : id " + (Type)i + " is superior than " + ids.Length);
                break;
            }

            SetSpriteRenderer((Type)i, ids[i]);
        }
    }
}
