using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MaterialRandomizer : MonoBehaviour
{

    [SerializeField]
    private List<Material> _possibleMaterials = new List<Material>();

    [SerializeField, Tooltip("Where is the mesh that we're changing material for? If left null, will use this gameobject.")]
    private GameObject materialTarget = null;

    void Awake()
    {
        ChangeMaterials();
    }

    [Button("Test")]
    public void ChangeMaterials()
    {
        GameObject target = (materialTarget == null ? this.gameObject : materialTarget);
        target.GetComponent<MeshRenderer>().material = _possibleMaterials[Random.Range(0, _possibleMaterials.Count)];

    }
}
