using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MoneyApp : MonoBehaviour
{
    private int _buttonIndex, _previousButtonIndex;
    [SerializeField]
    private PlayerPointer _playerPointer;
    private bool _canMove, _isCountingDown;
    private float _revertingToWhiteTimer;
    // Update is called once per frame

    private void Start()
    {
        _buttonIndex = Random.Range(0, 4);
        _previousButtonIndex = -1;//-1 so that it doesnt do the while loop at the start
    }
    void Update()
    {
        while (_buttonIndex == _previousButtonIndex)
        {
            _buttonIndex = Random.Range(0, 4);
        }
        


        if (_buttonIndex != _previousButtonIndex)
        {
            for (int childIndex = 0; childIndex < transform.childCount-3; childIndex++)
            {
                if(childIndex == _buttonIndex)
                {
                    transform.GetChild(childIndex).gameObject.SetActive(true);
                }
                else { transform.GetChild(childIndex).gameObject.SetActive(false); }
            }
        }
        
        Vector2 dpadInput = Gamepad.all[_playerPointer.PlayerIndex].dpad.ReadValue();
        
        if (dpadInput == Vector2.zero) _canMove = true;

        if (_canMove)
        {
            if (dpadInput.y > 0)
            {
                CheckCorrectButton(0);
            }
            else if (dpadInput.y < 0)
            {
                CheckCorrectButton(1);
            }
            else if (dpadInput.x < 0)
            {
                CheckCorrectButton(2);
            }
            else if (dpadInput.x > 0)
            {
                CheckCorrectButton(3);
            }
        }

        if (_revertingToWhiteTimer <= 0 && _isCountingDown)
        {
            MakeTheArrowsTheChosenColor(Color.white);
            _isCountingDown = false;
        }

        if (_isCountingDown)
        {
            _revertingToWhiteTimer -= Time.deltaTime;
        }
    }

    private void CheckCorrectButton(int buttonIndex)
    {
        if (_buttonIndex == buttonIndex)
        {
            transform.parent.parent.GetComponent<ButtonSelect>().MoneyManager.GetComponent<MoneyManager>().IncreaseMoney(_playerPointer.PlayerIndex, 10);
            
            if (Random.Range(1,21) == 1)
            {
                transform.parent.parent.GetComponent<ButtonSelect>()._sabotageCount++;
            }
            _previousButtonIndex = _buttonIndex;
            _buttonIndex = Random.Range(0, 4);
            MakeTheArrowsTheChosenColor(Color.green);
            _isCountingDown = true;
            _revertingToWhiteTimer = .2f;
        }
        else
        {
            MakeTheArrowsTheChosenColor(Color.red);
            _isCountingDown = true;
            _revertingToWhiteTimer = .2f;
            transform.parent.parent.GetComponent<ButtonSelect>().MoneyManager.GetComponent<MoneyManager>().DecreaseMoney(_playerPointer.PlayerIndex, 30);
        }

        _canMove = false;
    }

    private void MakeTheArrowsTheChosenColor(Color clr)
    {
        for (int childIndex = 0; childIndex<transform.childCount-3; childIndex++)
        {
            transform.GetChild(childIndex).GetComponent<Image>().color = clr;
        }
    }
}
