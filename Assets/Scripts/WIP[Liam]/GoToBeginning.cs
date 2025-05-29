using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToBeginning : MonoBehaviour
{
    private float _timer = 30;

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if ( _timer < 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
