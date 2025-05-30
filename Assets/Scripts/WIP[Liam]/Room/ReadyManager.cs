using System.Collections.Generic;
using UnityEngine;

public class ReadyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _timer, _playerTextCanvas, _getReadyUI,_gameSource, _readySource;


    public List<GameObject> Players;
    private List<GameObject> _getReadyObjects = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timer.SetActive(false);
        _playerTextCanvas.SetActive(false);
        foreach (GameObject player in Players)
        {
            player.SetActive(false);
        }

        for (int i = 0; i < Players.Count; i++)
        {
            GameObject getReadyInstantiate = Instantiate(_getReadyUI);
            getReadyInstantiate.GetComponent<GetReadyUI>().PlayerIndex = i;
            getReadyInstantiate.GetComponent<GetReadyUI>().PlayerColor = Players[i].transform.GetChild(1).GetComponent<MeshRenderer>().material;

            getReadyInstantiate.transform.GetChild(0).localScale = new Vector3(getReadyInstantiate.transform.localScale.x / 2, getReadyInstantiate.transform.localScale.y /2);
            int yCheck, xCheck;
            
            if (i < 2)
                yCheck = 1;
            else
                yCheck = 0;
            
            if (!(i % 2 == 0))
                xCheck = 1;
            else
                xCheck = 0;

            getReadyInstantiate.transform.GetChild(0).localPosition += Camera.main.ViewportToScreenPoint(new Vector3(-.25f + (.5f * xCheck), -.25f + (.5f*yCheck)));
            _getReadyObjects.Add(getReadyInstantiate);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int readyPlayerCount = 0;
        foreach (GameObject getReadyUI in _getReadyObjects)
        {
            if (getReadyUI.GetComponent<GetReadyUI>().Ready)
            {
                readyPlayerCount++;
            }

            if (readyPlayerCount == _getReadyObjects.Count) 
                StartTheGame();

        }
    }

    private void StartTheGame()
    {
        _timer.SetActive(true);
        _playerTextCanvas.SetActive(true);
        foreach (GameObject player in Players)
        {
            player.SetActive(true);
            player.transform.GetChild(0).GetComponent<ButtonSelect>().CancelAction();
        }
        _gameSource.SetActive(true);
        
        Destroy(_readySource);
        foreach (GameObject getReadyUI in _getReadyObjects)
        {
            Destroy(getReadyUI);
        }
        Destroy(gameObject);
    }
}
