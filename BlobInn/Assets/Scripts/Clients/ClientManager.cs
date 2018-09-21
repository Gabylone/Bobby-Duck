using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{

    public static ClientManager Instance;

    public Coords appearCoords;

    public GameObject clientPrefab;

    public float prince_ChanceAppearing = 0.05f;

    public GameObject princePrefab;

    public Transform clientParent;

    public float timer = 0f;

    public float timeBeforeFirstClient = 3f;

    public int currentClientAmount = 0;
    public int lostClientAmount = 0;

    public int lineCapacity = 4;

    public Color patienceColor_Full;
    public Color patienceColor_Mid;
    public Color patienceColor_Empty;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Client.clients.Clear();

        timer = timeBeforeFirstClient;

    }

    public void ResetTimer()
    {
        if (LevelManager.Instance.rushHour)
        {
            timer = Level.Current.rate_RushHour;
        }
        else
        {
            //float f = timer / Level.Current.dayDuration;
            float f = (float)currentClientAmount / (float)Level.Current.maxClientAmount;

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

    Client.Type GetNewClientType()
    {
        int clientType_Index = Random.Range(0, (int)Level.Current.clientType + 1);

        if (Random.value * 100f < prince_ChanceAppearing)
        {
            clientType_Index = (int)Client.Type.Prince;

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
        }

        return (Client.Type)clientType_Index;
    }

    void NewClient()
    {
        HandleQueue();

        Door.Instance.Open();

        Invoke("NewClientDelay", Door.Instance.openTime);
    }

    void NewClientDelay()
    {
        GameObject newClient = Instantiate(clientPrefab, clientParent) as GameObject;
        newClient.GetComponent<Client>().type = GetNewClientType();
        newClient.transform.position = new Vector3(appearCoords.x, appearCoords.y, 0f);

        ++currentClientAmount;

        if (currentClientAmount == Level.Current.clientsToRushHour)
        {
            LevelManager.Instance.StartRushHour();
        }

        if (currentClientAmount == Level.Current.maxClientAmount)
        {
            LevelManager.Instance.EndDay();
        }

        Door.Instance.Close();

        Tutorial.Instance.Show(TutorialStep.Client);
    }

    #region queue
    void HandleQueue()
    {
        int waitingClientCount = 0;
        foreach (var client in Client.clients)
        {
            if (client.currentState == Client.State.LookForTable)
            {
                ++waitingClientCount;
            }
        }

        if (waitingClientCount >= -Tile.minY + 5)
        {
            foreach (var client in Client.clients)
            {
                if (client.currentState == Client.State.LookForTable)
                {
                    client.ChangeState(Client.State.Leaving);
                    return;
                }
            }
            return;
        }

        int a = 0;
        foreach (var client in Client.clients)
        {
            if (client.currentState == Client.State.LookForTable)
            {
                if (client.coords.y == Tile.minY)
                {
                    client.Move(Swipe.Direction.Left);
                }
                else
                {
                    client.Move(Swipe.Direction.Down);
                }


                ++a;
            }
        }
    }
    #endregion

}
