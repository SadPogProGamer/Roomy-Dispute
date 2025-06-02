using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayersInstantiate : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab, _moneyManager;
    [SerializeField]
    private GameObject[] _room;
    [HideInInspector] public int _playerCount;
    [SerializeField]
    private ReadyManager _readyManager;

    public Material[] _playerMaterials;

    public List<GameObject> SpawnedPlayers { get; private set; } = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerCount = Gamepad.all.Count;

        for (int plyrcnt = 0; plyrcnt < _playerCount; plyrcnt++)
        {
            GameObject player = Instantiate(_prefab);
            player.transform.GetChild(0).GetComponent<ButtonSelect>().MoneyManager = _moneyManager;
            if (plyrcnt % 2 != 0)
            {
                player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y);
                player.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-player.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition.x, player.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y);
            }
            if (plyrcnt > 1)
            {
                player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, -player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y);
                player.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(player.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition.x, -player.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y);
            }

            for (int border = 0; border < player.transform.GetChild(0).GetChild(1).childCount; border++)
                player.transform.GetChild(0).GetChild(1).GetChild(border).GetComponent<Image>().color = _playerMaterials[plyrcnt].color * Color.gray;
            player.transform.GetChild(1).GetComponent<PlayerPointer>().PlayerIndex = plyrcnt;
            player.transform.GetChild(1).GetComponent<PlayerPointer>().Room = _room;


            Renderer rend = player.GetComponentInChildren<Renderer>();
            if (rend != null && plyrcnt < _playerMaterials.Length)
            {
                rend.material = _playerMaterials[plyrcnt];
            }

            SpawnedPlayers.Add(player);
            _readyManager.Players.Add(player);
        }
    }
    public GameObject GetPlayer(int index)
    {
        if (index >= 0 && index < SpawnedPlayers.Count)
            return SpawnedPlayers[index];
        return null;
    }
}
