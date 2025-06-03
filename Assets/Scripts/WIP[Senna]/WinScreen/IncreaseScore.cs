using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IncreaseScore : MonoBehaviour
{
    [SerializeField] private Image _borderPrefab;
    [SerializeField] private Transform[] _borderPositions;

    private List<int> _playerScores = new List<int>();
    private List<Image> _spawnedBorders = new List<Image>();

    private ScoreManager _scoreManager;

    private float _waitForIncreaseScoreTimer = 0f;

    private Color[] _playerColors = new Color[4]
    {
        new Color(0.6431373f, 0.2392157f, 0.6039216f), // Player 1
        new Color(0.2392157f, 0.6470588f, 0.2627451f), // Player 2
        new Color(0.2392157f, 0.3764706f, 0.6509804f), // Player 3
        new Color(0.6509804f, 0.6235294f, 0.2470588f)  // Player 4
    };

    void Start()
    {
        _scoreManager = FindFirstObjectByType<ScoreManager>();

        _borderPrefab.transform.position = _borderPositions[0].position;
        _borderPrefab.color = _playerColors[0];
    }

    void Update()
    {
        _waitForIncreaseScoreTimer += Time.deltaTime;

        if (_waitForIncreaseScoreTimer < 0.5f)
            return;

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (_playerScores.Count <= i)
            {
                _playerScores.Add(0);
            }
            if (_spawnedBorders.Count <= i)
            {
                Image border = Instantiate(_borderPrefab, _borderPositions[i]);
                border.color = _playerColors[i];
                _spawnedBorders.Add(border);
            }
            if (_playerScores[i] < _scoreManager._playerScores[i])
            {
                IncreaseScoreValue(1, i);
            }
        }

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (_spawnedBorders[i] != null)
            {
                _spawnedBorders[i].GetComponentInChildren<TextMeshProUGUI>().text = $"Score: {_playerScores[i]}";
            }
            else
            {
                Debug.LogWarning($"Border for player {i} is null.");
            }
        }
    }

    private void IncreaseScoreValue(int amount, int playerID)
    {
        int targetScore = _scoreManager._playerScores[playerID];
        _playerScores[playerID] = Mathf.Min(_playerScores[playerID] + amount, targetScore);
    }
}