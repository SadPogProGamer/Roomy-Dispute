using Unity.VisualScripting;
using UnityEngine;

public class CubeIsTriggerable : MonoBehaviour
{
    public bool IsTriggerable;
    private GameObject _currentCollider;

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag.Contains("Item"))
    //        IsTriggerable = true;
    //}

    private void Update()
    {
        if (_currentCollider.IsDestroyed())
                IsTriggerable = true;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Item"))
        {
            IsTriggerable = false;
            _currentCollider = other.gameObject;
        }
    }
}
