using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _playerTexts;

    private List<int> _playerMoney = new List<int>();


    private void Start()
    {
        for (int i = 0; i < _playerTexts.Length; i++)
        {
            _playerMoney.Add(100); // Start at 0 instead of 10
        }

        UpdateAllMoneyUI();
    }


    private void Update()
    {
        for (int i = 0; i < _playerMoney.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) 
            {
                IncreaseMoney(i, 10);
            }
        }
    }

    public void IncreaseMoney(int playerID, int amount)
    {
        if (playerID >= 0 && playerID < _playerMoney.Count)
        {
            _playerMoney[playerID] += amount;
            UpdateMoneyUI(playerID);
        }
    }

    private void DecreaseMoney(int playerID, int amount)
    {
        if (playerID >= 0 && playerID < _playerMoney.Count)
        {
            _playerMoney[playerID] += amount;
            UpdateMoneyUI(playerID);
        }
    }

    private void UpdateMoneyUI(int playerID)
    {
        _playerTexts[playerID].text = $"Player {playerID + 1}: ${_playerMoney[playerID]}";
    }

    private void UpdateAllMoneyUI()
    {
        for (int i = 0; i < _playerMoney.Count; i++)
        {
            UpdateMoneyUI(i);
        }
    }
}
