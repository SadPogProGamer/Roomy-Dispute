using UnityEngine;
using static UnityEditor.Progress;

public class ItemPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject _item;
    private Ray _ray;
    private RaycastHit _hit;
    [SerializeField]
    private float _itemDistanceFromCamera;
    private int _itemRotation;

    // Update is called once per frame
    void Update()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_item != null) 
        {
            _item.transform.localRotation = Quaternion.Euler(0, _itemRotation, 0);
            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.gameObject.transform.childCount == 0)
                {
                    _item.transform.localScale = new Vector3(1.5f * _hit.transform.localScale.x, 1.5f * _hit.transform.localScale.y, 1.5f * _hit.transform.localScale.z);
                    _item.transform.localPosition = new Vector3(_hit.transform.position.x, _item.transform.lossyScale.y/2, _hit.transform.position.z);
                }
            }
            else {Vector3 mousePos = Input.mousePosition; mousePos.z = _itemDistanceFromCamera; _item.transform.position = Camera.main.ScreenToWorldPoint(mousePos); }
        }
    }
    public void OnComfirm()
    {

        if (_item != null)
        {
            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.gameObject.transform.childCount == 0)
                {
                    GameObject item = Instantiate(_item, _hit.collider.transform.position, _item.transform.localRotation, _hit.transform);
                    item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    Destroy(_item);
                    _itemRotation = 0;
                }
            }
        }
    }

    public void OnRBumper()
    {
        _itemRotation -= GetRotationValue();
    }
    public void OnLBumper()
    {
        _itemRotation += GetRotationValue();
    }

    private int GetRotationValue()
    {
        int rotationValue = 45;
        //will change this with some if statements
        return rotationValue;
    }
}
