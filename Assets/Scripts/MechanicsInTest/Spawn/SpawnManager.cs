using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    [InlineEditor]
    SpawnPoint[] spawnPoints;

    List<GameObject> spawnedThisRound = new List<GameObject>();

    [ShowInInspector]
    List<SpawnQueueItem> toSpawn = new List<SpawnQueueItem>();

    public GameObject enemySpawnParent = null;

    int[] spawnWeightWheel;

    RoundSettings activeRoundSettings;

    // Start is called before the first frame update
    void Start()
    {
        InitializeManager();
    }

    void InitializeManager()
    {
        spawnPoints = (SpawnPoint[])FindObjectsOfType(typeof(SpawnPoint));
        List<int> spawnWeights = new List<int>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Debug.Log("SpawnPoint " + spawnPoints[i].name);
            int toAdd = spawnPoints[i].spawnWeight;
            while (toAdd > 0)
            {
                spawnWeights.Add(i);
                toAdd--;
            }
        }

        spawnWeightWheel = spawnWeights.ToArray();
    }


    public void OnNextRoundEvent(int i, RoundSettings round)
    {
        DestroyAllSpawned();

        foreach (EnemySpawnInfo esi in round.enemies)
        {
            toSpawn.Add(new SpawnQueueItem()
            {
                objectToSpawn = esi.enemy.inGamePrefab,
                amountLeftToSpawn = esi.spawnAmount,
                maxAlive = esi.maxAlive
            });
        }

        activeRoundSettings = round;


    }

    private SpawnPoint ChooseSpawnPoint()
    {
        return spawnPoints[spawnWeightWheel[UnityEngine.Random.Range(0, spawnWeightWheel.Length)]];
    }

    private void SpawnEnemy(SpawnQueueItem item, SpawnPoint inSpawnPoint)
    {
        GameObject go = (GameObject)Instantiate(item.objectToSpawn, inSpawnPoint.placementCollider.center + RandomPointInCollider(inSpawnPoint.placementCollider), Quaternion.identity, enemySpawnParent.transform);
        spawnedThisRound.Add(go);
        item.amountLeftToSpawn -= 1;
        item.currentAlive += 1;
    }

    private Vector3 RandomPointInCollider(Collider col)
    {
        Bounds bounds = col.bounds;
        //IMPROVE: raycast to ground at point for accurate y pos
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            0,
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void DestroyAllSpawned()
    {
        for (int i = 0; i < spawnedThisRound.Count; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(spawnedThisRound[i]);
#else
            Destroy(spawnedThisRound[i]);
#endif
        }
    }

    private SpawnQueueItem SelectRandomFromSpawnQueue()
    {
        List<SpawnQueueItem> availItems = toSpawn.Where(item => item.maxAlive > item.currentAlive && item.amountLeftToSpawn > 0).ToList();
        if (availItems.Count == 0)
        {
            return null;
        }
        int selection = UnityEngine.Random.Range(0, availItems.Count());
        return availItems[selection];
    }

    float nextSpawnTime = 0;
    // Update is called once per frame
    void Update()
    {
        //Actually do our spawning.
        if (toSpawn.Count > 0)
        {
            if (Time.time > nextSpawnTime)
            {
                SpawnQueueItem item = SelectRandomFromSpawnQueue();
                if (item != null)
                {
                    SpawnEnemy(item, ChooseSpawnPoint());
                    if (item.amountLeftToSpawn == 0)
                    {
                        toSpawn.Remove(item);
                    }
                    nextSpawnTime = Time.time + activeRoundSettings.averageSpawnInterval + UnityEngine.Random.Range(-activeRoundSettings.acceptedDeviation, activeRoundSettings.acceptedDeviation);
                }
            }

            //Select a random enemy from our spawn queue
        }
    }

    public void OnEnemyKilled(Enemy enemyData)
    {
        SpawnQueueItem[] enemyToKill = toSpawn.Where(item => item.objectToSpawn.GetComponent<Enemy>().enemyTypeID == enemyData.enemyTypeID).ToArray();

        if (enemyToKill.Length > 0)
        {
            enemyToKill[0].currentAlive -= 1;
        }
    }
}

internal class SpawnQueueItem
{
    public GameObject objectToSpawn;
    public int amountLeftToSpawn;

    public int maxAlive;

    public int currentAlive;
}