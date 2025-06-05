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

    [SerializeField] private Transform _canvasParent; // Parent object for borders
    [SerializeField] private Transform[] _borderPositions;

    //[SerializeField] private GoToBeginning _goToBeginningScript;

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

    public Transform positionP1;
    public Transform positionP2;
    public Transform positionP3;
    public Transform positionP4;

    private List<int> _playerScores = new List<int>();
    private List<Image> _spawnedBorders = new List<Image>();

    private ScoreManager _scoreManager;

    private float _waitForIncreaseScoreTimer = 0f;
    private float _lerpTime = 1f;

    private bool _canOrderBorders = false;
    private bool _hasSkipped;
    private bool _orderingHasFinished, _order1IsFinished, _order2IsFinished, _order3IsFinished, _order4IsFinished;

    //[HideInInspector] public bool _canContinue = false;

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
        
        
        if (Gamepad.current.buttonSouth.isPressed)
        {
            if (_orderingHasFinished)
            {
                if (Gamepad.current.buttonSouth.wasPressedThisFrame)
                {
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                _hasSkipped = true;
            }
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
                else _order1IsFinished = true;

                if (_order1IsFinished)
                    _orderingHasFinished = true;

                return; // Ensure second place border is initialized
            }

            else if (_thirdPlaceBorder == null)
            {
                if (_firstPlaceBorder.transform.position != _borderPositions[0].position)
                {
                    _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
                }
                else _order1IsFinished = true;


                if (_secondPlaceBorder.transform.position != _borderPositions[1].position)
                {
                    _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
                }
                else _order2IsFinished = true;

                if (_order1IsFinished && _order2IsFinished)
                    _orderingHasFinished = true;

                return; // Ensure third place border is initialized

            }

            else if (_fourthPlaceBorder == null)
            {
                if (_firstPlaceBorder.transform.position != _borderPositions[0].position)
                {
                    _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
                }
                else _order1IsFinished = true;

                if (_secondPlaceBorder.transform.position != _borderPositions[1].position)
                {
                    _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
                }
                else _order2IsFinished = true;

                if (_thirdPlaceBorder.transform.position != _borderPositions[2].position)
                {
                    _thirdPlaceBorder.transform.position = Vector3.Lerp(_thirdPlaceBorder.transform.position, _borderPositions[2].position, Time.deltaTime * _lerpTime);
                }
                else _order3IsFinished = true;

                if (_order1IsFinished && _order2IsFinished && _order3IsFinished)
                    _orderingHasFinished = true;

                return; // Ensure fourth place border is initialized
            }
            else
            {
                if (_firstPlaceBorder.transform.position != _borderPositions[0].position)
                {
                    _firstPlaceBorder.transform.position = Vector3.Lerp(_firstPlaceBorder.transform.position, _borderPositions[0].position, Time.deltaTime * _lerpTime);
                }
                else _order1IsFinished = true;

                if (_secondPlaceBorder.transform.position != _borderPositions[1].position)
                {
                    _secondPlaceBorder.transform.position = Vector3.Lerp(_secondPlaceBorder.transform.position, _borderPositions[1].position, Time.deltaTime * _lerpTime);
                }
                else _order2IsFinished = true;

                if (_thirdPlaceBorder.transform.position != _borderPositions[2].position)
                {
                    _thirdPlaceBorder.transform.position = Vector3.Lerp(_thirdPlaceBorder.transform.position, _borderPositions[2].position, Time.deltaTime * _lerpTime);
                }
                else _order3IsFinished = true;

                if (_fourthPlaceBorder.transform.position != _borderPositions[3].position)
                {
                    _fourthPlaceBorder.transform.position = Vector3.Lerp(_fourthPlaceBorder.transform.position, _borderPositions[3].position, Time.deltaTime * _lerpTime);
                }
                else _order4IsFinished = true;

                if (_order1IsFinished && _order2IsFinished && _order3IsFinished && _order4IsFinished)
                    _orderingHasFinished = true;
            }
        }
    }

    private void GetHighestScore()
    {
        var playerCount = Gamepad.all.Count;
        List<(int score, int index)> scoreList = new List<(int score, int index)>();

        for (int i = 0; i < playerCount; i++)
        {
            scoreList.Add((_scoreManager._playerScores[i], i));
        }

        scoreList.Sort((a, b) => b.score.CompareTo(a.score));

        if (scoreList.Count > 0)
        {
            _spawnedBorders[scoreList[0].index].GetComponent<RectTransform>().anchoredPosition =
                positionP1.GetComponent<RectTransform>().anchoredPosition;
        }

        if (scoreList.Count > 1)
        {
            _spawnedBorders[scoreList[1].index].GetComponent<RectTransform>().anchoredPosition =
                positionP2.GetComponent<RectTransform>().anchoredPosition;
        }

        if (scoreList.Count > 2)
        {
            _spawnedBorders[scoreList[2].index].GetComponent<RectTransform>().anchoredPosition =
                positionP3.GetComponent<RectTransform>().anchoredPosition;
        }

        if (scoreList.Count > 3)
        {
            _spawnedBorders[scoreList[3].index].GetComponent<RectTransform>().anchoredPosition =
                positionP4.GetComponent<RectTransform>().anchoredPosition;
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
            if (_hasSkipped)
            {
                _playerScores[i] = _scoreManager._playerScores[i];
            }

            if (_playerScores[i] < _scoreManager._playerScores[i])
            {
                IncreaseScoreValue(5, i);
            }

            if (_spawnedBorders[i] != null)
            {
                string label = $"Score: {_playerScores[i]}";

                //tie check
                int countSameScore = _scoreManager._playerScores.FindAll(score => score == _scoreManager._playerScores[i]).Count;
                if (countSameScore > 1)
                {
                    label += " (Tie)";
                }

                _spawnedBorders[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = label;
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