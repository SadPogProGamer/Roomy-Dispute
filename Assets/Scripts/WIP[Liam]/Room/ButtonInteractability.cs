using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractability : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab, _moneyManager, _playerIndexStartingFrom0;

    // Update is called once per frame
    void Update()
    {
        if (_moneyManager.GetComponent<ButtonSelect>().MoneyManager.GetComponent<MoneyManager>().PlayerMoney[_playerIndexStartingFrom0.GetComponent<PlayerPointer>().PlayerIndex]>= _prefab.GetComponent<ItemStats>().Cost)
            GetComponent<Button>().interactable = true;
        else 
            GetComponent<Button>().interactable = false;
    }
}
