using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IncreaseScore : MonoBehaviour
{
    [SerializeField] private Image _borderPrefab;

    [SerializeField] private Transform _canvasParent; // Parent object for borders
    [SerializeField] private Transform[] _borderPositions;

    private int _highestScore = 0;
    private int _secondHighestScore = 0;
    private int _thirdHighestScore = 0;
    private int _fourthHighestScore = 0;

    private string _highestScorePlayer = string.Empty;
    private string _secondHighestScorePlayer = string.Empty;
    private string _thirdHighestScorePlayer = string.Empty;
    private string _fourthHighestScorePlayer = string.Empty;

    private Image _firstPlaceBorder;
    private Image _secondPlaceBorder;
    private Image _thirdPlaceBorder;
    private Image _fourthPlaceBorder;

    private List<int> _playerScores = new List<int>();
    private List<Image> _spawnedBorders = new List<Image>();

    private ScoreManager _scoreManager;

    private float _waitForIncreaseScoreTimer = 0f;
    private float _lerpTime = 1f;

    private bool _canOrderBorders = false;
    [HideInInspector] public bool _canContinue = false;

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

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            GameObject border = Instantiate(_borderPrefab.gameObject, _borderPositions[i].position, Quaternion.identity);

            Image borderImage = border.GetComponent<Image>();
            borderImage.color = _playerColors[i];
            _spawnedBorders.Add(borderImage);

            _spawnedBorders[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"Player {i + 1}";

            if (_playerScores.Count <= i)
            {
                _playerScores.Add(0);
            }
        }
        MakeCanvasParent();
    }

    void Update()
    {
        _waitForIncreaseScoreTimer += Time.deltaTime;

        if (_waitForIncreaseScoreTimer < 0.5f)
        {
            return;
        }

        UpdateScoreText();
        GetHighestScore();
        OrderBorders();
    }

    private void OrderBorders()
    {
        for (int i = 0; i < _spawnedBorders.Count; i++)
        {
            if (_playerScores[i] == _highestScore)
            {
                _canOrderBorders = true;
            }
        }

        if (_canOrderBorders)
        {
            if (_firstPlaceBorder == null)
            {              
                return; // Ensure first place border is initialized
            }

            else if (_secondPlaceBorder == null)
            {
                if (_firstPlaceBorder.transform.position != _borderPositions[0].position)
                {
                    _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
                }
                return; // Ensure second place border is initialized
            }

            else if (_thirdPlaceBorder == null)
            {
                if (_firstPlaceBorder.transform.position != _borderPositions[0].position)
                {
                    _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
                }

                if (_secondPlaceBorder.transform.position != _borderPositions[1].position)
                {
                    _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
                }
                return; // Ensure third place border is initialized
            }

            else if (_fourthPlaceBorder == null)
            {
                if (_firstPlaceBorder.transform.position != _borderPositions[0].position)
                {
                    _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
                }

                if (_secondPlaceBorder.transform.position != _borderPositions[1].position)
                {
                    _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
                }

                if (_thirdPlaceBorder.transform.position != _borderPositions[2].position)
                {
                    _thirdPlaceBorder.transform.position = Vector3.Lerp(_thirdPlaceBorder.transform.position, _borderPositions[2].position, Time.deltaTime * _lerpTime);
                }
                return; // Ensure fourth place border is initialized
            }
            else
            {
                if (_firstPlaceBorder.transform.position != _borderPositions[0].position)
                {
                    _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
                }

                if (_secondPlaceBorder.transform.position != _borderPositions[1].position)
                {
                    _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
                }

                if (_thirdPlaceBorder.transform.position != _borderPositions[2].position)
                {
                    _thirdPlaceBorder.transform.position = Vector3.Lerp(_thirdPlaceBorder.transform.position, _borderPositions[2].position, Time.deltaTime * _lerpTime);
                }

                if (_fourthPlaceBorder.transform.position != _borderPositions[3].position)
                {
                    _fourthPlaceBorder.transform.position = Vector3.Lerp(_fourthPlaceBorder.transform.position, _borderPositions[3].position, Time.deltaTime * _lerpTime);
                }
            }
        }
    }

    private void GetHighestScore()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (_scoreManager._playerScores[i] > _highestScore)
            {
                _fourthHighestScore = _thirdHighestScore;
                _thirdHighestScore = _secondHighestScore;
                _secondHighestScore = _highestScore;
                _highestScore = _scoreManager._playerScores[i];

                _fourthHighestScorePlayer = _thirdHighestScorePlayer;
                _thirdHighestScorePlayer = _secondHighestScorePlayer;
                _secondHighestScorePlayer = _highestScorePlayer;
                _highestScorePlayer = $"Player {i + 1}";

                _fourthPlaceBorder = _thirdPlaceBorder;
                _thirdPlaceBorder = _secondPlaceBorder;
                _secondPlaceBorder = _firstPlaceBorder;
                _firstPlaceBorder = _spawnedBorders[i];
            }
            else if (_scoreManager._playerScores[i] > _secondHighestScore && _scoreManager._playerScores[i] < _highestScore)
            {
                _fourthHighestScore = _thirdHighestScore;
                _thirdHighestScore = _secondHighestScore;
                _secondHighestScore = _scoreManager._playerScores[i];

                _fourthHighestScorePlayer = _thirdHighestScorePlayer;
                _thirdHighestScorePlayer = _secondHighestScorePlayer;
                _secondHighestScorePlayer = $"Player {i + 1}";

                _fourthPlaceBorder = _thirdPlaceBorder;
                _thirdPlaceBorder = _secondPlaceBorder;
                _secondPlaceBorder = _spawnedBorders[i];
            }
            else if (_scoreManager._playerScores[i] > _thirdHighestScore && _scoreManager._playerScores[i] < _secondHighestScore)
            {
                _fourthHighestScore = _thirdHighestScore;
                _thirdHighestScore = _scoreManager._playerScores[i];

                _fourthHighestScorePlayer = _thirdHighestScorePlayer;
                _thirdHighestScorePlayer = $"Player {i + 1}";

                _fourthPlaceBorder = _thirdPlaceBorder;
                _thirdPlaceBorder = _spawnedBorders[i];
            }
            else if (_scoreManager._playerScores[i] > _fourthHighestScore && _scoreManager._playerScores[i] < _thirdHighestScore)
            {
                _fourthHighestScore = _scoreManager._playerScores[i];

                _fourthHighestScorePlayer = $"Player {i + 1}";

                _fourthPlaceBorder = _spawnedBorders[i];
            }
        }
    }

    private void MakeCanvasParent()
    {
        for (int i = 0; i < _spawnedBorders.Count; i++)
        {
            if (_spawnedBorders[i] != null)
            {
                _spawnedBorders[i].transform.SetParent(_canvasParent, false);
                _spawnedBorders[i].transform.position = _borderPositions[i].position;
            }
            else
            {
                Debug.LogWarning($"Border for player {i} is null.");
            }
        }
    }

    private void UpdateScoreText()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {

            if (_playerScores[i] < _scoreManager._playerScores[i])
            {
                IncreaseScoreValue(5, i);
            }

            if (_spawnedBorders[i] != null)
            {
                _spawnedBorders[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Score: {_playerScores[i]}";
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