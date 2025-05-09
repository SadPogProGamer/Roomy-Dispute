using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Image _cashAppImage;
    [SerializeField] private Image _sabotageAppImage;

    //[Header("Furniture Images")]
    //[SerializeField] private Image _furnitureImage;

    [Header("Sabotage Images")]
    [SerializeField] private Image _fireImage;
    [SerializeField] private Image _bombImage;
    [SerializeField] private Image _targetIcon;
    [SerializeField] private Image _breakIcon;

    private InputAction _cancelAction;
    private InputAction _selectAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideSabotageIcons();

        _cancelAction = new InputAction("Cancel", binding: "<Gamepad>/buttonEast");
        _cancelAction.performed += OnCancel;
        _cancelAction.Enable();

        _selectAction = new InputAction("South", binding: "<Gamepad>/buttonSouth");
        _selectAction.performed += OnSelect;
        _selectAction.Enable();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        // Toggle images based on which app is selected
        if (_cashAppImage.enabled)
        {
            IsCashAppPressed();
        }
        else if (_sabotageAppImage.enabled)
        {
            IsSabotageAppPressed();
        }
    }

    public void IsCashAppPressed()
    {
        HideBigIcons();
        ShowFurnitureImages();
    }

    private void ShowFurnitureImages()
    {
        // Implement showing furniture images logic here
    }

    public void IsSabotageAppPressed()
    {
        HideBigIcons();
        ShowSabotageImages();
    }

    private void ShowSabotageImages()
    {
        _fireImage.enabled = true;
        _bombImage.enabled = true;
        _targetIcon.enabled = true;
        _breakIcon.enabled = true;
    }

    void OnCancel(InputAction.CallbackContext context)
    {
        HideSabotageIcons();
        HideFurnitureIcons();
        ShowBigIcons();
    }

    private void HideFurnitureIcons()
    {

    }

    private void HideSabotageIcons()
    {
        _bombImage.enabled = false;
        _fireImage.enabled = false;
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
}