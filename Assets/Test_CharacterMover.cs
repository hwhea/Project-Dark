using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CharacterMover : MonoBehaviour
{
    public Vector3 targetPoint = new Vector3();

    bool moveToTargetPoint = false;

    public void MoveTowardsTarget(Vector3 target)
    {
        targetPoint = target;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, Time.deltaTime * 3f);
    }

    public void StopMoving()
    {
    }

    public Vector3 SelectRandomTargetPoint()
    {
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }


    // Update is called once per frame
    void Update()
    {

    }
}
