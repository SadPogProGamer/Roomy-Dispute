using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager _instance;

    [HideInInspector] public List<int> _playerScores = new List<int>();

    private const int _initialScore = 0;

    private List<TextMeshProUGUI> _scoreTexts = new List<TextMeshProUGUI>();

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

        for (int i = 0; i < _playerInstantiate._playerCount; i++)
        {
            GameObject player = _playerInstantiate.GetPlayer(i); // Assumes you added this method (see below)

            Transform ScoreTextTransform = player.transform.Find("Phone/PhoneImage/CashAppButton/PointCount");

            if (ScoreTextTransform != null)
            {
                TextMeshProUGUI ScoreText = ScoreTextTransform.GetComponent<TextMeshProUGUI>();

                if (ScoreText != null)
                {
                    _playerScores.Add(_initialScore);
                    _scoreTexts.Add(ScoreText);
                    ScoreText.text = $"Score: {_initialScore}";
                    Debug.Log($"ScoreText initialized for player {i}");
                }
                else
                {
                    Debug.LogWarning($"ScoreText component missing on player {i}");
                    _scoreTexts.Add(null);
                    _playerScores.Add(_initialScore);
                }
            }
            else
            {
                Debug.LogWarning($"ScoreText not found on player {i}");
                _scoreTexts.Add(null);
                _playerScores.Add(_initialScore);
            }
        }
    }

    private void Update()
    {
        UpdateAllScoreUI();
    }

    public void AddPoints(int playerIndex, int points)
    {
        if (playerIndex < 0 || playerIndex >= _playerScores.Count) return;

        _playerScores[playerIndex] += points;
    }

    public void RemovePoints(int playerIndex, int points)
    {
        if (playerIndex < 0 || playerIndex >= _playerScores.Count) return;

        _playerScores[playerIndex] -= points;
    }


    private void UpdateAllScoreUI()
    {
        for (int i = 0; i < _playerInstantiate._playerCount; i++)
        {
            UpdateScoreUI(i);
        }
    }

    private void UpdateScoreUI(int playerID)
    {
        if (_scoreTexts[playerID] != null)
        {
            _scoreTexts[playerID].text = $"Score: {_playerScores[playerID]}";
        }
    }
}