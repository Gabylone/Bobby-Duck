using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobApparenceManager : MonoBehaviour {

    public static BlobApparenceManager Instance;

    public string[] spritePaths;

    public Sprite[][] sprites;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        sprites = new Sprite[(int)Blob_Apparence.Type.None][];

        for (int i = 0; i < (int)Blob_Apparence.Type.None; i++)
        {
            sprites[i] = Resources.LoadAll<Sprite>(spritePaths[i]);
        } 

    }

    public Sprite GetSprite ( Blob_Apparence.Type type , int id)
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
