using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IncreaseScore : MonoBehaviour
{
    [SerializeField] private Image _borderPrefab;
    [SerializeField] private Transform _canvasParent;
    [SerializeField] private Transform[] _borderPositions;

    public RectTransform positionP1, positionP2, positionP3, positionP4;

    private int _highestScore = 0;
    private List<int> _playerScores = new List<int>();
    private List<Image> _spawnedBorders = new List<Image>();
    private ScoreManager _scoreManager;

    private float _waitForIncreaseScoreTimer = 0f;
    private float _lerpTime = 1f;

    private bool _canOrderBorders = false;
    private bool _hasSkipped;
    private bool _orderingHasFinished, _order1IsFinished, _order2IsFinished, _order3IsFinished, _order4IsFinished;

    private Image _firstPlaceBorder;
    private Image _secondPlaceBorder;
    private Image _thirdPlaceBorder;
    private Image _fourthPlaceBorder;

    private Color[] _playerColors = new Color[4]
    {
        new Color(0.6431373f, 0.2392157f, 0.6039216f),
        new Color(0.2392157f, 0.6470588f, 0.2627451f),
        new Color(0.2392157f, 0.3764706f, 0.6509804f),
        new Color(0.6509804f, 0.6235294f, 0.2470588f)
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
                _playerScores.Add(0);
        }

        MakeCanvasParent();
    }

    void Update()
    {
        _waitForIncreaseScoreTimer += Time.deltaTime;
        if (_waitForIncreaseScoreTimer < 0.5f) return;

        if (Gamepad.current.buttonSouth.isPressed)
        {
            if (_orderingHasFinished)
            {
                    SceneManager.LoadScene(0);
            }
            else
            {
                _hasSkipped = true;
            }
        }

        UpdateScoreText();

        // Only assign place borders and begin ordering when everyone reached final score
        if (!_orderingHasFinished)
        {
            if (AllScoresReachedFinal())
            {
                AssignPlaceBorders();
                OrderBorders();
            }
        }
    }

    private bool AllScoresReachedFinal()
    {
        for (int i = 0; i < _playerScores.Count; i++)
        {
            if (_playerScores[i] < _scoreManager._playerScores[i])
                return false;
        }
        return true;
    }

    private void AssignPlaceBorders()
    {
        List<(int score, int index)> scoreList = new List<(int, int)>();

        for (int i = 0; i < _scoreManager._playerScores.Count; i++)
        {
            scoreList.Add((_scoreManager._playerScores[i], i));
        }

        scoreList.Sort((a, b) => b.score.CompareTo(a.score)); // Descending order

        if (scoreList.Count > 0) _firstPlaceBorder = _spawnedBorders[scoreList[0].index];
        if (scoreList.Count > 1) _secondPlaceBorder = _spawnedBorders[scoreList[1].index];
        if (scoreList.Count > 2) _thirdPlaceBorder = _spawnedBorders[scoreList[2].index];
        if (scoreList.Count > 3) _fourthPlaceBorder = _spawnedBorders[scoreList[3].index];
    }

    private void OrderBorders()
    {
        if (_firstPlaceBorder == null) return;

        if (_secondPlaceBorder == null)
        {
            _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
            if (Vector3.Distance(_firstPlaceBorder.transform.position, _borderPositions[0].position) < 0.1f)
                _order1IsFinished = true;

            if (_order1IsFinished)
                _orderingHasFinished = true;

            return;
        }

        if (_thirdPlaceBorder == null)
        {
            _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
            if (Vector3.Distance(_firstPlaceBorder.transform.position, _borderPositions[0].position) < 0.1f)
                _order1IsFinished = true;

            _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
            if (Vector3.Distance(_secondPlaceBorder.transform.position, _borderPositions[1].position) < 0.1f)
                _order2IsFinished = true;

            if (_order1IsFinished && _order2IsFinished)
                _orderingHasFinished = true;

            return;
        }

        if (_fourthPlaceBorder == null)
        {
            _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
            if (Vector3.Distance(_firstPlaceBorder.transform.position, _borderPositions[0].position) < 0.1f)
                _order1IsFinished = true;

            _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
            if (Vector3.Distance(_secondPlaceBorder.transform.position, _borderPositions[1].position) < 0.1f)
                _order2IsFinished = true;

            _thirdPlaceBorder.transform.position = Vector3.Lerp(_thirdPlaceBorder.transform.position, _borderPositions[2].position, Time.deltaTime * _lerpTime);
            if (Vector3.Distance(_thirdPlaceBorder.transform.position, _borderPositions[2].position) < 0.1f)
                _order3IsFinished = true;

            if (_order1IsFinished && _order2IsFinished && _order3IsFinished)
                _orderingHasFinished = true;

            return;
        }

        _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
        if (Vector3.Distance(_firstPlaceBorder.transform.position, _borderPositions[0].position) < 0.1f)
            _order1IsFinished = true;

        _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
        if (Vector3.Distance(_secondPlaceBorder.transform.position, _borderPositions[1].position) < 0.1f)
            _order2IsFinished = true;

        _thirdPlaceBorder.transform.position = Vector3.Lerp(_thirdPlaceBorder.transform.position, _borderPositions[2].position, Time.deltaTime * _lerpTime);
        if (Vector3.Distance(_thirdPlaceBorder.transform.position, _borderPositions[2].position) < 0.1f)
            _order3IsFinished = true;

        _fourthPlaceBorder.transform.position = Vector3.Lerp(_fourthPlaceBorder.transform.position, _borderPositions[3].position, Time.deltaTime * _lerpTime);
        if (Vector3.Distance(_fourthPlaceBorder.transform.position, _borderPositions[3].position) < 0.1f)
            _order4IsFinished = true;

        if (_order1IsFinished && _order2IsFinished && _order3IsFinished && _order4IsFinished)
            _orderingHasFinished = true;
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
        }
    }

    private void UpdateScoreText()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (_hasSkipped)
                _playerScores[i] = _scoreManager._playerScores[i];

            if (_playerScores[i] < _scoreManager._playerScores[i])
                IncreaseScoreValue(5, i);

            if (_spawnedBorders[i] != null)
            {
                string label = $"Score: {_playerScores[i]}";
                int countSameScore = _scoreManager._playerScores.FindAll(score => score == _scoreManager._playerScores[i]).Count;
                if (countSameScore > 1)
                    label += " (Tie)";
                _spawnedBorders[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = label;
            }
        }
    }

    private void IncreaseScoreValue(int amount, int playerID)
    {
        int targetScore = _scoreManager._playerScores[playerID];
        _playerScores[playerID] = Mathf.Min(_playerScores[playerID] + amount, targetScore);
    }
}
