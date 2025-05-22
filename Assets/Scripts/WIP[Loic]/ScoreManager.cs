////using UnityEngine;
////using TMPro;
////using System.Collections.Generic;

////public class ScoreManager : MonoBehaviour
////{
////    [SerializeField] private TextMeshProUGUI[] _playerScoreTexts;

////    public List<int> PlayerScore = new List<int>();


////    private void Start()
////    {
////        for (int i = 0; i < _playerScoreTexts.Length; i++)
////        {
////            PlayerScore.Add(0);
////        }

////        UpdateAllScoreUI();
////    }


////    private void Update()
////    {
////        for (int i = 0; i < PlayerScore.Count; i++)
////        {
////            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
////            {
////                IncreaseScore(i, 10);
////            }
////        }
////    }

////    public void IncreaseScore(int playerID, int amount)
////    {
////        if (playerID >= 0 && playerID < PlayerScore.Count)
////        {
////            PlayerScore[playerID] += amount;
////            UpdateScoreUI(playerID);
////        }
////    }

////    public void DecreaseScore(int playerID, int amount)
////    {
////        if (playerID >= 0 && playerID < PlayerScore.Count)
////        {
////            PlayerScore[playerID] -= amount;
////            UpdateScoreUI(playerID);
////        }
////    }

////    private void UpdateScoreUI(int playerID)
////    {
////        _playerScoreTexts[playerID].text = $"Player {playerID + 1}: {PlayerScore[playerID]}";
////    }

////    private void UpdateAllScoreUI()
////    {
////        for (int i = 0; i < PlayerScore.Count; i++)
////        {
////            UpdateScoreUI(i);
////        }
////    }
////}
//using UnityEngine;
//using TMPro;

//public class ScoreManager : MonoBehaviour
//{
//    public static ScoreManager Instance;  // Singleton for easy access

//    public int playerScore = 0;
//    public TextMeshProUGUI scoreText;

//    private void Awake()
//    {
//        // Simple singleton pattern
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    private void Start()
//    {
//        UpdateScoreText();
//    }

//    public void AddPoints(int points)
//    {
//        playerScore += points;
//        UpdateScoreText();
//    }

//    private void UpdateScoreText()
//    {
//        if (scoreText != null)
//            scoreText.text = "Score: " + playerScore;
//    }
//}
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int[] playerScores = new int[4];  // Array for 4 players
    public TextMeshProUGUI[] scoreTexts;     // Assign 4 TextMeshProUGUI elements, one for each player

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            UpdateScoreText(i);
        }
    }

    public void AddPoints(int playerIndex, int points)
    {
        if (playerIndex < 0 || playerIndex >= playerScores.Length) return;

        playerScores[playerIndex] += points;
        UpdateScoreText(playerIndex);
    }

    private void UpdateScoreText(int playerIndex)
    {
        if (scoreTexts != null && scoreTexts.Length > playerIndex && scoreTexts[playerIndex] != null)
            scoreTexts[playerIndex].text = $"Player {playerIndex + 1} Score: {playerScores[playerIndex]}";
    }
}
