using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _playerControls;

    protected InputAction _jump;
    protected InputAction _move;
    protected InputAction _attack;

    protected void Awake()
    {
        _playerControls = new PlayerControls();
    }
    protected void OnEnable()
    {
        _jump = _playerControls.Player.Jump;
        _move = _playerControls.Player.Move;
        _attack = _playerControls.Player.Attack;

        _jump.Enable();
        _move.Enable();
        _attack.Enable();
    }
    protected void OnDisable()
    {
        _jump.Disable();
        _move.Disable();
        _attack.Disable();
    }
}