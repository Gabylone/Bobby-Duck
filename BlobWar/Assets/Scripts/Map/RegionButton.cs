using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System.IO;

public class RegionButton : MonoBehaviour {

    public Coords coords;

  

    public static Dictionary<Coords, RegionButton> regionButtons = new Dictionary<Coords, RegionButton>();

    public delegate void OnClickRegionButton(RegionButton regionButton);
    public static OnClickRegionButton onClickRegionButton;

    private void OnDestroy()
    {
        onClickRegionButton = null;
    }
    public Image image;
    public Button button;

    public RectTransform rectTransform;

    Transform parent;
    
    Texture2D texture;

    public bool selected = false;

    Vector2 initPos;

    public float decalUp = 200f;

    private void Start()
    {
        string path = "RegionTextures/regionTexture" + coords.x + "x" + coords.y + "y";

        //Texture2D tex = Resources.Load(path) as Texture2D;

        //image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

        UpdateDisplay();
    }

    public void OnClick()
    {
        if (!selected)
            Select();

		SoundManager.Instance.Play (SoundManager.SoundType.UI_Bip);
    }

    public void UpdateDisplay()
    {
       switch (region.state)
        {
            case Region.State.Invaded:
                //image.color = Color.green;
                float l = (float)region.level / Region.regions.Count;
                image.color = Color.Lerp( Color.green , Color.red , l );
                break;
            case Region.State.Conquered:
                image.color = Color.blue;
                button.interactable = false;
                UpdateSurroundings();
                break;
            case Region.State.Attacked:
                button.interactable = true;
                image.color = Color.red;
                break;
            default:
                break;
        }
    }

    public void UpdateSurroundings()
    {
        foreach (var item in Coords.allDirections)
        {
            Coords c = region.coords + (Coords)item;

            RegionButton regionButton = RegionButton.Get(c);

            if (regionButton == null)
                continue;

            if (regionButton.region.state == Region.State.Conquered)
            {
                regionButton.button.interactable = false;
                continue;
            }

            if (Tutorial.show && Tutorial.count < 1 && regionButton.region.level != 1)
                continue;

            regionButton.button.interactable = true;

        }
    }

    private void Select()
    {
        Region.selected = region;
		selected = true;

		initPos = rectTransform.anchoredPosition;

		Vector3 p = WorldMapScrollView.Instance.transform.position;

		DisplayRegion.Instance.Open ();


		parent = transform.parent;
		transform.SetParent ( DisplayRegion.Instance.regionButtonParent );

		rectTransform.localPosition = Vector3.zero;

        if ( onClickRegionButton != null)
        {
            onClickRegionButton(this);
        }

    }

    private static RegionButton Get(Coords c)
    {
        if (regionButtons.ContainsKey(c) == false)
            return null;

        return regionButtons[c];
    }

	public void Deselect()
    {
        rectTransform.SetParent(parent);

        Vector2 p = initPos;
        HOTween.To( rectTransform , 0.5f , "anchoredPosition" , p );

        selected = false;

        if (onClickRegionButton != null)
        {
            onClickRegionButton(this);
        }

        Region.selected = null;

    }

    #region texture
    public void InitTexture(int scale)
    {
        texture = new Texture2D(scale, scale);
        image.sprite = Sprite.Create(texture, new Rect(0, 0, scale, scale), Vector2.zero);
    }

    public void Place(float x, float y)
    {
        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public void Paint(int x, int y, Color color)
    {
        texture.SetPixel(x, y, color);
    }

    public void Refresh()
    {
        texture.Apply();

        UpdateDisplay();
    }
    #endregion

	public Region region
	{
		get
		{
			return Region.regions[coords];
		}
	}

	public static RegionButton GetSelected {
		get {
			return regionButtons [Region.selected.coords];
		}
	}
}
