using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractability : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab, _moneyManager;
    [SerializeField]
    private int _playerIndexStartingFrom0;

    // Update is called once per frame
    void Update()
    {
        if (_moneyManager.GetComponent<MoneyManager>().PlayerMoney[_playerIndexStartingFrom0]>= _prefab.GetComponent<ItemStats>().Cost)
            GetComponent<Button>().interactable = true;
        else 
            GetComponent<Button>().interactable = false;
    }
}
