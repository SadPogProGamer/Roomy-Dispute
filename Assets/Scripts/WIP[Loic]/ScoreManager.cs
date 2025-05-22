//using UnityEngine;
//using TMPro;
//using System.Collections.Generic;

//public class ScoreManager : MonoBehaviour
//{
//    [SerializeField] private TextMeshProUGUI[] _playerScoreTexts;

//    public List<int> PlayerScore = new List<int>();


//    private void Start()
//    {
//        for (int i = 0; i < _playerScoreTexts.Length; i++)
//        {
//            PlayerScore.Add(0);
//        }

//        UpdateAllScoreUI();
//    }


//    private void Update()
//    {
//        for (int i = 0; i < PlayerScore.Count; i++)
//        {
//            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
//            {
//                IncreaseScore(i, 10);
//            }
//        }
//    }

//    public void IncreaseScore(int playerID, int amount)
//    {
//        if (playerID >= 0 && playerID < PlayerScore.Count)
//        {
//            PlayerScore[playerID] += amount;
//            UpdateScoreUI(playerID);
//        }
//    }

//    public void DecreaseScore(int playerID, int amount)
//    {
//        if (playerID >= 0 && playerID < PlayerScore.Count)
//        {
//            PlayerScore[playerID] -= amount;
//            UpdateScoreUI(playerID);
//        }
//    }

//    private void UpdateScoreUI(int playerID)
//    {
//        _playerScoreTexts[playerID].text = $"Player {playerID + 1}: {PlayerScore[playerID]}";
//    }

//    private void UpdateAllScoreUI()
//    {
//        for (int i = 0; i < PlayerScore.Count; i++)
//        {
//            UpdateScoreUI(i);
//        }
//    }
//}
