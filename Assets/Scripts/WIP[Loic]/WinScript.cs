using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WinScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText; // Assign in Inspector

    private ScoreManager _scoreManager;

    void Start()
    {
        _scoreManager = FindFirstObjectByType<ScoreManager>();

        ShowWinner();
    }

    void ShowWinner()
    {
        List<int> scores = ScoreManager._instance._playerScores;
        int winningPlayerIndex = 0;
        int highestScore = scores[0];

        for (int i = 1; i < scores.Count; i++)
        {
            if (scores[i] > highestScore)
            {
                highestScore = scores[i];
                winningPlayerIndex = i;
            }
        }

        winText.text = $"Player {winningPlayerIndex + 1} Wins!";
    }
}
