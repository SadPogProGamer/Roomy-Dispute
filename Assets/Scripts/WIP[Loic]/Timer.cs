using System;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] 
    private MoneyManager moneyManager; // Assign this in Inspector



    private float StartRoundTime = 61f;
    [SerializeField]
    private float RoundTime = 61f;

    private int Round = 1;

    [SerializeField]
    private TextMeshProUGUI timerText; // Link this in the Inspector

    private int playerMoney = 0;

    void Update()
    {
        RoundTime -= Time.deltaTime;

        if (RoundTime <= 0.0f && Round < 4)
        {
            SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);

            RoundTime = StartRoundTime - 10f * Round;
            Round++;
            AddMoneyForRound(Round);
        }
        else if (RoundTime <= 0.0f && Round >= 4 && Round != 9)
        {
            RoundTime = StartRoundTime - 30f - 5f * (Round - 3);
            Round++;
            AddMoneyForRound(Round);
        }
        else if (RoundTime <= 0.0f && Round == 9)
        {
            SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(RoundTime / 60);
        int seconds = Mathf.FloorToInt(RoundTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
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
