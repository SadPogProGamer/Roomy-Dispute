using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToBeginning : MonoBehaviour
{
    [SerializeField] private IncreaseScore _increaseScore;

    [SerializeField] private Image _continueImage;

    // Update is called once per frame
    void Update()
    {
        if (_increaseScore._canContinue)
        {
            _continueImage.enabled = true;
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
