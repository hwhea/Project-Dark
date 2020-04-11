using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "ProjectDark/New Map")]
public class MapDefinition : SerializedScriptableObject
{
    [BoxGroup("Map Meta Information")]
    [LabelWidth(80)]
    public string mapName = "New Map";

    [BoxGroup("Map Meta Information")]
    [TextArea]
    public string mapDescription = "";

    [BoxGroup("Rounds")]
    [InlineEditor]
    public List<RoundSettings> rounds = new List<RoundSettings>();

    [BoxGroup("Rounds")]
    [Button("Automatically build rounds.")]
    void BuildRounds()
    {
        Debug.Log("Build rounds not yet implemented.");
    }

}
