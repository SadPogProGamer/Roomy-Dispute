using System;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] 
    private MoneyManager moneyManager; // Assign this in Inspector

    private float _startRoundTime = 31f;
    [SerializeField]
    private float _roundTime = 31f;

    public int _round = 1;


    [SerializeField]
    private TextMeshProUGUI _timerText; // Link this in the Inspector

    void Update()
    {
        _roundTime -= Time.deltaTime;

        if (_roundTime <= 0.0f && _round < 4)
        {
            _roundTime = _startRoundTime;
            _round++;
            AddMoneyForRound(_round);
        }
        else if (_roundTime <= 0.0f && _round >= 4 && _round != 9)
        {
            _roundTime = _startRoundTime - 5f * (_round - 3);
            _round++;
            AddMoneyForRound(_round);

        }
        else if (_roundTime <= 0.0f && _round == 9)
        {
            SceneManager.LoadScene("WinScreen");
            return;
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_roundTime / 60);
        int seconds = Mathf.FloorToInt(_roundTime % 60);
        _timerText.text = $"Round {_round}/9  {minutes:00}:{seconds:00}";
    }

    private void AddMoneyForRound(int round)
    {
        int amount = round switch
        {
            1 => 50,
            2 => 75,
            3 => 100,
            4 => 125,
            5 => 150,
            6 => 175,
            7 => 200,
            8 => 225,
            9 => 250,
            _ => 0
        };

        for (int i = 0; i < 4; i++)
        {
            //if (IsPlayerActive(i))
            //{
            //}
            moneyManager.IncreaseMoney(i, amount);
            Debug.Log($"Round {round}: Gave Player {i + 1} ${amount}");
        }
    }

    private bool IsPlayerActive(int index)
    {
        var field = typeof(PlayerManager).GetField("_playerIndexes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        if (field != null)
        {
            int[] indexes = (int[])field.GetValue(null);
            return indexes[index] != 0;
        }
        return false;
    }
}
