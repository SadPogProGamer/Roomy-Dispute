//using UnityEngine;
//using TMPro;

//public class ScoreManager : MonoBehaviour
//{
//    public static ScoreManager _instance;

//    public int[] _playerScores = new int[4];  // Array for 4 players
//    public TextMeshProUGUI[] _scoreTexts;     // Assign 4 TextMeshProUGUI elements, one for each player
//    private PlayersInstantiate _playerInstantiate;

//    private void Awake()
//    {
//        if (_instance == null)
//            _instance = this;
//        else
//            Destroy(gameObject);
//    }

//    private void Start()
//    {
//        _playerInstantiate = Object.FindFirstObjectByType<PlayersInstantiate>();

//        for (int i = 0; i < _scoreTexts.Length; i++)
//        {
//            _scoreTexts[i].color = Color.clear;
//            UpdateScoreText(i);
//        }

//        for (int i = 0; i < _playerInstantiate._playerCount; i++)
//        {
//            _scoreTexts[i].color = _playerInstantiate._playerMaterials[i].color;
//        }
//    }

//    public void AddPoints(int playerIndex, int points)
//    {
//        if (playerIndex < 0 || playerIndex >= _playerScores.Length) return;

//        _playerScores[playerIndex] += points;
//        UpdateScoreText(playerIndex);
//    }

//    private void UpdateScoreText(int playerIndex)
//    {
//        if (_scoreTexts != null && _scoreTexts.Length > playerIndex && _scoreTexts[playerIndex] != null)
//            _scoreTexts[playerIndex].text = $"Player {playerIndex + 1} Score: {_playerScores[playerIndex]}";
//    }
//}
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager _instance;

    public int[] _playerScores = new int[4];  // Scores for 4 players
    public TextMeshProUGUI[] _scoreTexts;     // Assign 4 UI elements in scene
    private PlayersInstantiate _playerInstantiate;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); //  Important to persist scores between scenes
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void RemovePoints(int playerIndex, int points)
    {
        if (playerIndex < 0 || playerIndex >= _playerScores.Length) return;

        _playerScores[playerIndex] -= points;
        UpdateScoreText(playerIndex);
    }

    private void UpdateScoreText(int playerIndex)
    {
        if (_scoreTexts != null && _scoreTexts.Length > playerIndex && _scoreTexts[playerIndex] != null)
            _scoreTexts[playerIndex].text = $"Player {playerIndex + 1} Score: {_playerScores[playerIndex]}";
    }
}
