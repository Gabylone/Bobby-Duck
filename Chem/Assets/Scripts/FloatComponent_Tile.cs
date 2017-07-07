using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class FloatComponent_Tile : FloatComponent {

	public override void Start ()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void Float_Start ()
	{
		base.Float_Start ();

		Array.Instance.RemoveTile (new TileCoord ((int)transform.position.x,(int)transform.position.y));
	}

	public override void Kill ()
	{
		Vector2 p = transform.position;
		Vector3 tPos = (Vector3)Array.RoundToUnit (p);
		HOTween.To (transform , 1f , "position" , tPos , false , EaseType.EaseOutBounce , 0f );
		HOTween.To (transform , 1f , "rotation" , Quaternion.identity , false , EaseType.EaseOutBounce , 0f );

		Invoke ("KillDelay",1f);

	}

	void KillDelay () {
		Array.Instance.AddTile (Array.GetTileFromPos (transform.position), GetComponent<SpriteRenderer> ());
		base.Kill ();
	}

}
