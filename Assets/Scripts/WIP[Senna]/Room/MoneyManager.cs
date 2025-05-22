using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _playerTexts;

    private PlayersInstantiate _playerInstantiate;

    public List<int> PlayerMoney = new List<int>();


    private void Start()
    {
        for (int i = 0; i < _playerTexts.Length; i++)
        {
            PlayerMoney.Add(400);
            _playerTexts[i].color = Color.clear;
        }

        _playerInstantiate = Object.FindFirstObjectByType<PlayersInstantiate>();

        UpdateAllMoneyUI();

        for (int i = 0 ; i < _playerInstantiate._playerCount ; i++)
        {
            _playerTexts[i].color = _playerInstantiate._playerMaterials[i].color;
        }
    }


    private void Update()
    {
        for (int i = 0; i < PlayerMoney.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) 
            {
                IncreaseMoney(i, 10);
            }
        }
    }

    public void IncreaseMoney(int playerID, int amount)
    {
        if (playerID >= 0 && playerID < PlayerMoney.Count)
        {
            PlayerMoney[playerID] += amount;
            UpdateMoneyUI(playerID);
        }
    }

    public void DecreaseMoney(int playerID, int amount)
    {
        if (playerID >= 0 && playerID < PlayerMoney.Count)
        {
            PlayerMoney[playerID] -= amount;
            UpdateMoneyUI(playerID);
        }
    }

    private void UpdateMoneyUI(int playerID)
    {
        _playerTexts[playerID].text = $"Player {playerID + 1}: ${PlayerMoney[playerID]}";
    }

    private void UpdateAllMoneyUI()
    {
        for (int i = 0; i < PlayerMoney.Count; i++)
        {
            UpdateMoneyUI(i);
        }
    }
}
