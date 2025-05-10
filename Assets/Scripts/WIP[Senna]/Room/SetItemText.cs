using System;
using TMPro;
using UnityEngine;

public class SetItemText : MonoBehaviour
{
    [Header("Item Text")]
    [SerializeField] private TextMeshProUGUI _bookshelfPriceText;
    [SerializeField] private TextMeshProUGUI _bowlPriceText;
    [SerializeField] private TextMeshProUGUI _chairPriceText;

    [Header("Items")]
    [SerializeField] private ItemStats _bookshelfItemStats;
    [SerializeField] private ItemStats _bowlItemStats;
    [SerializeField] private ItemStats _chairItemStats;

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
    }
}
