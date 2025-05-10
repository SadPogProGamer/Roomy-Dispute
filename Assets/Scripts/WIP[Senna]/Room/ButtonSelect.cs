using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Image _cashAppImage;
    [SerializeField] private Image _sabotageAppImage;

    [Header("Sabotage Images")]
    [SerializeField] private Image _fireIcon;
    [SerializeField] private Image _bombIcon;
    [SerializeField] private Image _targetIcon;
    [SerializeField] private Image _breakIcon;

    [Header("Event System")]
    [SerializeField] private EventSystem eventSystem;

    private InputAction _cancelAction;
    private InputAction _selectAction;

    private GameObject _currentSelected;
    private GameObject _lastSelected;

    void Start()
    {
        HideSabotageIcons();

        _cancelAction = new InputAction("Cancel", binding: "<Gamepad>/buttonEast");
        _cancelAction.performed += OnCancel;
        _cancelAction.Enable();

        _selectAction = new InputAction("South", binding: "<Gamepad>/buttonSouth");
        _selectAction.performed += OnSelect;
        _selectAction.Enable();

        // Initially, the selected object is the main SabotageApp icon
        _currentSelected = _cashAppImage.gameObject;
    }

    private void Update()
    {
        // Check which button is selected
        CurrentSelectedButton();

        HandleMovement();
        HandleActions();
    }

    private static void CurrentSelectedButton()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

            if (selectedButton != null)
            {
                Debug.Log("Currently selected button: " + selectedButton.name);
            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (_currentSelected == _sabotageAppImage.gameObject)
        {
            IsSabotageAppPressed();
        }
        else if (_currentSelected == _fireIcon.gameObject)
        {
            SelectFireIcon();
        }
    }

    private void IsCashAppPressed()
    {
        HideBigIcons();
        ShowFurnitureImages();
        _lastSelected = _cashAppImage.gameObject;
    }

    private void ShowFurnitureImages()
    {
        // Implement showing furniture images logic here
    }

    private void IsSabotageAppPressed()
    {
        HideBigIcons();
        ShowSabotageImages();
        _currentSelected = _fireIcon.gameObject;  // Set the current selection to the Fire icon
        _lastSelected = _sabotageAppImage.gameObject;
        eventSystem.SetSelectedGameObject(_currentSelected);  // Ensure the Fire icon is selected
    }

    private void ShowSabotageImages()
    {
        _fireIcon.enabled = true;
        _bombIcon.enabled = true;
        _targetIcon.enabled = true;
        _breakIcon.enabled = true;
    }

    void OnCancel(InputAction.CallbackContext context)
    {
        if (_currentSelected == _sabotageAppImage.gameObject | _currentSelected == _cashAppImage.gameObject)
        {
            return; 
            Debug.Log("Cancel button pressed on Sabotage or Cash App");
        }

        _currentSelected = _lastSelected;  // Return to the last selected object
        HideSabotageIcons();
        HideFurnitureIcons();
        ShowBigIcons();
        eventSystem.SetSelectedGameObject(_currentSelected);  // Ensure the last selected object is focused again
    }

    private void HideFurnitureIcons()
    {
        // Implement logic to hide furniture icons here
    }

    private void HideSabotageIcons()
    {
        _fireIcon.enabled = false;
        _bombIcon.enabled = false;
        _targetIcon.enabled = false;
        _breakIcon.enabled = false;
    }

    private void ShowBigIcons()
    {
        _sabotageAppImage.enabled = true;
        _cashAppImage.enabled = true;
    }

    private void HideBigIcons()
    {
        _sabotageAppImage.enabled = false;
        _cashAppImage.enabled = false;
    }

    private void SelectFireIcon()
    {
        // Add any specific behavior when the fire icon is selected
        Debug.Log("Fire Icon Selected");
    }

    private void HandleMovement()
    {
        Vector2 dpadInput = Gamepad.current.dpad.ReadValue();

        if (dpadInput != Vector2.zero)
        {
            if (dpadInput.y > 0)
            {
                NavigateUp();
            }
            else if (dpadInput.y < 0)
            {
                NavigateDown();
            }
            else if (dpadInput.x < 0)
            {
                NavigateLeft();
            }
            else if (dpadInput.x > 0)
            {
                NavigateRight();
            }
        }
    }

    private void NavigateUp()
    {
        if (_currentSelected != null)
        {
            SelectButton(_currentSelected.GetComponent<Selectable>().FindSelectableOnUp());
        }
    }

    private void NavigateDown()
    {
        if (_currentSelected != null)
        {
            SelectButton(_currentSelected.GetComponent<Selectable>().FindSelectableOnDown());
        }
    }

    private void NavigateLeft()
    {
        if (_currentSelected != null)
        {
            SelectButton(_currentSelected.GetComponent<Selectable>().FindSelectableOnLeft());
        }
    }

    private void NavigateRight()
    {
        if (_currentSelected != null)
        {
            SelectButton(_currentSelected.GetComponent<Selectable>().FindSelectableOnRight());
        }
    }

    private void SelectButton(Selectable selectable)
    {
        if (selectable != null)
        {
            eventSystem.SetSelectedGameObject(selectable.gameObject);
        }
    }

    private void HandleActions()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                if (_currentSelected != null)
                {
                    _currentSelected.GetComponent<Button>().onClick.Invoke();
                }
            }
            else if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                Debug.Log("Cancel button pressed");
            }
        }
    }
}