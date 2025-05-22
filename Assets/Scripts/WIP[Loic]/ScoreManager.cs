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
    public static ScoreManager _instance;

    public int[] _playerScores = new int[4];  // Array for 4 players
    public TextMeshProUGUI[] _scoreTexts;     // Assign 4 TextMeshProUGUI elements, one for each player
    private PlayersInstantiate _playerInstantiate;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _playerInstantiate = Object.FindFirstObjectByType<PlayersInstantiate>();

        for (int i = 0; i < _scoreTexts.Length; i++)
        {
            _scoreTexts[i].color = Color.clear;
            UpdateScoreText(i);
        }

        for (int i = 0; i < _playerInstantiate._playerCount; i++)
        {
            _scoreTexts[i].color = _playerInstantiate._playerMaterials[i].color;
        }
    }

    public void AddPoints(int playerIndex, int points)
    {
        if (playerIndex < 0 || playerIndex >= _playerScores.Length) return;

        _playerScores[playerIndex] += points;
        UpdateScoreText(playerIndex);
    }

    private void UpdateScoreText(int playerIndex)
    {
        if (_scoreTexts != null && _scoreTexts.Length > playerIndex && _scoreTexts[playerIndex] != null)
            _scoreTexts[playerIndex].text = $"Player {playerIndex + 1} Score: {_playerScores[playerIndex]}";
    }
}
