using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Image _cashAppImage;
    [SerializeField] private Image _sabotageAppImage;

    private InputAction _cancelAction;
    private InputAction _southAction; // Input action for "A" button on Xbox / "Circle" on PS
    private InputAction _eastAction; // Input action for "B" button on Xbox / "Cross" on PS (Cancel)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Input action for cancel (Escape button)
        _cancelAction = new InputAction("Cancel", binding: "<Gamepad>/buttonEast"); // This listens for "B" (Xbox) or "Cross" (PS)
        _cancelAction.performed += OnCancel; // Subscribe to the cancel event
        _cancelAction.Enable(); // Enable the action

        // Input action for "A" button on Xbox / "Circle" button on PS (South button)
        _southAction = new InputAction("South", binding: "<Gamepad>/buttonSouth");
        _southAction.performed += OnSouthPressed; // Subscribe to the performed event
        _southAction.Enable(); // Enable the action
    }

    // Update is called once per frame
    void Update()
    {
        // You can add other update logic here if needed
    }

    // Called when the South button (A or Circle) is pressed
    private void OnSouthPressed(InputAction.CallbackContext context)
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
        // Implement showing sabotage images logic here
    }

    // Called when the cancel (B or Cross) button is pressed
    void OnCancel(InputAction.CallbackContext context)
    {
        // Re-enable both images
        _sabotageAppImage.enabled = true;
        _cashAppImage.enabled = true;
    }
}
