using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GetReadyUI : MonoBehaviour
{
    public bool Ready;
    [SerializeField]
    private Image[] _images;
    public Material PlayerColor;
    public int PlayerIndex;
    [SerializeField]
    private TextMeshProUGUI _playerText, _readyText;
    private string _originalReadyText;

    void Start()
    {
        foreach (Image image in _images)
        {
            image.color = PlayerColor.color;
        }
        _playerText.color = PlayerColor.color;
        _playerText.text = "Player " + (PlayerIndex+1); 
        _originalReadyText = _readyText.text;
        _readyText.color = PlayerColor.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all[PlayerIndex].buttonSouth.isPressed)
        {
            OnComfirm();
        }
        if (Gamepad.all[PlayerIndex].buttonEast.isPressed)
        {
            OnCancel();
        }
    }

    private void OnCancel()
    {
        Ready = false;
        _readyText.text = _originalReadyText;
    }

    private void OnComfirm()
    {
        Ready = true;
        _readyText.text = "READY!\n(press B to undo)";
    }
}
