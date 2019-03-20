using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobApparenceManager : MonoBehaviour {

    public static BlobApparenceManager Instance;

    public string[] spritePaths;

    public Color[] blobColors;

    public Sprite[][] sprites;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        sprites = new Sprite[(int)BlobApparence.Type.None][];

        for (int i = 0; i < spritePaths.Length; i++)
        {
                sprites[i] = Resources.LoadAll<Sprite>(spritePaths[i]);
        }

    }

    public Sprite GetSprite ( BlobApparence.Type type , int id)
    {
        if (id >= sprites[(int)type].Length)
        {
            Debug.Log("id : " + id + " is too much for " + type + " L : " + sprites[(int)type].Length);
            return null;
        }
        if ((int)type >= sprites.Length)
        {
            Debug.Log("type : " + type+ " is too much L : " + sprites[(int)type].Length);
            return null;
        }
        return sprites[(int)type][id];
    }
}
