using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed, _speedRotation;
    private float _angle, _calculatedTime, _startingPoint, _normalisedTime;
    private Vector3 _moveVector, _movement;
    CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    public void OnMove(InputValue value)
    {
        _moveVector = value.Get<Vector2>();
    }

    private void Update()
    {
        _movement = _speed * Time.deltaTime * new Vector3(_moveVector.x,0,_moveVector.y);

        if (_movement != Vector3.zero)
        {
            _angle = Mathf.Atan2(_moveVector.x, _moveVector.y) * Mathf.Rad2Deg;
            //transform.localRotation = Quaternion.Euler(0, _angle, 0);

            _calculatedTime = Quaternion.Angle(transform.localRotation, Quaternion.Euler(0, _angle, 0));
            _normalisedTime = Time.deltaTime * _speedRotation / _calculatedTime;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, _angle, 0), _normalisedTime);
        }

        _characterController.Move(_movement+Physics.gravity);
    }
}
