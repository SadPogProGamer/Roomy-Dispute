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
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        _ray = Camera.main.ScreenPointToRay(screenPos);
        if (_item != null)
        {
            if (_item.CompareTag("Item/Big")) _layerMask = 1 << 6;
            if (_item.CompareTag("Item/Long"))
            {
                if (Mathf.Abs(_itemRotation) % 180 != 0)
                    _layerMask = 1 << 9;
                else
                    _layerMask = 1 << 8;
            }
            if (_item.CompareTag("Item/Small")) _layerMask = 1 << 7;

            _item.transform.localRotation = Quaternion.Euler(0, _itemRotation, 0);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
            {
                MoveObjectOnGrid("ShortGrid", "Item/Small");

                if (Mathf.Abs(_itemRotation) % 180 != 0)
                    MoveObjectOnGrid("LongGrid/Vert", "Item/Long");
                else
                    MoveObjectOnGrid("LongGrid/Hori", "Item/Long");

                MoveObjectOnGrid("BigGrid", "Item/Big");
            }
            else { screenPos.z = _itemDistanceFromCamera; _item.transform.position = Camera.main.ScreenToWorldPoint(screenPos); }
        }
    }
    public void OnComfirm()
    {

        if (_item != null && _hit.collider != null)
        {

            PlaceObjectOnGrid("ShortGrid", "Item/Small");

            if (Mathf.Abs(_itemRotation) % 180 != 0)
                PlaceObjectOnGrid("LongGrid/Vert", "Item/Long");
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
            item.GetComponent<ItemStats>().IsPlaced = true;
            Destroy(_item);
            _itemRotation = 0;
        }
    }

}
