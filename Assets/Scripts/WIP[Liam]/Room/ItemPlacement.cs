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
    private int _itemRotation, _layerMask;

    // Update is called once per frame
    void Update()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_item != null) 
        {
            if (_item.CompareTag("Item/Big")) _layerMask = 1 << 6;
            if (_item.CompareTag("Item/Long"))
            {
                if (Mathf.Abs(_itemRotation) == 90)
                    _layerMask = 1 << 9;
                else
                    _layerMask = 1 << 8;
            }
            if (_item.CompareTag("Item/Small")) _layerMask = 1 << 7;

            _item.transform.localRotation = Quaternion.Euler(0, _itemRotation, 0);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
            {
                MoveObjectOnGrid("ShortGrid", "Item/Small");

                if (Mathf.Abs(_itemRotation) == 90)
                    MoveObjectOnGrid("LongGrid/Virt", "Item/Long");
                else
                    MoveObjectOnGrid("LongGrid/Hori", "Item/Long");

                MoveObjectOnGrid("BigGrid", "Item/Big");
            }
            else {Vector3 mousePos = Input.mousePosition; mousePos.z = _itemDistanceFromCamera; _item.transform.position = Camera.main.ScreenToWorldPoint(mousePos); }
        }
    }
    public void OnComfirm()
    {

        if (_item != null && _hit.collider != null)
        {

            PlaceObjectOnGrid("ShortGrid", "Item/Small");

            if (Mathf.Abs(_itemRotation) == 90)
                PlaceObjectOnGrid("LongGrid/Virt", "Item/Long");
            else
                PlaceObjectOnGrid("LongGrid/Hori", "Item/Long");

            PlaceObjectOnGrid("BigGrid", "Item/Big");

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
        int rotationValue;
        if (_item.CompareTag("Item/Long")) rotationValue = 90;
        else rotationValue = 45;
        return rotationValue;
    }

    private void MoveObjectOnGrid(string gridTag, string itemTag)
    {
        if (_item.CompareTag(itemTag) && _hit.transform.parent.CompareTag(gridTag))
        {
            _item.transform.localScale = new Vector3(1.5f * _hit.transform.localScale.x, 1.5f * _hit.transform.localScale.y, 1.5f * _hit.transform.localScale.z);
            _item.transform.localPosition = new Vector3(_hit.transform.position.x, _item.transform.lossyScale.y / 2, _hit.transform.position.z);
        }
    }

    private void PlaceObjectOnGrid(string gridTag, string itemTag)
    {
        if (_hit.transform.GetComponent<CubeIsTriggerable>().IsTriggerable && _item.CompareTag(itemTag) && _hit.transform.parent.CompareTag(gridTag))
        {
            GameObject item = Instantiate(_item, _hit.collider.transform.position, _item.transform.localRotation, _hit.transform);
            item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            item.GetComponent<Collider>().enabled = true;
            Destroy(_item);
            _itemRotation = 0;
        }
    }

}
