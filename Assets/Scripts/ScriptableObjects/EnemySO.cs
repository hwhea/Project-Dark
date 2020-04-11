using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Project Dark/Enemy Data")]
public class EnemySO : SerializedScriptableObject
{
    [HorizontalGroup("Split", 55, LabelWidth = 70)]
    [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
    public GameObject inGamePrefab;

    [VerticalGroup("Split/Meta")]
    [LabelText("Name")]
    public string enemyName;

    [VerticalGroup("Split/Meta")]
    [TextArea]
    public string description;

    [VerticalGroup("Split/Meta")]

    public string enemyTypeID;

    [TabGroup("Basic Stats")]
    public float maxHealth = 100f;

    [TabGroup("AI")]
    public string stateMachine;

}
