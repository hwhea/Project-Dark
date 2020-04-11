using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyWalletUIDisplay : MonoBehaviour
{

    [SerializeField]
    private Text _walletAmountTextElement;

    public void UpdateAmount(int amountAdded, int newTotal)
    {
        _walletAmountTextElement.text = newTotal + "";
    }
}
