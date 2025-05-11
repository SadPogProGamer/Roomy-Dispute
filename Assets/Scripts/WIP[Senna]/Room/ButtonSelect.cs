using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [Header("PlayerPointers")]
    [SerializeField] private GameObject _player1Pointer; 
    [SerializeField] private GameObject _player2Pointer;
    [SerializeField] private GameObject _player3Pointer;
    [SerializeField] private GameObject _player4Pointer;
    
    [Header("PlayerPhones")]
    [SerializeField] private GameObject _player1Phone;
    [SerializeField] private GameObject _player2Phone;
    [SerializeField] private GameObject _player3Phone;
    [SerializeField] private GameObject _player4Phone;

    [Header("UI Stuff")]
    [SerializeField] private GameObject _moneyManager;


    [Header("App Icons")]
    [SerializeField] private GameObject _cashAppIcon;
    [SerializeField] private GameObject _sabotageIcon;

    [Header("Furniture Apps")]
    [SerializeField] private GameObject[] _furnitureObjects;  // Array of furniture objects

    [Header("Sabotage Apps")]
    [SerializeField] private GameObject _fireApp;
    [SerializeField] private GameObject _bombApp;
    [SerializeField] private GameObject _targetApp;
    [SerializeField] private GameObject _breakApp;

    [Header("Event Timer")]
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private SabotageTool _sabotageTool;

    private GameObject _lastSelectedButton;
    private int _currentFurnitureIndex = 0;  // To track the selected furniture item
    private bool _canMove, _didLoop;


    private void Start()
    {
        DisableSabotageApps();
        DisableFurnitureApps();

        // Ensure the first selected button is set and valid
        if (_eventSystem != null && _cashAppIcon != null)
        {
            _eventSystem.SetSelectedGameObject(_cashAppIcon);
        }
    }

    private void Update()
    {
        HandleInput();
        CheckCurrentSelectedButton();
        CheckCancelButton();
    }

    private void CheckCancelButton()
    {
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            CancelAction();
        }
    }

    private void CancelAction()
    {
        EnableBigApps();
        _eventSystem.SetSelectedGameObject(_lastSelectedButton);
        DisableSabotageApps();
        DisableFurnitureApps();
    }

    private void CheckCurrentSelectedButton()
    {
        // Check if currentSelectedGameObject is not null before accessing its name
        if (_eventSystem.currentSelectedGameObject != null)
        {
            Debug.Log("Current Selected: " + _eventSystem.currentSelectedGameObject.name);
        }
        else
        {
            Debug.Log("No object selected.");
        }
    }

    private void HandleInput()
    {
        Vector2 dpadInput = Gamepad.current.dpad.ReadValue();

        if (dpadInput.y > 0)  // Moving Up
        {
            MoveSelection(Vector2.up);
        }
        else if (dpadInput.y < 0)  // Moving Down
        {
            MoveSelection(Vector2.down);
        }
        else if (dpadInput.x > 0)  // Moving Right
        {
            MoveSelection(Vector2.right);
        }
        else if (dpadInput.x < 0)  // Moving Left
        {
            MoveSelection(Vector2.left);
        }
        else if (dpadInput.x == 0 && dpadInput.y == 0)
        {
            MoveSelection(Vector2.zero);
        }
    }

    private void MoveSelection(Vector2 direction)
    {
        GameObject currentSelected = _eventSystem.currentSelectedGameObject;

        if (currentSelected != null)
        {
            if (direction == Vector2.up)
            {
                if (currentSelected == _sabotageIcon)
                {
                    _eventSystem.SetSelectedGameObject(_cashAppIcon);
                }

                if (currentSelected == _targetApp)
                {
                    _eventSystem.SetSelectedGameObject(_fireApp);
                }

                if (currentSelected == _breakApp)
                {
                    _eventSystem.SetSelectedGameObject(_bombApp);
                }
            }

            else if (direction == Vector2.down)
            {
                if (currentSelected == _cashAppIcon)
                {
                    _eventSystem.SetSelectedGameObject(_sabotageIcon);
                }
                if (currentSelected == _fireApp)
                {
                    _eventSystem.SetSelectedGameObject(_targetApp);
                }
                if (currentSelected == _bombApp)
                {
                    _eventSystem.SetSelectedGameObject(_breakApp);
                }
            }

            else if (direction == Vector2.left)
            {
                if (currentSelected == _bombApp)
                {
                    _eventSystem.SetSelectedGameObject(_fireApp);
                }
                if (currentSelected == _breakApp)
                {
                    _eventSystem.SetSelectedGameObject(_targetApp);
                }
            }

            else if (direction == Vector2.right)
            {
                if (currentSelected == _fireApp)
                {
                    _eventSystem.SetSelectedGameObject(_bombApp);
                }
                if (currentSelected == _targetApp)
                {
                    _eventSystem.SetSelectedGameObject(_breakApp);
                }
            }
            
            if (_furnitureObjects[_currentFurnitureIndex].activeSelf)
            {
                if (direction == Vector2.zero) _canMove = true;

                if (_canMove)
                {
                    if (direction == Vector2.right)
                    {
                        _furnitureObjects[_currentFurnitureIndex].SetActive(false);
                        _currentFurnitureIndex++;
                        if (_currentFurnitureIndex >= _furnitureObjects.Length) _currentFurnitureIndex = 0;
                        _furnitureObjects[_currentFurnitureIndex].SetActive(true);
                        StartCoroutine(CheckChildrensInteractability());
                        _canMove = false;
                    }
                    else if (direction == Vector2.left)
                    {
                        _furnitureObjects[_currentFurnitureIndex].SetActive(false);
                        _currentFurnitureIndex--;
                        if (_currentFurnitureIndex < 0) _currentFurnitureIndex = _furnitureObjects.Length - 1;
                        _furnitureObjects[_currentFurnitureIndex].SetActive(true);
                        StartCoroutine(CheckChildrensInteractability());
                        _canMove = false;
                    }
                }

            }
        }
    }

    public void OnCashAppButtonClick()
    {
        
        Debug.Log("Cash App Button Clicked");
        DisableBigApps();
        EnableFurnitureApps();
        _currentFurnitureIndex = 0;
        StartCoroutine(CheckChildrensInteractability());  // Set the first interactable item of furniture as selected
        _lastSelectedButton = _cashAppIcon;
        
    }

    private IEnumerator CheckChildrensInteractability()
    {
        yield return null;
        for (int childIndex = 0; childIndex < _furnitureObjects[_currentFurnitureIndex].transform.childCount && _furnitureObjects[_currentFurnitureIndex].activeSelf; childIndex++)
        {
            if (_furnitureObjects[_currentFurnitureIndex].transform.GetChild(childIndex).GetComponent<Button>().interactable)
            {
                _eventSystem.SetSelectedGameObject(_furnitureObjects[_currentFurnitureIndex].transform.GetChild(childIndex).gameObject);
                break;
            }
        }
    }

    public void OnSabotageButtonClick()
    {
        Debug.Log("Sabotage Button Clicked");
        EnableSabotageApps();
        DisableBigApps();
        _eventSystem.SetSelectedGameObject(_fireApp);
        _lastSelectedButton = _sabotageIcon;
    }

    public void OnFireAppButtonClick()
    {
        Debug.Log("Fire App Button Clicked");
        _sabotageTool.FireSabotage();
    }

    public void OnBombAppButtonClick()
    {
        Debug.Log("Bomb App Button Clicked");
        _sabotageTool.BombSabotage();
    }

    public void OnTargetAppButtonClick()
    {
        Debug.Log("Target App Button Clicked");
        _sabotageTool.TargetSabotage();
    }

    public void OnBreakAppButtonClick()
    {
        Debug.Log("Break App Button Clicked");
        _sabotageTool.BreakSabotage();
    }


    public void OnItemButtonClickPlayer1(GameObject item)
    {
        _player1Pointer.SetActive(true);
        GameObject spawnedItem = Instantiate(item);
        spawnedItem.GetComponent<ItemStats>().PlayerPhone = _player1Phone;
        _player1Pointer.GetComponent<ItemPlacement>().Item = spawnedItem;
        DisableFurnitureApps();
        EnableBigApps();
        _eventSystem.SetSelectedGameObject(_cashAppIcon);
        _player1Phone.SetActive(false);
        _moneyManager.GetComponent<MoneyManager>().DecreaseMoney(0, Math.Abs(item.GetComponent<ItemStats>().Cost));
    }

    public void OnItemButtonClickPlayer2(GameObject item)
    {
        _player2Pointer.SetActive(true);
        GameObject spawnedItem = Instantiate(item);
        spawnedItem.GetComponent<ItemStats>().PlayerPhone = _player2Phone;
        _player2Pointer.GetComponent<ItemPlacement>().Item = spawnedItem;
        DisableFurnitureApps();
        EnableBigApps();
        _eventSystem.SetSelectedGameObject(_cashAppIcon);
        _player2Phone.SetActive(false);
        _moneyManager.GetComponent<MoneyManager>().DecreaseMoney(1, Math.Abs(item.GetComponent<ItemStats>().Cost));

    }
    public void OnItemButtonClickPlayer3(GameObject item)
    {
        _player3Pointer.SetActive(true);
        GameObject spawnedItem = Instantiate(item);
        item.GetComponent<ItemStats>().PlayerPhone = _player3Phone;
        _player3Pointer.GetComponent<ItemPlacement>().Item = spawnedItem;
        DisableFurnitureApps();
        EnableBigApps();
        _eventSystem.SetSelectedGameObject(_cashAppIcon);
        _player3Phone.SetActive(false);
        _moneyManager.GetComponent<MoneyManager>().DecreaseMoney(2, Math.Abs(item.GetComponent<ItemStats>().Cost));
    }
    public void OnItemButtonClickPlayer4(GameObject item)
    {
        _player4Pointer.SetActive(true);
        GameObject spawnedItem = Instantiate(item);
        spawnedItem.GetComponent<ItemStats>().PlayerPhone = _player4Phone;
        _player4Pointer.GetComponent<ItemPlacement>().Item = spawnedItem;
        DisableFurnitureApps();
        EnableBigApps();
        _eventSystem.SetSelectedGameObject(_cashAppIcon);
        _player4Phone.SetActive(false);
        _moneyManager.GetComponent<MoneyManager>().DecreaseMoney(3, Math.Abs(item.GetComponent<ItemStats>().Cost));
    }

    private void DisableSabotageApps()
    {
        _fireApp.SetActive(false);
        _bombApp.SetActive(false);
        _targetApp.SetActive(false);
        _breakApp.SetActive(false);
    }

    private void EnableSabotageApps()
    {
        _fireApp.SetActive(true);
        _bombApp.SetActive(true);
        _targetApp.SetActive(true);
        _breakApp.SetActive(true);
    }

    private void DisableFurnitureApps()
    {
        foreach (GameObject furniture in _furnitureObjects)
        {
            furniture.SetActive(false);
        }
    }

    private void EnableFurnitureApps()
    {
        _furnitureObjects[0].SetActive(true);
    }

    private void DisableBigApps()
    {
        _cashAppIcon.SetActive(false);
        _sabotageIcon.SetActive(false);
    }

    private void EnableBigApps()
    {
        _cashAppIcon.SetActive(true);
        _sabotageIcon.SetActive(true);
    }

    
}
