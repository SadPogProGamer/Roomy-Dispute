using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [Header("PlayerPointers")]
    [SerializeField] private GameObject _player1Pointer;

    [Header("PlayerPhones")]
    [SerializeField] private GameObject _player1Phone;


    [Header("UI Stuff")]
    public GameObject MoneyManager;


    [Header("App Icons")]
    [SerializeField] private GameObject _cashAppIcon;
    [SerializeField] private GameObject _sabotageIcon;

    [Header("Furniture Apps")]
    [SerializeField] private GameObject[] _cheapFurniture;
    [SerializeField] private GameObject[] _mediumFurniture;
    [SerializeField] private GameObject[] _expensiveFurniture;

    [Header("Sabotage Apps")]
    [SerializeField] private GameObject _fireApp;
    [SerializeField] private GameObject _bombApp;
    [SerializeField] private GameObject _targetApp;
    [SerializeField] private GameObject _breakApp;

    [Header("Event Timer")]
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private SabotageTool _sabotageTool;

    private GameObject _lastSelectedButton;
    private bool _canMove, _didLoop;

    private int _cheapFurnitureIndex;
    private int _mediumFurnitureIndex;
    private int _expensiveFurnitureIndex;

    private void Start()
    {
        DisableSabotageApps();
        DisableFurnitureApps();

        GiveRandomNumberForCheapFurniture();
        GiveRandomNumberForExpensiveFurniture();
        GiveRandomNumberForMediumFurniture();

        // Ensure the first selected button is set and valid
        if (_eventSystem != null && _cashAppIcon != null)
        {
            _eventSystem.SetSelectedGameObject(_cashAppIcon);
        }
    }

    private void Update()
    {
        if (Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex] != null && _player1Phone.activeSelf)
        {
            HandleInput();
            CheckCurrentSelectedButton();
            CheckCancelButton();
            SubmitCurrent();
        }
    }

    private void CheckCancelButton()
    {
        if (Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex].buttonEast.wasPressedThisFrame &&
            _eventSystem.currentSelectedGameObject != _cashAppIcon &&
            _eventSystem.currentSelectedGameObject != _sabotageIcon)
        {
            CancelAction();
        }
    }

    public void CancelAction()
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
        Vector2 dpadInput = Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex].dpad.ReadValue();

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

                if (currentSelected == _mediumFurniture[_mediumFurnitureIndex])
                {
                    _eventSystem.SetSelectedGameObject(_cheapFurniture[_cheapFurnitureIndex]);
                }

                if (currentSelected == _expensiveFurniture[_expensiveFurnitureIndex])
                {
                    _eventSystem.SetSelectedGameObject(_mediumFurniture[_mediumFurnitureIndex]);
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

                if (currentSelected == _cheapFurniture[_cheapFurnitureIndex])
                {
                    _eventSystem.SetSelectedGameObject(_mediumFurniture[_mediumFurnitureIndex]);
                }

                if (currentSelected == _mediumFurniture[_mediumFurnitureIndex])
                {
                    _eventSystem.SetSelectedGameObject(_expensiveFurniture[_expensiveFurnitureIndex]);
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
        }
    }

    public void OnCashAppButtonClick()
    {
        Debug.Log("Cash App Button Clicked");
        DisableBigApps();
        EnableFurnitureApps();
        _eventSystem.SetSelectedGameObject(_cheapFurniture[_cheapFurnitureIndex]);
        _lastSelectedButton = _cashAppIcon;
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
        _player1Pointer.GetComponent<MeshRenderer>().enabled = true;
        _player1Pointer.GetComponent<ItemPlacement>().enabled = true;
        _player1Pointer.GetComponent<PlayerPointer>().CanMove = true;

        GameObject spawnedItem = Instantiate(item);
        spawnedItem.GetComponent<ItemStats>().PlayerPhone = _player1Phone;
        foreach (Material material in spawnedItem.GetComponent<MeshRenderer>().materials)
        {
            material.color *= _player1Pointer.GetComponent<MeshRenderer>().material.color;
        }
        _player1Pointer.GetComponent<ItemPlacement>().Item = spawnedItem;
        DisableFurnitureApps();
        EnableBigApps();
        _eventSystem.SetSelectedGameObject(_cashAppIcon);
        _player1Phone.SetActive(false);
        MoneyManager.GetComponent<MoneyManager>().DecreaseMoney(_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex, Math.Abs(item.GetComponent<ItemStats>().Cost));

        GiveRandomNumberForCheapFurniture();
        GiveRandomNumberForMediumFurniture();
        GiveRandomNumberForExpensiveFurniture();
    }

    private void GiveRandomNumberForExpensiveFurniture()
    {
        _expensiveFurnitureIndex = (int)UnityEngine.Random.Range(0f, _expensiveFurniture.Length - 1);
    }

    private void GiveRandomNumberForMediumFurniture()
    {
        _mediumFurnitureIndex = (int)UnityEngine.Random.Range(0f, _mediumFurniture.Length - 1);
    }

    private void GiveRandomNumberForCheapFurniture()
    {
        _cheapFurnitureIndex = (int)UnityEngine.Random.Range(0f, _cheapFurniture.Length - 1);
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
        foreach (GameObject furniture in _cheapFurniture)
        {
            furniture.SetActive(false);
        }
        foreach (GameObject furniture in _mediumFurniture)
        {
            furniture.SetActive(false);
        }
        foreach (GameObject furniture in _expensiveFurniture)
        {
            furniture.SetActive(false);
        }
    }

    private void EnableFurnitureApps()
    {
        _cheapFurniture[_cheapFurnitureIndex].SetActive(true);
        _mediumFurniture[_mediumFurnitureIndex].SetActive(true);
        _expensiveFurniture[_expensiveFurnitureIndex].SetActive(true);
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

    private void SubmitCurrent()
    {
        if (Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex] != null && Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex].buttonSouth.wasPressedThisFrame)
        {
            _eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }

    }

}
