using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [OnValueChanged("UpdateEnemyDataInPlace")]
    private EnemySO enemyData;


    public string enemyTypeID { get; private set; } = "";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateEnemyDataInPlace()
    {


        //remove children objects i.e. visuals
        foreach (Transform child in this.transform)
        {
            if (Application.isEditor)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
        }

        if (enemyData == null)
        {
            return;
        }

        GameObject.Instantiate(enemyData.inGamePrefab, Vector3.zero, Quaternion.identity, this.gameObject.transform);


        name = enemyData.enemyName;
        enemyTypeID = enemyData.enemyTypeID;


    }
}
