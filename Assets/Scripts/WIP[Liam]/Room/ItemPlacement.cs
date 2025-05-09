using UnityEngine;
using static UnityEditor.Progress;

public class ItemPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject _empty;
    public GameObject Item;
    private Ray _ray;
    private RaycastHit _hit;
    [SerializeField]
    private float _itemDistanceFromCamera;
    private int _itemRotation, _layerMask, _usedSpaces;
    private Transform _child;
    private bool _prefabIsNotInstantiated;


    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        _ray = Camera.main.ScreenPointToRay(screenPos);
        if (Item != null)
        {
            if (_prefabIsNotInstantiated)
            {
                Item = Instantiate(Item);
                _prefabIsNotInstantiated = false;
            }

            if (Item.CompareTag("Item/Big")) _layerMask = 1 << 6;
            if (Item.CompareTag("Item/Long"))
            {
                if (Mathf.Abs(_itemRotation) % 180 != 0)
                    _layerMask = 1 << 9;
                else
                    _layerMask = 1 << 8;
            }
            if (Item.CompareTag("Item/Small")) _layerMask = 1 << 7;
            if (Item.tag.Contains("Placable")) _layerMask = 1 << 10;

            Item.transform.localRotation = Quaternion.Euler(0, _itemRotation, 0);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
            {
                MoveObjectOnGrid("ShortGrid", "Item/Small");

                if (Mathf.Abs(_itemRotation) % 180 != 0)
                    MoveObjectOnGrid("LongGrid/Vert", "Item/Long");
                else
                    MoveObjectOnGrid("LongGrid/Hori", "Item/Long");

                MoveObjectOnGrid("BigGrid", "Item/Big");

                MovePlacableOnObject("Item/Big");
                MovePlacableOnObject("Item/Long");
            }
            else { screenPos.z = _itemDistanceFromCamera; Item.transform.position = Camera.main.ScreenToWorldPoint(screenPos); }
        }
        else _prefabIsNotInstantiated = true;
    }
    public void OnComfirm()
    {

        if (Item != null && _hit.collider != null)
        {

            PlaceObjectOnGrid("ShortGrid", "Item/Small");

            if (Mathf.Abs(_itemRotation) % 180 != 0)
                PlaceObjectOnGrid("LongGrid/Vert", "Item/Long");
            else
                PlaceObjectOnGrid("LongGrid/Hori", "Item/Long");

            PlaceObjectOnGrid("BigGrid", "Item/Big");

            PlacePlacableOnObject("Item/Big");
            PlacePlacableOnObject("Item/Long");
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

    public void OnCancel()
    {
        Destroy(Item);
    }
    private int GetRotationValue()
    {
        int rotationValue;
        if (Item.CompareTag("Item/Long")) rotationValue = 90;
        else rotationValue = 45;
        return rotationValue;
    }

    private void MoveObjectOnGrid(string gridTag, string itemTag)
    {
        if (Item.CompareTag(itemTag) && _hit.transform.parent.CompareTag(gridTag))
        {
            Item.transform.localScale = new Vector3(1.5f * _hit.transform.localScale.x, 1.5f * _hit.transform.localScale.y, 1.5f * _hit.transform.localScale.z);
            Item.transform.localPosition = new Vector3(_hit.transform.position.x, Item.transform.lossyScale.y / 2, _hit.transform.position.z);
        }
    }

    private void PlaceObjectOnGrid(string gridTag, string itemTag)
    {
        if (_hit.transform.GetComponent<CubeIsTriggerable>() != null && _hit.transform.GetComponent<CubeIsTriggerable>().IsTriggerable && Item.CompareTag(itemTag) && _hit.transform.parent.CompareTag(gridTag))
        {
            GameObject item = Instantiate(Item, _hit.collider.transform.position, Item.transform.localRotation, _hit.transform);
            item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            item.GetComponent<Collider>().enabled = true;
            item.GetComponent<ItemStats>().IsPlaced = true;
            Destroy(Item);
            _itemRotation = 0;
        }
    }

    private void MovePlacableOnObject(string objectTag)
    {
        if (Item.tag.Contains("Placable") && _hit.transform.CompareTag(objectTag))
        {
            if (_hit.transform.CompareTag("Item/Big"))
            {
                if (_hit.transform.childCount == 9)
                    _usedSpaces = 4;
                if (_hit.transform.childCount == 8)
                    _usedSpaces = 3;
                if (_hit.transform.childCount == 7)
                    _usedSpaces = 2;
                if (_hit.transform.childCount == 6)
                    _usedSpaces = 1;
                if (_hit.transform.childCount == 5)
                    _usedSpaces = 0;
            }
            if (_hit.transform.CompareTag("Item/Long"))
            {
                if (_hit.transform.childCount == 5)
                    _usedSpaces = 2;
                if (_hit.transform.childCount == 4)
                    _usedSpaces = 1;
                if (_hit.transform.childCount == 3)
                    _usedSpaces = 0;
            }
            _child = _hit.transform.GetChild(_usedSpaces);
            Item.transform.localScale = new Vector3(_hit.transform.parent.localScale.x * _hit.transform.localScale.x, _hit.transform.parent.localScale.y *_hit.transform.localScale.y , _hit.transform.parent.localScale.z * _hit.transform.localScale.z );
            Item.transform.localPosition = new Vector3(_child.transform.position.x, _child.transform.position.y + Item.transform.lossyScale.y / 4, _child.transform.position.z);
        }
    }
    private void PlacePlacableOnObject(string objectTag)
    {
        if (Item.tag.Contains("Placable") && _hit.transform.CompareTag(objectTag))
        {
            GameObject item;
            if (_hit.transform.CompareTag("Item/Big"))
            {
                if (Item.transform.CompareTag("Item/Placable/Small") && _hit.transform.childCount < 10)
                {
                    item = InstantiatePlacable();
                }
                if (Item.transform.CompareTag("Item/Placable/Medium") && _hit.transform.childCount < 9)
                {
                    item = InstantiatePlacable();
                    Instantiate(_empty, _hit.transform).GetComponent<PlacableEmpties>().PlacableItem = item;
                }
                if (Item.transform.CompareTag("Item/Placable/Big") && _hit.transform.childCount < 8)
                {
                    item = InstantiatePlacable();
                    Instantiate(_empty, _hit.transform).GetComponent<PlacableEmpties>().PlacableItem = item;
                    Instantiate(_empty, _hit.transform).GetComponent<PlacableEmpties>().PlacableItem = item;
                }
            }
            if (_hit.transform.CompareTag("Item/Long"))
            {
                if (Item.transform.CompareTag("Item/Placable/Small") && _hit.transform.childCount < 6)
                {
                    item = InstantiatePlacable();
                }
               
                if (Item.transform.CompareTag("Item/Placable/Medium") && _hit.transform.childCount < 5)
                {
                    item = InstantiatePlacable();
                    Instantiate(_empty, _hit.transform).GetComponent<PlacableEmpties>().PlacableItem = item;

                }

                if (Item.transform.CompareTag("Item/Placable/Big") && _hit.transform.childCount < 4)
                {
                    item = InstantiatePlacable();
                    Instantiate(_empty, _hit.transform).GetComponent<PlacableEmpties>().PlacableItem = item;
                    Instantiate(_empty, _hit.transform).GetComponent<PlacableEmpties>().PlacableItem = item;
                }
            }
        }
    }
    private GameObject InstantiatePlacable()
    {
        GameObject item;
        item = Instantiate(Item, _child.position, Item.transform.localRotation, _hit.transform);
        item.transform.localScale = new Vector3(1, 1, 1);
        item.GetComponent<Collider>().enabled = true;
        item.GetComponent<ItemStats>().IsPlaced = true;
        Destroy(Item);
        _itemRotation = 0;
        return item;
    }
}
