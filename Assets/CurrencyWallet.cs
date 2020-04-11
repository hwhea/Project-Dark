using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyWallet : MonoBehaviour
{

    [SerializeField]
    bool limitCurrency = false;

    [SerializeField, ShowIf("limitCurrency")]
    private int _maxAllowedCurrencyInInventory = 100;

    [System.Serializable]
    public class CurrencyEvent : UnityEvent<int, int> { };

    public CurrencyEvent OnCurrencyChanged;

    [ReadOnly, SerializeField]
    private int _currencyInWallet = 0;

    /// <summary>
    /// The amount of the currency in this inventory.
    /// </summary>
    /// <value></value>
    public int CurrencyInWallet
    {
        get => _currencyInWallet;

        private set
        {
            if (limitCurrency)
            {
                if (value + _currencyInWallet > _maxAllowedCurrencyInInventory)
                {
                    _currencyInWallet = _maxAllowedCurrencyInInventory;
                }
            }
            else
            {
                _currencyInWallet = value;
            }
        }
    }

    void Start()
    {
        OnCurrencyChanged.Invoke(0, CurrencyInWallet);
    }

    public void AddCurrency(int amount)
    {
        CurrencyInWallet += amount;
        OnCurrencyChanged.Invoke(amount, CurrencyInWallet);
    }

    public void RemoveCurrency(int amount)
    {
        CurrencyInWallet -= amount;
        OnCurrencyChanged.Invoke(-amount, CurrencyInWallet);
    }

#if UNITY_EDITOR

    [Button("Test Add 10")]
    private void TestAdd10()
    {
        AddCurrency(10);
    }

    [Button("Test Remove 10")]
    private void TestRemove10()
    {
        RemoveCurrency(10);
    }

#endif
}
