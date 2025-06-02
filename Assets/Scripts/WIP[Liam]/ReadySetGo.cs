using System.Collections.Generic;
using UnityEngine;

public class ReadySetGo : MonoBehaviour
{
    [SerializeField]
    private GameObject _ready, _set, _go;
    public GameObject GameTimer, PlayerTextCanvas;
    public List<GameObject> Players;

    private float _timer;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >.7f)
        {
            _ready.SetActive(true);
            _ready.transform.localScale += Vector3.one / 10 * Time.deltaTime;

            if (_timer > 2.3f)
            {
                _ready.SetActive(false);
                _set.SetActive(true);
                _set.transform.localScale += Vector3.one / 10 * Time.deltaTime;

                if (_timer > 3.7f)
                {
                    _set.SetActive(false);
                    _go.SetActive(true);
                    _go.transform.localScale += Vector3.one/10 * Time.deltaTime;
                    if (_timer > 6)
                    {
                        _go.SetActive(false);

                        GameTimer.SetActive(true);
                        PlayerTextCanvas.SetActive(true);
                        foreach (GameObject player in Players)
                        {
                            player.SetActive(true);
                            player.transform.GetChild(0).GetComponent<ButtonSelect>().CancelAction();
                        }
                        gameObject.SetActive(false);
                    }
                }
            }
        }

    }
}
