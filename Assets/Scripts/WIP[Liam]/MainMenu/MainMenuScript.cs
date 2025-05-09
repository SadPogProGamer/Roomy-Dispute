using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private int _count;
    [SerializeField]
    private GameObject _titlescreenCanvas, _playerCountCanvas, _playersReadyCanvas, _button , _toggle1, _toggle2, _toggle3, _toggle4;

    public void GoToPlayerCountCanvas()
    {
        _titlescreenCanvas.SetActive(false);
        _playerCountCanvas.SetActive(true);
        _playersReadyCanvas.SetActive(false);
        _button.GetComponent<Button>().interactable = false;
        _button.GetComponent<Button>().Select();
    }
    public void GoToPlayerReadyCanvas(int playerCount)
    {
        _titlescreenCanvas.SetActive(false);
        _playerCountCanvas.SetActive(false);
        _playersReadyCanvas.SetActive(true);
        if (playerCount==2)
        {
            _toggle1.SetActive(true);
            _toggle2.SetActive(true);
            _toggle3.SetActive(false);
            _toggle4.SetActive(false);
        }
        if (playerCount == 3)
        {
            _toggle1.SetActive(true);
            _toggle2.SetActive(true);
            _toggle3.SetActive(true);
            _toggle4.SetActive(false);
        }
        if (playerCount == 4)
        {
            _toggle1.SetActive(true);
            _toggle2.SetActive(true);
            _toggle3.SetActive(true);
            _toggle4.SetActive(true);
        }
    }

    public void OnCancel()
    {
        if (_playersReadyCanvas.activeInHierarchy)
        {
            GoToPlayerCountCanvas();
            _button.GetComponent<Button>().interactable = true;
        }
    }

    public void OnAnyButton()
    {
        if (_titlescreenCanvas.activeInHierarchy)
        {
            GoToPlayerCountCanvas();
        }
    }

    public void OnSubmit()
    {
        if (_playerCountCanvas.activeInHierarchy)
        {
            _button.GetComponent<Button>().interactable = true;

        }
    }
    public void OnMove()
    {
        if (_playerCountCanvas.activeInHierarchy)
        {
            _button.GetComponent<Button>().interactable = true;

        }
    }
}
