using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


public class PlayerPointer : MonoBehaviour
{
    public bool CanMove;
    public GameObject[] Room;
    [SerializeField]
    private float _speed;
    private Vector2 _moveVector;
    public int PlayerIndex;
    public void OnMove(InputValue value)
    {
        _moveVector = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            transform.localPosition += _speed * Time.fixedDeltaTime * new Vector3(_moveVector.x * transform.right.x, _moveVector.y * transform.up.y, _moveVector.y * transform.up.z).normalized;
            CheckOutOfBounds();
        }

    }

    private void CheckOutOfBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPosition.y > Screen.height) 
        {
            screenPosition.y = Screen.height;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.y < 0)
        {
            screenPosition.y = 0;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }
        
        if (screenPosition.x > Screen.width)
        {
            screenPosition.x = Screen.width;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.x < 0)
        {
            screenPosition.x = 0;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }

    public void StayOnFloor()
    {

    }

    public void StayOnWalls()
    {

    }
    public void OnComfirm()
    {
        if (TryGetComponent(out SabotageTool sabotage) && sabotage.enabled)
        {
            sabotage.OnComfirm(); // Now calls the no-parameter version
        }
    }


}
