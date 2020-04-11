using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class GameConductor : MonoBehaviour
{

    [Required]
    public MapLoader mapLoader;

    [System.Serializable]
    public class RoundChangeEvent : UnityEvent<int, RoundSettings> { };


    [TabGroup("Round Events")]
    public RoundChangeEvent roundChange;


    [TabGroup("Level Events")]
    public UnityEvent levelStart;
    [TabGroup("Level Events")]
    public UnityEvent levelWon;
    [TabGroup("Level Events")]
    public UnityEvent levelLost;


    [Space(10f)]
    [ReadOnly]
    public int currentRound = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentRound = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PropertySpace(10)]
    [HorizontalGroup("Controls")]
    [Button("Force Next Round")]
    public void NextRound()
    {

        if (mapLoader.mapDefinition.rounds.Count == currentRound)
        {
            Debug.Log("Won!");
            levelWon.Invoke();
            return;
        }

        currentRound += 1;

        Debug.Log($"<b> Moved to round {currentRound} </b>");

        roundChange.Invoke(currentRound, mapLoader.GetRoundByNumber(currentRound));
    }

    [PropertySpace(10)]
    [HorizontalGroup("Controls")]
    [Button("Reset Level")]
    public void ResetLevel()
    {
        currentRound = 1;
        roundChange.Invoke(currentRound, mapLoader.GetRoundByNumber(currentRound));
    }

    public void OnGUI()
    {
        GUILayout.Label("Map: " + mapLoader.mapDefinition.mapName);
        GUILayout.Label("Round: " + currentRound);
    }

    public void AllEnemiesKilledEvent()
    {
        Debug.Log("GC got AllEnemiesDead event.");
        NextRound();
    }

}
