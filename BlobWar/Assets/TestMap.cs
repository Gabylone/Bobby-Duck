using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMap : MonoBehaviour {

    public Image image;
    public Texture2D refTexture;

    public int framesToWait = 10;

    public int origin_X = 0;
    public int origin_Y = 0;

    public double radius = 5;

    Color color;

    public float speed = 1f;

    // Use this for initialization
    void Start () {

        //StartCoroutine(TestCoroutine());

	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Eh();
        }

    }

    void Eh()
    {
        

    }
}
