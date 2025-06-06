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
    [SerializeField]
    private ReadyManager _readyManager;

    private int _playerLimit = 4; // You can set this in the Inspector

    public Material[] _playerMaterials;
    public List<GameObject> SpawnedPlayers { get; private set; } = new List<GameObject>();
    [HideInInspector] public int _playerCount;

    void Awake()
    {
        _playerCount = Mathf.Min(Gamepad.all.Count, _playerLimit);

        for (int plyrcnt = 0; plyrcnt < _playerCount; plyrcnt++)
        {
            GameObject player = Instantiate(_prefab);
            player.transform.GetChild(0).GetComponent<ButtonSelect>().MoneyManager = _moneyManager;

            if (plyrcnt % 2 != 0)
            {
                var rt0 = player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
                var rt1 = player.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
                rt0.anchoredPosition = new Vector2(-rt0.anchoredPosition.x, rt0.anchoredPosition.y);
                rt1.anchoredPosition = new Vector2(-rt1.anchoredPosition.x, rt1.anchoredPosition.y);
            }
            if (plyrcnt > 1)
            {
                var rt0 = player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
                var rt1 = player.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
                rt0.anchoredPosition = new Vector2(rt0.anchoredPosition.x, -rt0.anchoredPosition.y);
                rt1.anchoredPosition = new Vector2(rt1.anchoredPosition.x, -rt1.anchoredPosition.y);
            }

            for (int border = 0; border < player.transform.GetChild(0).GetChild(1).childCount; border++)
                player.transform.GetChild(0).GetChild(1).GetChild(border).GetComponent<Image>().color = _playerMaterials[plyrcnt].color * Color.gray;

            player.transform.GetChild(1).GetComponent<PlayerPointer>().PlayerIndex = plyrcnt;
            player.transform.GetChild(1).GetComponent<PlayerPointer>().Room = _room;

            Renderer rend = player.GetComponentInChildren<Renderer>();
            if (rend != null && plyrcnt < _playerMaterials.Length)
                rend.material = _playerMaterials[plyrcnt];

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
