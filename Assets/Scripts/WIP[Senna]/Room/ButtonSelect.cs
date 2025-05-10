using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [Header("App Icons")]
    [SerializeField] private GameObject _cashAppIcon;
    [SerializeField] private GameObject _sabotageIcon;

    [Header("Furniture Apps")]
    [SerializeField] private GameObject[] _furnitureObjects;

    [Header("Sabotage Apps")]
    [SerializeField] private GameObject _fireApp;
    [SerializeField] private GameObject _bombApp;
    [SerializeField] private GameObject _targetApp;
    [SerializeField] private GameObject _breakApp;

    [Header("Event Timer")]
    [SerializeField] private EventSystem _eventSystem;

    private GameObject _lastSelectedButton;
<<<<<<< HEAD
    private int _currentFurnitureIndex = 0;  // To track the selected furniture item
=======
    private int _currentFurnitureIndex = 0;
    private bool _inFurnitureApp = false;
    private bool _inSabotageApp = false;
>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e

    private void Start()
    {
        DisableSabotageApps();
        DisableFurnitureApps();

        if (_eventSystem != null && _cashAppIcon != null)
        {
            _eventSystem.SetSelectedGameObject(_cashAppIcon);
<<<<<<< HEAD
=======
            _lastSelectedButton = _cashAppIcon;
>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e
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
<<<<<<< HEAD
=======
        _inFurnitureApp = false;
        _inSabotageApp = false;
>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e
    }

    private void CheckCurrentSelectedButton()
    {
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
        if (Gamepad.current == null) return;

        Vector2 dpadInput = Gamepad.current.dpad.ReadValue();

        if (dpadInput == Vector2.zero) return;

        if (dpadInput.y > 0)
            MoveSelection(Vector2.up);
        else if (dpadInput.y < 0)
            MoveSelection(Vector2.down);
        else if (dpadInput.x > 0)
            MoveSelection(Vector2.right);
        else if (dpadInput.x < 0)
            MoveSelection(Vector2.left);
    }

    private void MoveSelection(Vector2 direction)
    {
        GameObject currentSelected = _eventSystem.currentSelectedGameObject;
<<<<<<< HEAD

        if (currentSelected != null)
=======
        if (currentSelected == null) return;

        // Furniture navigation
        if (_inFurnitureApp)
        {
            if (direction == Vector2.up && _currentFurnitureIndex > 0)
            {
                _currentFurnitureIndex--;
                UpdateSelectedFurniture();
            }
            else if (direction == Vector2.down && _currentFurnitureIndex < _furnitureObjects.Length - 1)
            {
                _currentFurnitureIndex++;
                UpdateSelectedFurniture();
            }
            return;
        }

        // Sabotage navigation
        if (_inSabotageApp)
>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e
        {
            if (direction == Vector2.up)
            {
                if (currentSelected == _targetApp)
                    _eventSystem.SetSelectedGameObject(_fireApp);
                else if (currentSelected == _breakApp)
                    _eventSystem.SetSelectedGameObject(_bombApp);
<<<<<<< HEAD
                }
=======
                else if (currentSelected == _fireApp || currentSelected == _bombApp)
                    return; // Top-most, ignore up
>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e
            }

            else if (direction == Vector2.down)
            {
                if (currentSelected == _fireApp)
                    _eventSystem.SetSelectedGameObject(_targetApp);
                else if (currentSelected == _bombApp)
                    _eventSystem.SetSelectedGameObject(_breakApp);
<<<<<<< HEAD
                }
=======
                else if (currentSelected == _targetApp || currentSelected == _breakApp)
                    return; // Bottom-most, ignore down
>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e
            }

            else if (direction == Vector2.left)
            {
                if (currentSelected == _bombApp)
                    _eventSystem.SetSelectedGameObject(_fireApp);
                else if (currentSelected == _breakApp)
                    _eventSystem.SetSelectedGameObject(_targetApp);
            }

            else if (direction == Vector2.right)
            {
                if (currentSelected == _fireApp)
                    _eventSystem.SetSelectedGameObject(_bombApp);
                else if (currentSelected == _targetApp)
                    _eventSystem.SetSelectedGameObject(_breakApp);
            }

            return;
        }

        // Navigation between CashApp and SabotageIcon when in main menu
        if (!_inFurnitureApp && !_inSabotageApp)
        {
            if (direction == Vector2.down)
            {
                if (currentSelected == _cashAppIcon)
                    _eventSystem.SetSelectedGameObject(_sabotageIcon);
            }
            else if (direction == Vector2.up)
            {
                if (currentSelected == _sabotageIcon)
                    _eventSystem.SetSelectedGameObject(_cashAppIcon);
            }
        }
    }

<<<<<<< HEAD
=======
    private void UpdateSelectedFurniture()
    {
        if (_furnitureObjects.Length > 0)
        {
            _eventSystem.SetSelectedGameObject(_furnitureObjects[_currentFurnitureIndex]);
        }
    }

>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e
    public void OnCashAppButtonClick()
    {
        Debug.Log("Cash App Button Clicked");
        DisableBigApps();
        DisableSabotageApps(); // 🛠 Prevent overlap
        EnableFurnitureApps();
<<<<<<< HEAD
        _eventSystem.SetSelectedGameObject(_furnitureObjects[0]);  // Set the first item of furniture as selected
=======
        _inFurnitureApp = true;
        _inSabotageApp = false;
        _currentFurnitureIndex = 0;
        _eventSystem.SetSelectedGameObject(_furnitureObjects[0]);
>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e
        _lastSelectedButton = _cashAppIcon;
    }

    public void OnSabotageButtonClick()
    {
        Debug.Log("Sabotage Button Clicked");
        DisableBigApps();
        DisableFurnitureApps(); // 🛠 Prevent overlap
        EnableSabotageApps();
        _inSabotageApp = true;
        _inFurnitureApp = false;
        _eventSystem.SetSelectedGameObject(_fireApp);
        _lastSelectedButton = _sabotageIcon;
    }

<<<<<<< HEAD
    public void OnFireAppButtonClick()
    {
        Debug.Log("Fire App Button Clicked");
    }

    public void OnBombAppButtonClick()
    {
        Debug.Log("Bomb App Button Clicked");
    }

    public void OnTargetAppButtonClick()
    {
        Debug.Log("Target App Button Clicked");
    }

    public void OnBreakAppButtonClick()
    {
        Debug.Log("Break App Button Clicked");
    }

    public void OnBookshelfButtonClick()
    {
        Debug.Log("Bookshelf Button Clicked");
    }

    public void OnChairButtonClick()
    {
        Debug.Log("Chair Button Clicked");
    }
=======
    public void OnFireAppButtonClick() => Debug.Log("Fire App Button Clicked");
    public void OnBombAppButtonClick() => Debug.Log("Bomb App Button Clicked");
    public void OnTargetAppButtonClick() => Debug.Log("Target App Button Clicked");
    public void OnBreakAppButtonClick() => Debug.Log("Break App Button Clicked");
    public void OnBookshelfButtonClick() => Debug.Log("Bookshelf Button Clicked");
    public void OnChairButtonClick() => Debug.Log("Chair Button Clicked");
>>>>>>> f1e9cfe384ea55f0afd46dfacac21ef831e35f8e

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
        foreach (GameObject furniture in _furnitureObjects)
        {
            furniture.SetActive(true);
        }
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
