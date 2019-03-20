using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance;

    public GameObject enemyPrefab;

    public float flag_ChanceAppearing = 0.05f;

    public GameObject flagBlobPrefab;

    public Transform enemyParent;

    public float timer = 0f;

    public float timeBeforeFirstEnemy = 3f;

    public int currentEnemyAmount = 0;

    public Transform spawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Enemy.enemies.Clear();

        timer = timeBeforeFirstEnemy;

    }

    public void ResetTimer()
    {
        if (LevelManager.Instance.rushHour)
        {
            timer = Level.Current.rate_RushHour;
        }
        else
        {
            float f = (float)currentEnemyAmount / (float)Level.Current.maxClientAmount;

            timer = Mathf.Lerp(Level.Current.startOfDayRate, Level.Current.endOfDayRate, f);

        }
    }

    private void Update()
    {
        if (LevelManager.Instance.dayEnded == false)
        {
            UpdateClientArrival();
        }

    }

    void UpdateClientArrival()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            ResetTimer();

            NewClient();
        }
    }

    Enemy.Type GetNewEnemyType()
    {
        int clientType_Index = Random.Range(0, (int)Level.Current.clientType + 1);

        /*if (Random.value * 100f < flag_ChanceAppearing)
        {
			clientType_Index = (int)Enemy.Type.FlagBlob;

            switch (Inventory.currentLanguageType)
            {
                case Inventory.LanguageType.French:
                    DisplayInfo.Instance.Show("UN PRINCE ENTRE !");
                    break;
                case Inventory.LanguageType.English:
                    DisplayInfo.Instance.Show("A PRINCE APPEARS !");
                    break;
                case Inventory.LanguageType.None:
                    break;
                default:
                    break;
            }
        }*/

        return (Enemy.Type)clientType_Index;
    }

    void NewClient()
    {
		GameObject newEnemy = Instantiate(enemyPrefab, enemyParent) as GameObject;
		newEnemy.GetComponent<Enemy>().type = GetNewEnemyType();
        newEnemy.GetComponent<Enemy>().currentLignIndex = Random.Range(0, LignManager.Instance.ligns.Length);
        newEnemy.transform.position = spawnPoint.position;

		++currentEnemyAmount;

		if (currentEnemyAmount == Level.Current.clientsToRushHour)
		{
			LevelManager.Instance.StartRushHour();
		}

		if (currentEnemyAmount == Level.Current.maxClientAmount)
		{
			LevelManager.Instance.EndDay();
		}

		Invoke("NewClientDelay",0.1f);
    }

    void NewClientDelay()
    {
        Tutorial.Instance.Show(TutorialStep.Client);
    }

}
