using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpawnPoint : MonoBehaviour
{
    [Range(0, 100)]
    public int spawnWeight = 50;

    public BoxCollider placementCollider { get; private set; }


    void Awake()
    {
        placementCollider = GetComponent<BoxCollider>();
    }

    void OnDrawGizmos()
    {
        if (placementCollider == null)
            placementCollider = GetComponent<BoxCollider>();
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(255, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position + placementCollider.center, placementCollider.bounds.size);
#if UNITY_EDITOR
        Handles.Label(transform.position, $"Spawn Point (Weight: {spawnWeight})");
#endif
    }
}
