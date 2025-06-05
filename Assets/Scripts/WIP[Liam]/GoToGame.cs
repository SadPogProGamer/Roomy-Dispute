using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour
{



    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            SceneManager.LoadScene(2);
        }
    }
}
