using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayDiamonds : MonoBehaviour {

    public Text uiText;

    public RectTransform layoutGroup;

    public bool inTavern = false;

    public float tweenDuration = 0.5f;

    public float tweenDecal = 1f;

    Vector3 initPos = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        Inventory.Instance.onChangeDiamonds += HandleOnChangeDiamonds;

        initPos = transform.localPosition;

        UpdateUI();

    }

    private void HandleOnChangeDiamonds()
    {
        if (inTavern)
        {
            HOTween.Kill(transform);
            CancelInvoke();

            HOTween.To( transform , tweenDuration , "localPosition", initPos + Vector3.down * tweenDecal);

            Invoke("InTavern", tweenDuration);

            return;
        }

        UpdateUI();
        Tween.Bounce(transform);
    }

    void InTavern()
    {
        UpdateUI();
        Tween.Bounce(transform);

        Invoke("InTavernDelay", 1.5f);
    }

    void InTavernDelay()
    {
        HOTween.To(transform, tweenDuration, "localPosition", initPos);
    }

    void UpdateUI()
    {
        uiText.text = "" + Inventory.Instance.diamonds;
    }
}
