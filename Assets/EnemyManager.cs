using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{

    private static EnemyManager oneCheck = null;

    [SerializeField]
    int enemiesToKillThisRound = 0;

    [SerializeField]
    int enemiesKilledThisRound = 0;

    [SerializeField]
    [ReadOnly(true)]
    bool roundInitialized = false;

    public UnityEvent OnAllEnemiesDead;

    [System.Serializable]
    public class EnemyKilledEvent : UnityEvent<Enemy> { };

    public EnemyKilledEvent enemyKilledEvent;

    void Start()
    {
        if (oneCheck != null)
        {
            Debug.LogError("More than one EnemyManager registered in scene.");
        }
        else
        {
            oneCheck = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (roundInitialized)
        {
            if (enemiesKilledThisRound >= enemiesToKillThisRound)
            {
                roundInitialized = false;
                OnAllEnemiesDead.Invoke();
            }
        }
    }

    //Triggered by game conductor round start event
    public void OnRoundStart(int roundNumber, RoundSettings round)
    {
        enemiesToKillThisRound = round.GetTotalEnemyCount();
        enemiesKilledThisRound = 0;
        roundInitialized = true;
        Debug.Log("Enemy manager initialized new round");
    }

    public void OnEnemyKilled(Enemy enemyData)
    {
        enemyKilledEvent.Invoke(enemyData);
        enemiesKilledThisRound += 1;
    }
}
