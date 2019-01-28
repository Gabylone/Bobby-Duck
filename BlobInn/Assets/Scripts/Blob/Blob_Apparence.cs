using UnityEngine;

public class Blob_Apparence : MonoBehaviour {

    public enum Type
    {
        Head,
        Eyes,
        EyesAccessories,
        Color,

        None,
    }

    public SpriteRenderer bodySpriteRenderer;
    public SpriteRenderer[] spriteRenderers;

    public int[] ids = new int[(int)Type.None];

    public int initBodySortingOrder = 0;
    int[] initSortingOrders;
    public int spriteDecal = 5;

    private void Start()
    {
        ids = new int[(int)Type.None];

        initSortingOrders = new int[spriteRenderers.Length];
        int a = 0;
        foreach (var rend in spriteRenderers)
        {
            initSortingOrders[a] = rend.sortingOrder;

            ++a;
        }
        initBodySortingOrder = bodySpriteRenderer.sortingOrder;
    }

    public void LowerSortingOrder()
    {
        bodySpriteRenderer.sortingOrder = initBodySortingOrder;

        int a = 0;

        foreach (var rend in spriteRenderers)
        {
            rend.sortingOrder = initSortingOrders[a];

            ++a;
        }
    }

    public void RaiseSortingOrder()
    {
        bodySpriteRenderer.sortingOrder = initBodySortingOrder+ spriteDecal;

        int a = 0;

        foreach (var rend in spriteRenderers)
        {
            rend.sortingOrder = initSortingOrders[a] + spriteDecal;

            ++a;
        }
    }

    public void LoadFromIDs ( int[] _ids)
    {
        ids = _ids;

        UpdateSprites();
    }

    public void SetSprite(Type type, int id)
    {
        ids[(int)type] = id;

        UpdateSprites();
    }

    public void LoadFromInventory()
    {
        LoadFromIDs(Inventory.Instance.apparenceIDs);
    }

    public void Randomize()
    {
        for (int i = 0; i < (int)Type.None; i++)
        {
            if ( i == (int)Type.Head)
            {
                ids[i] = Random.Range(0, BlobApparenceManager.Instance.sprites[i].Length - 1);
            }
            else
            {
                ids[i] = Random.Range(0, BlobApparenceManager.Instance.sprites[i].Length);
            }
        }


        UpdateSprites();
    }

    public void UpdateSprites()
    {
        for (int i = 0; i < (int)4; i++)
        {
            if ( i >= ids.Length)
            {
                Debug.LogError("client update sprites : id " + i + " is superior than " + ids.Length);
                break;
            }

            SetSpriteRenderer((Type)i, ids[i]);
        }
    }

    public SpriteRenderer GetSpriteRenderer ( Type type)
    {
        return spriteRenderers[(int)type];
    }

    public void SetSpriteRenderer ( Type type, int id)
    {
        if ( type == Type.Color)
        {
            bodySpriteRenderer.color = BlobApparenceManager.Instance.blobColors[id];
        }
        else
        {
            spriteRenderers[(int)type].sprite = BlobApparenceManager.Instance.sprites[(int)type][id];
        }
    }
}
