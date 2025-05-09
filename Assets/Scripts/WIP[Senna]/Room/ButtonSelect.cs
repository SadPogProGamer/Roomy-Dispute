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

    private InputAction _cancelAction;
    private InputAction _southAction;
    private InputAction _eastAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fireImage.enabled = false;
        _bombImage.enabled = false;

        _cancelAction = new InputAction("Cancel", binding: "<Gamepad>/buttonEast");
        _cancelAction.performed += OnCancel;
        _cancelAction.Enable();

        _southAction = new InputAction("South", binding: "<Gamepad>/buttonSouth");
        _southAction.performed += OnSelect;
        _southAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {

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
        _sabotageAppImage.enabled = false;
        _cashAppImage.enabled = false;
        ShowFurnitureImages();
    }

    private void ShowFurnitureImages()
    {
        // Implement showing furniture images logic here
    }

    public void IsSabotageAppPressed()
    {
        _sabotageAppImage.enabled = false;
        _cashAppImage.enabled = false;
        ShowSabotageImages();
    }

    private void ShowSabotageImages()
    {
        _fireImage.enabled = true;
        _bombImage.enabled = true;
    }

    void OnCancel(InputAction.CallbackContext context)
    {
        _sabotageAppImage.enabled = false;
        _cashAppImage.enabled = false;
    }
}
