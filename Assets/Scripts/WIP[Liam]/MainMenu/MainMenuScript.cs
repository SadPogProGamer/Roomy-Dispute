using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    public void OnAnyButton()
    {
        SceneManager.LoadScene(1);
    }
}
