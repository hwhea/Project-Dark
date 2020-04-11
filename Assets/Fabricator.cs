using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BNG;
using System;

public class Fabricator : MonoBehaviour
{

    [SerializeField, Required]
    private FabricatorSelection _fabricatorSelection = null;

    [SerializeField, Required]
    private SnapZone _snapZone = null;

    [SerializeField]
    private CurrencyWallet _currencyWallet = null;

    [SerializeField]
    bool _canCancelBuild = false;


    [Header("Build Effects (enabled while building)")]
    public List<GameObject> _buildEffects = new List<GameObject>();

    //Related to currently building item:
    float _buildSecondsRemaining = 0;
    bool _currentlyBuilding = false;
    SOFabricatorItem _itemToBuild = null;

    // Start is called before the first frame update
    void Start()
    {
        if (_fabricatorSelection == null)
        {
            Debug.LogError("Missing assignment of FabricatorSelection in Fabricator; cannot operate.");
        }

        if (_snapZone == null)
        {
            Debug.LogError("Missing assignment of SnapZone in Fabricator; cannot operate.");
        }

        SetBuildEffectsEnabled(false);
    }

    void Update()
    {
        if (_currentlyBuilding && _itemToBuild != null)
        {
            if (_buildSecondsRemaining > 0)
            {

                _buildSecondsRemaining -= Time.deltaTime;
            }
            else
            {
                //Finished building
                _currentlyBuilding = false;

                //Stop particles
                SetBuildEffectsEnabled(false);

                //Stop animation
                Debug.Log("Finished Building");

                //Generate new item.
                GameObject newItem = GameObject.Instantiate(_itemToBuild.inGamePrefab, Vector3.zero, Quaternion.identity);

                //Snap new object to snapzone
                _snapZone.GrabGrabbable(newItem.GetComponent<Grabbable>());
            }
        }

    }

    private void SetBuildEffectsEnabled(bool enabled)
    {
        foreach (GameObject go in _buildEffects)
        {
            go.SetActive(enabled);
        }
    }

    /// <summary>
    /// Evaluates whether the building process can start. Will test if there's an object in the snapZone.
    /// </summary>
    /// <returns></returns>
    public bool CanBuild()
    {
        if (_currentlyBuilding)
            return false;

        if (_snapZone.HeldItem != null)
            return false;

        //Only check for currency if currency wallet is set.
        if (_currencyWallet)
            if (_currencyWallet.CurrencyInWallet < _fabricatorSelection.SelectedFabricatorItem.ItemPrice)
                return false;

        //TODO: add more checks

        return true;
    }

    public void BuildSelectedItem()
    {
        _itemToBuild = _fabricatorSelection.SelectedFabricatorItem;

        if (!_itemToBuild) return;

        if (!CanBuild())
        {
            //TODO: Handle not able to build. Red light or something?
            Debug.Log("Fabricator issue: can't build");
        }
        else
        {
            _buildSecondsRemaining = _itemToBuild.SecondsTakenToBuild;

            //Start particles
            SetBuildEffectsEnabled(true);

            //Start animation

            //Charge currency wallet
            if (_currencyWallet)
                _currencyWallet.RemoveCurrency(_itemToBuild.ItemPrice);

            _currentlyBuilding = true;
        }
    }

    public void CancelBuild()
    {
        if (!_canCancelBuild)
            return;

        _currentlyBuilding = false;
        _buildSecondsRemaining = -1;

        //Disable build effects.
        SetBuildEffectsEnabled(false);

        //Refund cost
        if (_currencyWallet)
            _currencyWallet.AddCurrency(_itemToBuild.ItemPrice);

        _itemToBuild = null;
    }


    public void UpgradeExisting()
    {

    }
}
