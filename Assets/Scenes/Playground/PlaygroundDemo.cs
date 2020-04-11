using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundDemo : MonoBehaviour
{
    public GameObject _biomassPrefab;
    public Transform _biomassSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnBiomass()
    {
        Instantiate(_biomassPrefab, _biomassSpawnPoint.position, Quaternion.identity, null);
    }
}
