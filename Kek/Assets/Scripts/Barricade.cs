using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barricade : Placable {

    public Image brokenWallImage;

	public int health = 100;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        UpdateTexture ();
	}

    public override void Update()
    {
        base.Update();
    }

    public void Damage (int damage)
	{
		if (placing)
			return;

		health -= damage;

		Tween.Bounce (transform, 0.2f, 0.9f);

		UpdateTexture ();

		if ( health <= 0 ) {
			Destroy (gameObject);
		}
	}

	void UpdateTexture ()
	{
		Color c = new Color ();
		c.a = 1 - ( (float)health / 100f );
        brokenWallImage.color = c;
	}
}
