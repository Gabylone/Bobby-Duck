using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public float decalMaxY = 0f;
    public float decalMinY = 0f;

    public float speed = 1f;

    public float minimumScale = 0;
    public float maximumScale = 0;

    public float minimumOrthographicSize = 0;
    public float maximumOrthographicSize = 0;

    public float decalToPlayer = 2f;

    // Use this for initialization
    private void Start()
    {

    }

    public float currentScale = 0;

    // Update is called once per frame
    void Update () {

        if (Player.Instance != null)
        {
            LerpCameraPosition();

        }
    }

    void UpdateCamPosition()
    {
        float y = Mathf.Clamp(Player.Instance.transform.position.y, Tile.minY+decalMinY, Tile.maxY + decalMaxY);

        transform.position = Vector3.up * (y + decalToPlayer);
    }

    void LerpCameraPosition()
    {

        /*if (Tile.minY + decalMinY < Tile.maxY + decalMaxY)
        {
            float y = Mathf.Clamp(Player.Instance.transform.position.y, Tile.minY + decalMinY, Tile.maxY + decalMaxY);

            transform.position = Vector3.MoveTowards(transform.position, Vector3.up * y, speed * Time.deltaTime);

        }*/

        float y = Mathf.Clamp(Player.Instance.transform.position.y, Tile.minY + decalMinY, Tile.maxY + decalMaxY);

        Vector3 tPos = Vector3.up * (y+decalToPlayer); 
        transform.position = Vector3.MoveTowards(transform.position, tPos, speed * Time.deltaTime);


    }
}
