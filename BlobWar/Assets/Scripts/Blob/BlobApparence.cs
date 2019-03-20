using UnityEngine;

public class BlobApparence : MonoBehaviour {

    public enum Type
    {
		Body,
        Head,
        Eyes,
        EyesAccessories,
		Gun,
		Explosion,

        None,
    }

    public virtual void Start()
    {
        ids = new int[(int)Type.None];
        
    }

    public virtual void UpdateSprites()
    {
        
    }

    #region ids
    public int[] ids = new int[(int)Type.None];
    public void SetSpriteIDs(int[] _ids)
    {
        ids = _ids;

        UpdateSprites();
    }
    public void SetSpriteID(Type type, int id)
    {
        ids[(int)type] = id;

        UpdateSprites();
    }
    public void RandomizeIDs()
    {
        ids = GetRandomIDs();
    }
    public static int[] GetRandomIDs () {

        int[] _ids = new int[(int)BlobApparence.Type.None];

        for (int i = 0; i < 4; i++)
        {
            if (i == (int)Type.Head)
            {
                _ids[i] = Random.Range(0, BlobApparenceManager.Instance.sprites[i].Length - 1);
            }
            else
            {
                _ids[i] = Random.Range(0, BlobApparenceManager.Instance.sprites[i].Length);
            }
        }

        return _ids;

    }
    #endregion

}
