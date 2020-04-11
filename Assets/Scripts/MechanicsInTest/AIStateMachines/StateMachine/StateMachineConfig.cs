using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State Machine Config", menuName = "Redwire/AI/State Machine Config")]
public class StateMachineConfig : ScriptableObject
{
    public List<GameObject> StateMachineNodeTypes = new List<GameObject>();
}
