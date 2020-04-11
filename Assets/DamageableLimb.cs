using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamageableLimb : MonoBehaviour, IDamageable
{
    [SerializeField]
    [Tooltip("The health controller for this object. Will be automatically detected if in a parent (even multiple levels up).")]
    HealthController _healthController = null;

    [SerializeField]
    [Tooltip("How much do we want to multiply damage in this region by? EG: Head would have a higher multiplier.")]
    float limbDamageModifier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (_healthController == null)
        {
            if (!RecursiveFindController(gameObject))
            {
                Debug.LogError($"{gameObject.name} failed to find HealthController in any parents.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DealDamage(float amount)
    {
        _healthController.Damage(amount * limbDamageModifier);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startPoint"></param>
    /// <returns></returns>
    bool RecursiveFindController(GameObject startPoint)
    {
        if (startPoint == null || startPoint.transform.parent == null) return false;

        if (startPoint.GetComponent<HealthController>())
        {
            _healthController = startPoint.GetComponent<HealthController>();
            return true;
        }
        else
        {
            return RecursiveFindController(startPoint.transform.parent.gameObject);
        }
    }



    /// <summary>
    /// Warning: only called in editor.
    /// </summary>
    [Button("Autolocate Controller")]
    void Reset()
    {
        Debug.Log("Found: " + RecursiveFindController(gameObject));
    }

    [Button("Simulate Hit (10pts)")]
    void DEBUG_SimulateHit()
    {
        DealDamage(10);
    }


}
