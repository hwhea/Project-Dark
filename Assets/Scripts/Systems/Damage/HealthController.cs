using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A central class for all other Health affectors to speak to. 
/// </summary>
public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 100f;

    [SerializeField]
    private float _currentHealth = 0f;

    [SerializeField]
    public float CurrentHealth { get { return _currentHealth; } }

    [System.Serializable]
    private class HealthEvent : UnityEvent<float> { };

    [System.Serializable]
    private class HealthLifeEvent : UnityEvent { };

    [SerializeField]
    private bool _destroyOnDeath = false;

    [SerializeField]
    [FoldoutGroup("Events")]
    HealthLifeEvent _onDeathEvent = new HealthLifeEvent();

    [SerializeField]
    [FoldoutGroup("Events")]

    HealthLifeEvent _onRespawnEvent = new HealthLifeEvent();

    [SerializeField]
    [FoldoutGroup("Events")]

    HealthEvent _onHealEvent = new HealthEvent();

    [SerializeField]
    [FoldoutGroup("Events")]
    HealthEvent _onDamageEvent = new HealthEvent();


    [FoldoutGroup("Death Actions")]
    [SerializeField]
    private bool _destroyDeathActionObjects = false;

    [FoldoutGroup("Death Actions")]
    [SerializeField]
    List<GameObject> _objectsToHide = new List<GameObject>();

    [FoldoutGroup("Death Actions")]
    [SerializeField]
    List<GameObject> _objectsToShow = new List<GameObject>();

    [FoldoutGroup("Death Actions")]
    [SerializeField]
    List<Collider> _collidersToDisable = new List<Collider>();

    [FoldoutGroup("Death Actions")]
    [SerializeField]
    List<Collider> _collidersToEnable = new List<Collider>();


    void Start()
    {
        //Current health should be max health at it's maximum at start.
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        _onHealEvent.Invoke(amount);


    }

    public void Damage(float amount)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        _onDamageEvent.Invoke(amount);

        if (_currentHealth <= 0)
        {
            _onDeathEvent.Invoke();

            DoDeathActions();

            if (_destroyOnDeath)
            {
                Destroy(gameObject);

            }
        }
    }

    private void DoDeathActions()
    {

        foreach (Collider col in _collidersToDisable)
        {
            col.enabled = false;
        }

        foreach (Collider col in _collidersToEnable)
        {
            col.enabled = true;
        }

        foreach (GameObject obj in _objectsToHide)
        {
            if (_destroyDeathActionObjects)
            {
                Destroy(obj);
            }
            else
            {
                obj.SetActive(false);
            }
        }

        foreach (GameObject obj in _objectsToShow)
        {
            obj.SetActive(true);
        }


    }
}
