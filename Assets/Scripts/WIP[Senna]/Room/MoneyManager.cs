using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int _initialMoney = 400;

    private PlayersInstantiate _playerInstantiate;

    private List<TextMeshProUGUI> _mainMenuMoneyText = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _moneyAppMoneyText = new List<TextMeshProUGUI>();

    [HideInInspector] public List<int> _playerMoney = new List<int>();

    private void Start()
    {
        _playerInstantiate = Object.FindFirstObjectByType<PlayersInstantiate>();

        for (int i = 0; i < _playerInstantiate._playerCount; i++)
        {
            GameObject player = _playerInstantiate.GetPlayer(i); // Assumes you added this method (see below)

            Transform mainMenuMoneyTextTransform = player.transform.Find("Phone/PhoneImage/ShoppingAppButton/MoneyText");
            Transform moneyAppMoneyTextTransform = player.transform.Find("Phone/PhoneImage/CashGameButtons/MoneyText");

            if (mainMenuMoneyTextTransform != null)
            {
                TextMeshProUGUI mainMenuMoneyText = mainMenuMoneyTextTransform.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI moneyAppMoneyText = moneyAppMoneyTextTransform.GetComponent<TextMeshProUGUI>();

                if (mainMenuMoneyText != null && moneyAppMoneyText != null)
                {
                    _playerMoney.Add(_initialMoney);
                    _mainMenuMoneyText.Add(mainMenuMoneyText);
                    _moneyAppMoneyText.Add(moneyAppMoneyText);
                    mainMenuMoneyText.text = $"Money: ${_initialMoney}";
                    moneyAppMoneyText.text = $"Money: ${_initialMoney}";
                    Debug.Log($"MoneyText initialized for player {i}");
                }
                else
                {
                    Debug.LogWarning($"MoneyText component missing on player {i}");
                    _mainMenuMoneyText.Add(null);
                    _moneyAppMoneyText.Add(null);
                    _playerMoney.Add(_initialMoney);
                }
            }
            else
            {
                Debug.LogWarning($"MoneyText not found on player {i}");
                _mainMenuMoneyText.Add(null);
                _moneyAppMoneyText.Add(null);
                _playerMoney.Add(_initialMoney);
            }
        }

        UpdateAllMoneyUI();
    }

    private void Update()
    {
        //// Press 1,2,3 or 4 based on player to increase money
        //for (int i = 0; i < _playerMoney.Count; i++)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha1 + i))
        //    {
        //        IncreaseMoney(i, 10);
        //    }
        //}

        UpdateAllMoneyUI();
    }

    public void IncreaseMoney(int playerID, int amount)
    {
        if (playerID >= 0 && playerID < _playerMoney.Count)
        {
            _playerMoney[playerID] += amount;
        }
    }

    public void DecreaseMoney(int playerID, int amount)
    {
        if (playerID >= 0 && playerID < _playerMoney.Count)
        {
            _playerMoney[playerID] -= amount;
        }
    }

    private void UpdateAllMoneyUI()
    {
        for (int i = 0; i < _playerMoney.Count; i++)
        {
            UpdateMoneyUI(i);
        }
    }

    private void UpdateMoneyUI(int playerID)
    {
        if (_mainMenuMoneyText[playerID] != null)
        {
            _mainMenuMoneyText[playerID].text = $"Money: ${_playerMoney[playerID]}";
        }

        if (_moneyAppMoneyText[playerID] != null)
        {
            _moneyAppMoneyText[playerID].text = $"Money: ${_playerMoney[playerID]}";
        }
    }
}
