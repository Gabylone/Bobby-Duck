using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;

using UnityEditor;

public class WorldTexture : MonoBehaviour {

    public RectTransform rectTransform;

    public float waitRate = 50;

    public Texture2D refTexture;

    public int regionScale = 50;

    public bool skipColor = false;

    public GameObject regionPrefab;

    public Transform regionParent;

    public Color capturedColor;

    public Color invadedColor;


    public void RecreateRegions()
    {
        InstantiateRegions();
    }

    public void InstantiateRegions ()
    {
        GameObject parent = new GameObject();
        parent.name = "RegionButtons";
        parent.transform.parent = regionParent;

        int w = refTexture.width / regionScale;

        int h = refTexture.height / regionScale;

        float difX = rectTransform.rect.width / refTexture.width;
        float difY = rectTransform.rect.height / refTexture.height;

        for (int regionX = 0; regionX < w + 1; regionX++)
        {
            for (int regionY = 0; regionY < h + 1; regionY++)
            {
                int landAmount = 0;

                Coords coords = new Coords(regionX, regionY);

				GameObject regionObj = PrefabUtility.InstantiatePrefab(regionPrefab) as GameObject;
                regionObj.transform.parent = parent.transform;
                RegionButton regionButton = regionObj.GetComponent<RegionButton>();
                regionButton.Place(regionX * regionScale * difX, regionY * regionScale * difY);
                regionButton.rectTransform.sizeDelta = new Vector2(regionScale * difX, regionScale * difY);
                regionButton.coords = coords;
                //regionButton.InitTexture(regionScale);

                for (int x = 0; x < regionScale; x++)
                {
                    for (int y = 0; y < regionScale; y++)
                    {
                        int pX = regionX * regionScale + x;
                        int pY = regionY * regionScale + y;

                        Color c = refTexture.GetPixel(pX, pY);

                        if (c.r > 0.9f && c.g > 0.9f && c.b > 0.9f)
                        {
                            continue;
                        }

                        if (pX >= refTexture.width || pY >= refTexture.height)
                            continue;


                        ++landAmount;

                    }
                }

                if (landAmount <= 0)
                {
                    DestroyImmediate(regionObj);
                }

            }

        }

    }

    public void ApplyTextures()
    {
        Texture2D[] texture2Ds = Resources.LoadAll<Texture2D>("RegionTextures");

        foreach (var item in regionParent.GetComponentsInChildren<RegionButton>())
        {
            string path = "RegionTextures/regionTexture" + item.coords.x + "x" + item.coords.y + "y";

            Texture2D tex = Resources.Load(path) as Texture2D;

            item.image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
    }

    public void RefreshTexture()
    {
        
    }
    public void CreateRegionTextures()
    {
        int w = refTexture.width / regionScale;

        int h = refTexture.height / regionScale;

        float difX = rectTransform.rect.width / refTexture.width;
        float difY = rectTransform.rect.height / refTexture.height;

        for (int regionX = 0; regionX < w + 1; regionX++)
        {
            for (int regionY = 0; regionY < h + 1; regionY++)
            {
                int landAmount = 0;

                Texture2D newTexture = new Texture2D(regionScale, regionScale );

                newTexture.filterMode = FilterMode.Point;
                newTexture.anisoLevel = 0;
                newTexture.mipMapBias = 0;
                newTexture.wrapMode = TextureWrapMode.Clamp;

                for (int x = 0; x < regionScale; x++)
                {
                    for (int y = 0; y < regionScale; y++)
                    {
                        int pX = regionX * regionScale + x;
                        int pY = regionY * regionScale + y;

                        Color c = refTexture.GetPixel(pX, pY);

                        if (c.r > 0.9f && c.g > 0.9f && c.b > 0.9f)
                        {
                            bool clear = true;
                            for (int iX = -1; iX < 2; iX++)
                            {
                                for (int iY = -1; iY < 2; iY++)
                                {
                                    Color c2 = refTexture.GetPixel(pX + iX, pY + iY);
                                    bool clearPixel = c2.r > 0.9f && c2.g > 0.9f && c2.b > 0.9f;
                                    bool blackPixel = c2.r < 0.1f && c2.g < 0.1f && c2.b < 0.1f;
                                    if (clearPixel == false && blackPixel == false)
                                    {
                                        clear = false;
                                        break;
                                    }
                                           
                                }

                                if (!clear)
                                {
                                    break;
                                }
                            }

                            if (clear)
                            {
                                newTexture.SetPixel(pX, pY, Color.clear);
                            }
                            else
                            {
                                newTexture.SetPixel(pX, pY, Color.black);
                            }

                            continue;
                        }

                        if (pX >= refTexture.width || pY >= refTexture.height)
                            continue;


                        if (x == 0 || y == 0 || x >= regionScale - 1 || y >= regionScale - 1)
                        {
                            newTexture.SetPixel(pX, pY, Color.black);
                        }
                        else
                        {
                            newTexture.SetPixel(pX, pY, Color.white);
                        }


                        ++landAmount;

                    }
                }

                if (landAmount > 0)
                {
                    string path = "Assets/Resources/RegionTextures/regionTexture" + regionX + "x" + regionY + "y.png";

                    newTexture.Apply();

                    byte[] bytes = newTexture.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);

                }

            }

        }

        AssetDatabase.Refresh();


    }
}
