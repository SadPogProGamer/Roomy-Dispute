using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSelectionTracker : MonoBehaviour
{
    private void Update()
    {
        // Check if any button is selected
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

            if (selectedButton != null)
            {
                Debug.Log("Currently selected button: " + selectedButton.name);
            }
        }
    }
}
