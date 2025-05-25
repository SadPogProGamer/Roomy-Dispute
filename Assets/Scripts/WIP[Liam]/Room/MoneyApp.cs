using UnityEngine;

public class MoneyApp : MonoBehaviour
{
    private int _buttonIndex, _previousButtonIndex;
    [SerializeField]
    private GameObject _buttonsParent, _playerPointer;
    // Update is called once per frame

    private void Start()
    {
        _buttonIndex = Random.Range(0, 3);
        _previousButtonIndex = -1;//-1 so that it doesnt do the while loop at the start
    }
    void Update()
    {
        while (_buttonIndex == _previousButtonIndex)
        {
            _buttonIndex = Random.Range(0, 3);
        }

        if (_buttonIndex != _previousButtonIndex)
        {
            for (int childIndex = 0; childIndex < _buttonsParent.transform.childCount; childIndex++)
            {
                if(childIndex == _buttonIndex)
                {
                    _buttonsParent.transform.GetChild(childIndex).gameObject.SetActive(true);
                }
                else { _buttonsParent.transform.GetChild(childIndex).gameObject.SetActive(false); }
            }
        }
    }


    public void OnUp()
    {
        CheckCorrectButton(0);
    }
    public void OnDown()
    {
        CheckCorrectButton(1);
    }
    public void OnLeft()
    {
        CheckCorrectButton(2);
    }
    public void OnRight()
    {
        CheckCorrectButton(3);
    }

    private void CheckCorrectButton(int buttonIndex)
    {
        if (_buttonIndex == buttonIndex)
        {
            GetComponent<ButtonSelect>().MoneyManager.GetComponent<MoneyManager>().IncreaseMoney(_playerPointer.GetComponent<PlayerPointer>().PlayerIndex, 10);
            _previousButtonIndex = _buttonIndex;
            _buttonIndex = Random.Range(0, 3);
        }
        else
            GetComponent<ButtonSelect>().MoneyManager.GetComponent<MoneyManager>().DecreaseMoney(_playerPointer.GetComponent<PlayerPointer>().PlayerIndex, 5);
    }
}
