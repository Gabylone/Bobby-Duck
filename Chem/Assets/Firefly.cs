using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : MonoBehaviour {

    public float startSpeed = 5f;
    public float endSpeed = 2f;

    public float timeToEndSpeed = 10f;

    public float currentSpeed = 0f;

    public bool drawPath = false;

    Transform _transform;

    public GameObject ingredientPrefab;

    public delegate void UpdateState();
    public UpdateState updateState;

    public float timeInState = 0f;

    public float distanceToChase = 5f;
    public float distanceToLosePlayer = 10f;

    public float maxRotSpeed = 10f;
    public float minRotSpeed = 2f;

    Transform[] wayPoints;
    public Transform wayPointParent;

    public float distanceToNextWayPoint = 1f;

    public float distanceToCatch = 1f;
    public float deccelerationSpeed = 2f;

    Rigidbody2D rigidBody;

    public float catchForce = 100f;

    int wayPointID = 0;

    public enum State
    {
        None,

        Idle,
        Chased,

        Dead,
    }

    public State currentState = State.Idle;
    public State previousState = State.None;

    // Use this for initialization
    void Start()
    {
        _transform = GetComponent<Transform>();

        rigidBody = GetComponent<Rigidbody2D>();

        Transform[] tmpsTr = wayPointParent.GetComponentsInChildren<Transform>();

        wayPoints = new Transform[tmpsTr.Length -1];
        for (int i = 1; i < tmpsTr.Length; i++)
        {
            wayPoints[i-1] = tmpsTr[i];
        }

        SetState(currentState);



    }
	
	// Update is called once per frame
	void Update () {

       if ( updateState != null)
        {
            updateState();
        timeInState += Time.deltaTime;
        }



    }

    #region idle
    void Idle_Start()
    {

    }
    void Idle_Update()
    {
        if (Vector2.Distance(_transform.position, Character.Instance.getTransform.position) < distanceToChase)
        {
            Debug.Log("CHASING !!!!!");
            SetState(State.Chased);
        }
    }
    void Idle_Exit()
    {

    }
    #endregion

    #region chased
    void Chased_Start()
    {
        currentSpeed = startSpeed;

        Vector2 dirToNextWayPoint = (wayPoints[wayPointID].position - _transform.position).normalized;
        _transform.right = dirToNextWayPoint;

    }
    void Chased_Update()
    {
        Vector2 dirToNextWayPoint = (wayPoints[wayPointID].position - _transform.position).normalized;

        _transform.right = Vector3.MoveTowards(_transform.right, dirToNextWayPoint, Mathf.Lerp(maxRotSpeed, minRotSpeed, timeInState / timeToEndSpeed) * Time.deltaTime);

        currentSpeed = Mathf.Lerp(startSpeed, endSpeed, timeInState / timeToEndSpeed);

        _transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);

        if (Vector2.Distance(_transform.position, wayPoints[wayPointID].position) < distanceToNextWayPoint)
        {
            ++wayPointID;

            if (wayPointID == wayPoints.Length)
            {
                wayPointID = 0;
            }
        }

        if (Vector2.Distance(_transform.position, Character.Instance.getTransform.position) > distanceToLosePlayer)
        {
            Debug.Log("joueur semé");
            SetState(State.Idle);
        }

        if (Vector2.Distance(_transform.position, Character.Instance.getTransform.position) < distanceToCatch)
        {
            Debug.Log("attrapé ");
            SetState(State.Dead);
        }
    }
    void Chased_Exit()
    {

    }
    #endregion

    #region dead
    void Dead_Start()
    {
        /*
        rigidBody.isKinematic = false;
        rigidBody.AddForce(((Vector2)Character.Instance.bodyTransform.right + Vector2.up) * catchForce);
        */

        GameObject obj = Instantiate(ingredientPrefab, transform.position, Quaternion.identity) as GameObject;

        Destroy(gameObject);


    }
    void Dead_Update()
    {

    }
    void Dead_Exit()
    {

    }
    #endregion

    void SetState ( State state)
    {
        previousState = currentState;
        currentState = state;

        timeInState = 0f;

        switch (currentState)
        {
            case State.Idle:

                updateState = Idle_Update;
                Idle_Start();
                break;
            case State.Chased:
                updateState = Chased_Update;
                Chased_Start();
                break;
            case State.Dead:
                updateState = Dead_Update;
                Dead_Start();
                break;
            default:
                break;
        }

        switch (previousState)
        {
            case State.Idle:
                Idle_Exit();
                break;
            case State.Chased:
                Chased_Exit();
                break;
            case State.Dead:
                Dead_Exit();
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
                Gizmos.color = Color.yellow;
        if ( drawPath)
        {
            Transform[] tmpsTr = wayPointParent.GetComponentsInChildren<Transform>();
            Transform[] tmpWayPoints = new Transform[tmpsTr.Length - 1];

            for (int i = 1; i < tmpsTr.Length; i++)
            {
                tmpWayPoints[i - 1] = tmpsTr[i];
            }

            for (int i = 0; i < tmpWayPoints.Length; i++)
            {
                int index = i + 1;
                if (index == tmpWayPoints.Length)
                {
                    index = 0;
                }

               

                Gizmos.DrawLine(tmpWayPoints[i].position, tmpWayPoints[index].position);
             


            }
        }

        if (currentState == State.Idle)
        {
            Gizmos.DrawWireSphere(transform.position, distanceToChase);
        }
        else
        {
        Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanceToCatch);
        }


        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distanceToLosePlayer);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanceToNextWayPoint);

    }
}
