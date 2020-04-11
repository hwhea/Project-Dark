using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FabricatorSelection : MonoBehaviour
{

    public SOFabricatorItem SelectedFabricatorItem { get { return _possibleItems[SelectedIndex]; } }

    [SerializeField, LabelText("Default Selected Index")]
    int SelectedIndex = 0;

    [SerializeField]
    List<SOFabricatorItem> _possibleItems = new List<SOFabricatorItem>();

    [Header("UI References")]
    [SerializeField]
    private Text _itemNameUIText;

    [SerializeField]
    private Text _itemDescUIText;

    [SerializeField]
    private Text _itemPriceUIText;

    [SerializeField]
    private Image _itemIconUIImage;

    // Start is called before the first frame update
    void Start()
    {
        //TODO need to get possible item list from map definition file.
        if (_possibleItems.Count == 0)
        {
            Debug.LogError("Fabricator has no possible items; nothing can be built. Terminating selector.");
            this.enabled = false;
        }

        if (!_itemNameUIText || !_itemDescUIText || !_itemPriceUIText)
        {
            Debug.LogError("One of the UI references on the fabricator selection was not set.");
        }

        SetActiveSelection(SelectedIndex);
    }

    void SetActiveSelection(int idx)
    {

        if (idx >= _possibleItems.Count)
        {
            idx = 0;
        }
        else if (idx < 0)
        {
            idx = _possibleItems.Count - 1;
        }

        SelectedIndex = idx;

        SOFabricatorItem item = _possibleItems[SelectedIndex];

        _itemNameUIText.text = item.ItemName;
        _itemPriceUIText.text = item.ItemPrice.ToString();
        _itemDescUIText.text = item.ItemDescription;

        if (_itemIconUIImage && item.ItemFabricatorIcon)
            _itemIconUIImage.sprite = item.ItemFabricatorIcon;

    }

    public void SelectNextItem()
    {
        SetActiveSelection(SelectedIndex + 1);
    }


    public void SelectPreviousItem()
    {
        SetActiveSelection(SelectedIndex - 1);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
