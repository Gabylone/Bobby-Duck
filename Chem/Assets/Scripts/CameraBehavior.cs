using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public static CameraBehavior Instance;

	public Vector2 cameraDecal = Vector2.zero;
	public Vector2 buffer;

    public int x = 0;
    public int y = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
//		CameraTrigger.touchBorder += MoveCam;
		transform.position = CenterCam (Character.Instance.getTransform.position);
        MoveCam(Border.None);
	}

	void Update () {
		Vector3 playerPos = 	Character.Instance.getTransform.position;

		if ( playerPos.x >= (transform.position.x+(cameraDecal.x/2f)+buffer.x) ) {
			MoveCam (Border.Right);
		}

		if ( playerPos.x <= (transform.position.x-(cameraDecal.x/2f)-buffer.x) ) {
			MoveCam (Border.Left);
		}

		if ( playerPos.y >= (transform.position.y+(cameraDecal.y/2f)+buffer.y) ) {
			MoveCam (Border.Top);
		}

		if ( playerPos.y <= (transform.position.y-(cameraDecal.y/2f)-buffer.y) ) {
			MoveCam (Border.Bottom);
		}
	}

	void MoveCam (Border border)
	{
		switch (border) {
		case Border.Top:
			transform.position += new Vector3 (0f,cameraDecal.y);
			break;
		case Border.Right:
			transform.position += new Vector3 (cameraDecal.x,0f);
			break;
		case Border.Bottom:
			transform.position += new Vector3 (0f,-cameraDecal.y);
			break;
		case Border.Left:
			transform.position += new Vector3 (-cameraDecal.x,0f);
                break;
            case Border.None:
                break;
		default:
			break;
		}

        if ( onCamMove != null )
        {
            onCamMove(Coords.GetWorldCoords(transform.position) );
        }
	}

    public delegate void OnCamMove(Coords newCoords);
    public OnCamMove onCamMove;

	Vector2 CenterCam ( Vector2 pos )
	{
		float x = Mathf.Round(pos.x / cameraDecal.x) * cameraDecal.x;
		float y = Mathf.Round(pos.y / cameraDecal.y) * cameraDecal.y;




        return new
			Vector2(x , y);
	}


	#region Grid
	[Header("camera gizmos")]
	[SerializeField]
	private Color gridColor;
	[SerializeField]
	private Color bufferColor;
	[SerializeField]
	private bool gridActive = true;
	[SerializeField]
	private int gridScale = 5;
	void OnDrawGizmos()
	{
		Gizmos.color = gridColor;

		if (gridActive)
		{
			Vector3 center = cameraDecal / 2f;

			for (int i = -gridScale; i < gridScale+1; ++i)
			{
				// Horizontal //
				float x = i * cameraDecal.x;
				float y = gridScale * cameraDecal.y;
				Gizmos.DrawLine(center + new Vector3(x,y,0f) , center + new Vector3(x,-y,0f) );
			}

			for (int i = -gridScale; i < gridScale+1; ++i)
			{
				// Horizontal //
				float x = gridScale * cameraDecal.x;
				float y = i * cameraDecal.y;
				Gizmos.DrawLine(center + new Vector3(x,y,0f) , center + new Vector3(-x,y,0f) );
			}
		}

		Gizmos.color = bufferColor;

		Gizmos.DrawWireCube ( transform.position , (Vector3)(cameraDecal+(buffer*2)) );

	}
	#endregion

}

public class Coords
{
    public int x = 0;
    public int y = 0;

    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Coords GetWorldCoords(Vector2 pos)
    {
        int x = Mathf.RoundToInt(pos.x / CameraBehavior.Instance.cameraDecal.x);
        int y = Mathf.RoundToInt(pos.y / CameraBehavior.Instance.cameraDecal.y);

        return new Coords(x, y);
    }
}
