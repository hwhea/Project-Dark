using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
public class Biomass : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Vector3 randScale = new Vector3(Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f));
        Vector3 randRotation = new Vector3(Random.Range(0, 1) * 360, Random.Range(0, 1) * 360, Random.Range(0, 1) * 360);

        this.transform.localScale = randScale;
        this.transform.localRotation = Quaternion.Euler(randRotation);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
