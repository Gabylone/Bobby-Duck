using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharge : MonoBehaviour {

	public static DisplayCharge Instance;

    public GameObject lockGroup;

    public GameObject rayblockerGroup;

    public RectTransform backGroundRectTransform;

    public RectTransform fillRectTransform;
    public Image fillImage;

   //public Color color1;
   //public Color color2;

    public int killCount = 0;
    public int killsToReload = 3;

    bool filling = false;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		rayblockerGroup.SetActive (false);

		ResetCharge ();

    }

	public void HandleOnEnemyKill()
    {
        if (killCount == killsToReload)
            return;

        ++killCount;

        UpdateImage();
    }

    private void HandleOnClickSoldierInventoryButton(SoldierInfo soldierInfo)
    {
        ResetCharge();
    }
    private void HandleOnClickBarricadeInventoryButton()
    {
        ResetCharge();
    }

	public void ResetCharge()
    {
        killCount = 0;
        rayblockerGroup.SetActive(true);

        UpdateImage();
    }

    private void UpdateImage()
    {
        if (killCount == killsToReload)
        {
			Debug.Log ("eh");
            Tween.Bounce(transform);
            rayblockerGroup.SetActive(false);

        }
		else{



		}

        float w = backGroundRectTransform.rect.width;

        //float l = currentLoad / chargeReloadDuration;
		float l = (float)killCount/ (float)killsToReload;

        //fillImage.color = Color.Lerp(color1, color2, l);
        

		fillRectTransform.sizeDelta = new Vector2( - w + l * w , fillRectTransform.sizeDelta.y );
		//fillRectTransform.sizeDelta = new Vector2( l , fillRectTransform.sizeDelta.y );
    }
}
