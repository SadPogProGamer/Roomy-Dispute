using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToBeginning : MonoBehaviour
{
    [SerializeField] private Image _continueImage;

    // Update is called once per frame
    void Update()
    {

        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            SceneManager.LoadScene(0);
        }
        
    }
}
