using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawn : MonoBehaviour {

    int zombieCount = 0;

    public static ZombieSpawn Instance;

	public GameObject zombiePrefab;

	public Transform parent;

    public float spawnRange = 50f;

    public float minRate = 0.1f;
    public float maxRate = 2f;
	public float rate = 1f;

    public float swarmMinRecurence = 10f;
    public float swarmMaxRecurence = 30f;
    float currentSwarmRecurence = 0f;
    public float swarmKwakAppearRate = 0.5f;
    bool swarming = false;
    int swarmCount = 0;
    public int swarmMaxAmount = 15;
    int swarmLign = 0;



    public delegate void OnKwakSwarm(int lignIndex);
    public OnKwakSwarm onKwakSwarm;

    float timer = 0f;

    bool spawning = true;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        Zone.onRetreat += HandleOnRetreat;
        Zone.onGoForth += HandleOnGoForth;

        DisplayHealth.onPlayerFailure += HandleOnPlayerFailure;

        zombieCount = 0;
        ResetTimer();

        ResetSwarm();
    }

    #region swarm
    private void ResetSwarm()
    {
        currentSwarmRecurence = Random.Range( swarmMinRecurence,swarmMaxRecurence );

        Invoke("LaunchSwarm",currentSwarmRecurence);
    }

    void LaunchSwarm()
    {
        swarmMaxAmount = (int)((float)Zone.Current.initAmount / 4f);

        if ( zombieCount > Zone.Current.initAmount - 5)
        {
            Debug.Log("NO :  zombie spawned " + zombieCount + " is superior than " + (Zone.Current.initAmount-5));

            return;
        }

        swarming = true;

        swarmCount = 0;

        swarmLign = Random.Range(0, 4);

        if (onKwakSwarm != null)
        {
            onKwakSwarm(swarmLign);
        }

    }
    void StopSwarm()
    {
        swarming = false;

        ResetSwarm();
    }
    #endregion

    private void HandleOnPlayerFailure()
    {
        spawning = false;
    }

    private void HandleOnGoForth()
    {
        zombieCount = 0;
    }

    private void HandleOnRetreat()
    {
        zombieCount = 0;
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && spawning)
        {

            if (zombieCount < Zone.Current.initAmount)
            {
                Spawn();
            }
        }

        /*testImage.color = swarming ? Color.red : Color.green;
        testText.text = "rate : " + rate;*/
       
    }

    public Image testImage;
    public Text testText;

    private void ResetTimer()
    {
        if ( swarming)
        {
            timer = swarmKwakAppearRate;
        }
        else
        {
            float l = (float)zombieCount / Zone.Current.initAmount;
            rate = Mathf.Lerp( maxRate , minRate, l );

            timer = rate;
        }
    }

    void Spawn () {

        ResetTimer();

        GameObject zombie = Instantiate (zombiePrefab, parent) as GameObject;

        zombie.transform.position = new Vector3(Lign.ligns[0].zombieAnchor.position.x + Random.Range(-spawnRange, spawnRange), Lign.ligns[0].zombieAnchor.position.y , 0f);

        ++zombieCount;
        if (swarming)
        {
            ++swarmCount;
            if ( swarmCount == swarmMaxAmount)
            {
                StopSwarm();
            }

            zombie.GetComponent<Zombie>().ChangeLign(swarmLign);

        }
        else
        {
            zombie.GetComponent<Zombie>().ChangeLign(Random.Range(0, 4));

        }


    }
}
