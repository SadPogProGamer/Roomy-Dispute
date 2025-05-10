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

    [Header("Sabotage Apps")]
    [SerializeField] private GameObject _fireApp;
    [SerializeField] private GameObject _bombApp;
    [SerializeField] private GameObject _targetApp;
    [SerializeField] private GameObject _breakApp;

    [Header("Event Timer")]
    [SerializeField] private EventSystem _eventSystem;

    private GameObject _lastSelectedButton;

    private void Start()
    {
        DisableSabotageApps();

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
        _eventSystem.SetSelectedGameObject(_lastSelectedButton);
        EnableBigApps();
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

        if (dpadInput.y > 0)
        {
            MoveSelection(Vector2.up);
        }
        else if (dpadInput.y < 0)
        {
            MoveSelection(Vector2.down);
        }
        else if (dpadInput.x > 0)
        {
            MoveSelection(Vector2.right);
        }
        else if (dpadInput.x < 0)
        {
            MoveSelection(Vector2.left);
        }
    }

    private void MoveSelection(Vector2 direction)
    {
        GameObject currentSelected = _eventSystem.currentSelectedGameObject;
        if (currentSelected != null)
        {
            Navigation navigation = currentSelected.GetComponent<UnityEngine.UI.Button>().navigation;
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
        }
    }

    public void OnCashAppButtonClick()
    {
        Debug.Log("Cash App Button Clicked");
        DisableBigApps();
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

    }

    private void EnableFurnitureApps()
    {

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