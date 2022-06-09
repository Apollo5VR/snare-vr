using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    /// <summary>
    /// Summary description
    /// </summary>
    [Tooltip("Tooltip")]
    [Header("Header For Use")]
    //Script Purpose: To create a countdown clock that respawns enemies
    public bool debug = false;
    public int enemyMaxCount = 10;
    public ForestEnemy[] forestEnemyPrefabs; //grabbed from Resources folder
    public GameObject[] randomSpawnLocations;
    public float timeCounter = 5.0f;

    private Transform destination;
    private List<List<ForestEnemy>> forestEnemies = new List<List<ForestEnemy>>(); //multi-dimensional list
    private bool sequenceOver;
    private float originalTime;
    private int deathCount;

    private void Start()
    {
        originalTime = timeCounter;
        InitializeEnemyPool();
        sequenceOver = false;
        ScriptsConnector.Instance.OnStartEnemySpawnSequence += StartSpawnSequence;
        ScriptsConnector.Instance.OnDeath += ProgressTicker;

        Debug.Log("GG - we ran this!");
    }

    private void InitializeEnemyPool()
    {    
        //spawn list of enemies for enemy pool
        if(forestEnemyPrefabs.Length != 0)
        {
            for (int i = 0; i < forestEnemyPrefabs.Length; i++)
            {
                int arrayLength = enemyMaxCount;
                forestEnemies.Add(new List<ForestEnemy>());

                for (int j = 0; j < arrayLength; j++)
                {
                    forestEnemies[i].Add(Instantiate(forestEnemyPrefabs[i]));
                    forestEnemies[i][j].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("forest enemy prefabs need to be added to SpawnController inspector");
        }
    }

    private ForestEnemy RetrieveEnemyFromPool(int typeIndex)
    {
        ForestEnemy returnObject = null;

        if(destination == null)
        {
            //set destination
            destination = ScriptsConnector.Instance.OnGetTrapDestination?.Invoke();
        }

        //TODO - better system than this for accessing pool?
        for (int i = 0; i < forestEnemies[typeIndex].Count; i++)
        {
            if (!forestEnemies[typeIndex][i].gameObject.activeSelf)
            {
                returnObject = forestEnemies[typeIndex][i];
                break;
            }
            //if last one is not inactive
            else if(i == forestEnemies[typeIndex].Count - 1)
            {
                forestEnemies[typeIndex].Add(Instantiate(forestEnemyPrefabs[typeIndex]));
                returnObject = forestEnemies[typeIndex][i + 1];
                break;
            }         
        }

        return returnObject;
    }

    public void Update()
    {
        if (debug)
        {
            StartSpawnSequence();
            debug = false;
        }
    }

    public void StartSpawnSequence()
    {
        StartCoroutine(SpawnSequence());
    }

    private IEnumerator SpawnSequence()
    {
        int spawnedCount = 0;

        while (!sequenceOver && spawnedCount < enemyMaxCount)
        {
            float waitTime = 1;

            //if the count down reaches 0, respawn another zombie
            if (timeCounter <= 0)
            {
                //select one of the random spawn locations
                //TODO - to allow flexibility later on if we want to add a difficulty system
                int randomLocation = Random.Range(0, 3);
                int randomType = Random.Range(0, 2);

                //spawn at chosen random location
                ForestEnemy enemy = RetrieveEnemyFromPool(0); //proto/testing only
                //ForestEnemy enemy = RetrieveEnemyFromPool(randomType);

                enemy.Spawn(1.1f, randomSpawnLocations[randomLocation].transform, destination);

                spawnedCount++;

                //reset counter to the original time
                timeCounter = originalTime;
            }

            //start countdown
            timeCounter = timeCounter - waitTime;

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void ProgressTicker(GameObject hitObj, bool isEnemy)
    {
        if(isEnemy)
        {
            deathCount++;
            if (deathCount == 6)
            {
                sequenceOver = true;
                ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "ALL THE WOLVES ARE DEAD!");
            }
        }
        else
        {
            sequenceOver = true;
            ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "UH OH THEY DESTROYED YOUR TRAP. SET ANOTHER.");
            //attackSuccessCount++;
            
            //TODO - clear trap via UGS & in-game visuals?
        }
    }

    private void OnDestroy()
    {
        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnStartEnemySpawnSequence -= StartSpawnSequence;
            ScriptsConnector.Instance.OnDeath -= ProgressTicker;
        }
    }
}
