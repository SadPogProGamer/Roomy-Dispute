using UnityEngine;
using TMPro;

public class WinScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText; // Assign in Inspector

    void Start()
    {
        ShowWinner();
    }

    void ShowWinner()
    {
        if (ScoreManager._instance == null || winText == null)
        {
            Debug.LogError("ScoreManager or winText not set.");
            return;

        }

        int[] scores = ScoreManager._instance._playerScores;
        int winningPlayerIndex = 0;
        int highestScore = scores[0];

        for (int i = 1; i < scores.Length; i++)
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
