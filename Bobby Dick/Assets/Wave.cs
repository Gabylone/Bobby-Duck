using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    Transform transform;

    public float minimumX = 0f;
    public float minimumY = 0f;
    public float maximumX = 0f;
    public float maximumY = 0f;

    public float maxStartDelay = 2f;

    public float minScale = 1.2f;
    public float maxScale = 1.2f;

    private void Start()
    {
        NavigationManager.Instance.EnterNewChunk += HandleOnEnterNewChunk;

        transform = GetComponent<Transform>();

        transform.localScale = Vector3.one * Random.Range( minScale , maxScale );

        GetComponent<Animator>().enabled = false;

        Invoke("StartDelay", Random.Range(0,maxStartDelay));

        Move();
    }

    private void HandleOnEnterNewChunk()
    {
        Move();
    }

    void StartDelay()
    {
        GetComponent<Animator>().enabled = true;

    }

    public void Move()
    {
        transform.localPosition = new Vector3( Random.Range(minimumX,maximumX) , Random.Range(minimumY , maximumY) );
    }
}
