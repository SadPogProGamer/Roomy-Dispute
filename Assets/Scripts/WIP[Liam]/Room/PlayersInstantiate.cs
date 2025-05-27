using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersInstantiate : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab, _moneyManager;
    [HideInInspector] public int _playerCount;
    [SerializeField]
    private ReadyManager _readyManager;

    public Material[] _playerMaterials;

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
            }
            if (plyrcnt > 1)
            {
                player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, -player.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y);
            }
            player.transform.GetChild(1).GetComponent<PlayerPointer>().PlayerIndex = plyrcnt;

            Renderer rend = player.GetComponentInChildren<Renderer>();
            if (rend != null && plyrcnt < _playerMaterials.Length)
            {
                rend.material = _playerMaterials[plyrcnt];
            }

            _readyManager.Players.Add(player);
        }
    }
}
