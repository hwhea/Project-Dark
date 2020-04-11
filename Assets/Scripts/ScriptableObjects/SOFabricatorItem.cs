using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SOFabricatorItem : ScriptableObject
{
    [HorizontalGroup("Split", 55, LabelWidth = 70)]
    [Header("Item Prefab")]
    [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
    public GameObject inGamePrefab;


    [VerticalGroup("Split/Meta")]
    [LabelText("Name")]
    public string ItemName = "";

    [VerticalGroup("Split/Meta")]
    [TextArea]
    public string ItemDescription = "";

    [VerticalGroup("Split/Meta")]
    public int ItemPrice;

    public Sprite ItemFabricatorIcon;

    [SerializeField, Range(0, 60)]
    public float SecondsTakenToBuild;


}
