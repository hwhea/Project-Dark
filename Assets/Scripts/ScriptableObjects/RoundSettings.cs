using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Project Dark/Round Settings", fileName = "Round")]
public class RoundSettings : SerializedScriptableObject
{
    /*
    Define enemies to spawn, max spawnable per round and min spawnable per round
    */

    [Tooltip("How long between each spawn?")]
    public float averageSpawnInterval = 1f;

    [Tooltip("How long should we spread the spawning of these zombies out over?")]

    public float acceptedDeviation = 0.2f;


    public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();

    int cachedEnemyCount = -1;

    public int GetTotalEnemyCount()
    {
        //We can cache this as an optimisation, doesn't change after runtime.
        if (cachedEnemyCount != -1)
        {
            return cachedEnemyCount;
        }

        int sum = 0;
        foreach (EnemySpawnInfo enemy in enemies)
        {
            sum += enemy.spawnAmount;
        }
        cachedEnemyCount = sum;
        return sum;
    }

}

[System.Serializable]
public class EnemySpawnInfo
{
    [HorizontalGroup("Split", 55, LabelWidth = 90)]
    [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
    public GameObject gameObjectPreview;

    [VerticalGroup("Split/MoreData")]
    [OnValueChanged("UpdatePreview")]
    public EnemySO enemy;

    [VerticalGroup("Split/MoreData")]
    public int spawnAmount = 0;

    [VerticalGroup("Split/MoreData")]
    [ProgressBar(0, 50)]
    public int maxAlive = 1;


    private void UpdatePreview()
    {
        Debug.Log("Update called");
        gameObjectPreview = enemy.inGamePrefab;
    }
}

