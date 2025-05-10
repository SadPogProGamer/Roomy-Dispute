using System;
using TMPro;
using UnityEngine;

public class SetItemText : MonoBehaviour
{
    [Header("Item Text")]
    [SerializeField] private TextMeshProUGUI _bookshelfPriceText;
    [SerializeField] private TextMeshProUGUI _bowlPriceText;
    [SerializeField] private TextMeshProUGUI _chairPriceText;
    [SerializeField] private TextMeshProUGUI _carpetPriceText;
    [SerializeField] private TextMeshProUGUI _couchPriceText;
    [SerializeField] private TextMeshProUGUI _dimLampPriceText;
    [SerializeField] private TextMeshProUGUI _drawerPriceText;
    [SerializeField] private TextMeshProUGUI _glassCupPriceText;
    [SerializeField] private TextMeshProUGUI _mirrorPriceText;
    [SerializeField] private TextMeshProUGUI _paintingPriceText;
    [SerializeField] private TextMeshProUGUI _rugPriceText;
    [SerializeField] private TextMeshProUGUI _singleSeaterPriceText;
    [SerializeField] private TextMeshProUGUI _smallLampPriceText;
    [SerializeField] private TextMeshProUGUI _tablePriceText;
    [SerializeField] private TextMeshProUGUI _tvPriceText;
    [SerializeField] private TextMeshProUGUI _tvStandPriceText;
    [SerializeField] private TextMeshProUGUI _vasePriceText;

    [Header("Items")]
    [SerializeField] private ItemStats _bookshelfItemStats;
    [SerializeField] private ItemStats _bowlItemStats;
    [SerializeField] private ItemStats _chairItemStats;
    [SerializeField] private ItemStats _carpetItemStats;
    [SerializeField] private ItemStats _couchItemStats;
    [SerializeField] private ItemStats _dimLampItemStats;
    [SerializeField] private ItemStats _drawerItemStats;
    [SerializeField] private ItemStats _glassCupItemStats;
    [SerializeField] private ItemStats _mirrorItemStats;
    [SerializeField] private ItemStats _paintingItemStats;
    [SerializeField] private ItemStats _rugItemStats;
    [SerializeField] private ItemStats _singleSeaterItemStats;
    [SerializeField] private ItemStats _smallLampItemStats;
    [SerializeField] private ItemStats _tableItemStats;
    [SerializeField] private ItemStats _tvItemStats;
    [SerializeField] private ItemStats _tvStandItemStats;
    [SerializeField] private ItemStats _vaseItemStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _bookshelfPriceText.text = $"${_bookshelfItemStats.Cost}";
        _bowlPriceText.text = $"${_bowlItemStats.Cost}";
        _chairPriceText.text = $"${_chairItemStats.Cost}";
        _carpetPriceText.text = $"${_carpetItemStats.Cost}";
        _couchPriceText.text = $"${_couchItemStats.Cost}";
        _dimLampPriceText.text = $"${_dimLampItemStats.Cost}";
        _drawerPriceText.text = $"${_drawerItemStats.Cost}";
        _glassCupPriceText.text = $"${_glassCupItemStats.Cost}";
        _mirrorPriceText.text = $"${_mirrorItemStats.Cost}";
        _paintingPriceText.text = $"${_paintingItemStats.Cost}";
        _rugPriceText.text = $"${_rugItemStats.Cost}";
        _singleSeaterPriceText.text = $"${_singleSeaterItemStats.Cost}";
        _smallLampPriceText.text = $"${_smallLampItemStats.Cost}";
        _tablePriceText.text = $"${_tableItemStats.Cost}";
        _tvPriceText.text = $"${_tvItemStats.Cost}";
        _tvStandPriceText.text = $"${_tvStandItemStats.Cost}";
        _vasePriceText.text = $"${_vaseItemStats.Cost}";
    }
}
