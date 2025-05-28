//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Progress;

public class ItemPlacement : MonoBehaviour
{
    [SerializeField]
    private Vector3 _originPoint;
    public Vector3 OriginPointFloor, OriginPointWall;
    [SerializeField]
    private GameObject _empty;
    public GameObject Item;
    private Ray _ray;
    private RaycastHit _hit;
    [SerializeField]
    private float _itemDistanceFromCamera;
    private int _itemRotation, _layerMask, _usedSpaces;
    private Transform _child;

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        _ray = Camera.main.ScreenPointToRay(screenPos);
        if (Item != null)
        {
            if (Item.CompareTag("Item/Big"))
            {
                _layerMask = 1 << 6;
            }
            if (Item.CompareTag("Item/Long"))
            {
                if (Mathf.Abs(_itemRotation) % 180 != 0)
                {
                    _layerMask = 1 << 9;

                }
                else
                {
                    _layerMask = 1 << 8;
                }
            }
            if (Item.CompareTag("Item/Small")) _layerMask = 1 << 7;
            if (Item.tag.Contains("Placable")) _layerMask = 1 << 10;

            if (Item.CompareTag("Item/Wall/Long"))
            {
                if (Mathf.Abs(_itemRotation) % 180 == 0)
                    _layerMask = 1 << 13;
                else
                    _layerMask = 1 << 12;
            }
            if (Item.CompareTag("Item/Wall/Small")) _layerMask = 1 << 11;

            
            
            
            
            if (Item.tag.Contains("Wall"))
            {
                Item.transform.localRotation = Quaternion.Euler(0, 0, _itemRotation);
                
                int awayFromHori = 40;
                int awayFromVert = 30;
                if (Item.CompareTag("Item/Wall/Long"))
                {
                    if (Mathf.Abs(_itemRotation) % 180 != 0)
                    {
                        awayFromVert = 110;
                        awayFromHori = 50;

                    }
                    else
                    {
                        awayFromVert = 40;
                        awayFromHori = 85;
                    }
                }
                GetComponent<PlayerPointer>().StayOnWalls(awayFromHori, awayFromVert);
            }
            else
            {
                Item.transform.localRotation = Quaternion.Euler(0, _itemRotation, 0);

                int awayFromHori = 15;
                int awayFromVert = 15;
                if (Item.CompareTag("Item/Long"))
                {
                    if (Mathf.Abs(_itemRotation) % 180 != 0)
                    {
                        awayFromHori = 70;
                    }
                    else
                    {
                        awayFromVert = 70;
                    }
                }
                else if (Item.CompareTag("Item/Big"))
                {
                    awayFromHori = 70;
                    awayFromVert = 70;
                }

                GetComponent<PlayerPointer>().StayOnFloor(awayFromHori, awayFromVert);
            }





            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
            {
                //floorstuff
                MoveObjectOnGrid("ShortGrid", "Item/Small");

                if (Mathf.Abs(_itemRotation) % 180 != 0)
                    MoveObjectOnGrid("LongGrid/Vert", "Item/Long");
                else
                    MoveObjectOnGrid("LongGrid/Hori", "Item/Long");

                MoveObjectOnGrid("BigGrid", "Item/Big");

                //wallstuff
                if (Mathf.Abs(_itemRotation) % 180 == 0)
                    MoveObjectOnGrid("LongGridWall/Vert", "Item/Wall/Long");
                else
                    MoveObjectOnGrid("LongGridWall/Hori", "Item/Wall/Long");

                MoveObjectOnGrid("ShortGridWall", "Item/Wall/Small");

                //placable
                MovePlacableOnObject("Item/Big");
                MovePlacableOnObject("Item/Long");
                MovePlacableOnObject("Item/Small");
            }
            else { screenPos.z = _itemDistanceFromCamera; Item.transform.position = Camera.main.ScreenToWorldPoint(screenPos); }
        }
    }
    public void OnComfirm()
    {

        if (Item != null && _hit.collider != null)
        {
            //floorstuff
            PlaceObjectOnGrid("ShortGrid", "Item/Small");

            if (Mathf.Abs(_itemRotation) % 180 != 0)
                PlaceObjectOnGrid("LongGrid/Vert", "Item/Long");
            else
                PlaceObjectOnGrid("LongGrid/Hori", "Item/Long");

            PlaceObjectOnGrid("BigGrid", "Item/Big");

            //wallstuff
            PlaceObjectOnGrid("ShortGridWall", "Item/Wall/Small");

            if (Mathf.Abs(_itemRotation) % 180 == 0)
                PlaceObjectOnGrid("LongGridWall/Vert", "Item/Wall/Long");
            else
                PlaceObjectOnGrid("LongGridWall/Hori", "Item/Wall/Long");

            //placable
            PlacePlacableOnObject("Item/Big");
            PlacePlacableOnObject("Item/Long");
            PlacePlacableOnObject("Item/Small");
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

    public void OnReturn()
    {
        if (GetComponent<ItemPlacement>().enabled)
        {
            Item.GetComponent<ItemStats>().PlayerPhone.SetActive(true);
            Item.GetComponent<ItemStats>().PlayerPhone.transform.parent.GetComponent<ButtonSelect>().CancelAction();
            Item.GetComponent<ItemStats>().PlayerPhone.transform.parent.GetComponent<ButtonSelect>().MoneyManager.GetComponent<MoneyManager>().IncreaseMoney(GetComponent<PlayerPointer>().PlayerIndex, Item.GetComponent<ItemStats>().Cost);
            //Item.GetComponent<ItemStats>().PlayerPhone.transform.parent.GetComponent<ButtonSelect>().ScoreManager.GetComponent<ScoreManager>().DecreaseScore(GetComponent<PlayerPointer>().PlayerIndex, Item.GetComponent<ItemStats>().Points);

            Destroy(Item);
            SetPointerBackToOrigin();
        }
    }
    private int GetRotationValue()
    {
        int rotationValue;
        if (Item.tag.Contains("Item") && Item.tag.Contains("Long")) rotationValue = 90;
        else rotationValue = 45;
        return rotationValue;
    }

    private void MoveObjectOnGrid(string gridTag, string itemTag)
    {
        if (Item.CompareTag(itemTag) && _hit.transform.parent.CompareTag(gridTag))
        {
            if (Item.tag.Contains("Wall"))
            {
                Item.transform.localScale = new Vector3(1.5f * _hit.transform.localScale.x, 1.5f * _hit.transform.localScale.x, 1.5f * _hit.transform.localScale.x);
                Item.transform.localRotation = Quaternion.Euler(0, _hit.transform.parent.localEulerAngles.y, _itemRotation);
                if (_hit.transform.parent.localEulerAngles.y == 90)
                    Item.transform.localPosition = new Vector3(_hit.transform.position.x + Item.transform.lossyScale.z / 2, _hit.transform.position.y, _hit.transform.position.z);
                else if (_hit.transform.parent.localEulerAngles.y == 270)
                    Item.transform.localPosition = new Vector3(_hit.transform.position.x - Item.transform.lossyScale.z / 2, _hit.transform.position.y, _hit.transform.position.z);
                else
                    Item.transform.localPosition = new Vector3(_hit.transform.position.x, _hit.transform.position.y, _hit.transform.position.z + Item.transform.lossyScale.z / 2);
            }
            else
            {
                Item.transform.localScale = new Vector3(1.5f * _hit.transform.localScale.x, 1.5f * _hit.transform.localScale.y, 1.5f * _hit.transform.localScale.z);
                Item.transform.localPosition = new Vector3(_hit.transform.position.x, Item.transform.lossyScale.y / 2, _hit.transform.position.z);
            }
        }
    }

    private void PlaceObjectOnGrid(string gridTag, string itemTag)
    {
        if (_hit.transform.GetComponent<CubeIsTriggerable>() != null && _hit.transform.GetComponent<CubeIsTriggerable>().IsTriggerable && Item.CompareTag(itemTag) && _hit.transform.parent.CompareTag(gridTag))
        {
            GameObject item = Instantiate(Item, _hit.collider.transform.position, Item.transform.localRotation, _hit.transform);

            if (Item.tag.Contains("Wall"))
            {
                if (Mathf.Abs(_itemRotation) % 180 != 0)
                    item.transform.localScale = new Vector3(1.5f, 1.4f, 1.5f);
                else
                    item.transform.localScale = new Vector3(1.5f, 1.5f / _hit.transform.localScale.y * _hit.transform.localScale.x, 1.5f / _hit.transform.localScale.z * _hit.transform.localScale.x);
            }
            else
            {
                item.transform.localScale = new Vector3(
                    1.5f,
                    1.5f / _hit.transform.localScale.y * _hit.transform.localScale.x,
                    1.5f / _hit.transform.localScale.z * _hit.transform.localScale.x
                );
            }

            if (item.GetComponent<Collider>() != null)
                item.GetComponent<Collider>().enabled = true;
            item.GetComponent<ItemStats>().IsPlaced = true;
            Destroy(Item);
            _itemRotation = 0;
            SetPointerBackToOrigin();
            


        }
    }


    private void MovePlacableOnObject(string objectTag)
    {
        if (Item.tag.Contains("Placable") && _hit.transform.CompareTag(objectTag))
        {
            if (_hit.transform.CompareTag("Item/Small"))
            {
                if (_hit.transform.childCount == 3)
                    _usedSpaces = 1;
                if (_hit.transform.childCount == 2)
                    _usedSpaces = 0;
            }
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
            Item.transform.localScale = new Vector3(_hit.transform.parent.localScale.x * _hit.transform.localScale.x, _hit.transform.parent.localScale.y * _hit.transform.localScale.y, _hit.transform.parent.localScale.z * _hit.transform.localScale.z);
            Item.transform.localPosition = new Vector3(_child.transform.position.x, _child.transform.position.y + Item.transform.lossyScale.y / 4, _child.transform.position.z);
        }
    }
    private void PlacePlacableOnObject(string objectTag)
    {
        if (Item.tag.Contains("Placable") && _hit.transform.CompareTag(objectTag))
        {
            GameObject item;
            if (_hit.transform.CompareTag("Item/Small"))
            {
                if (Item.transform.CompareTag("Item/Placable/Small") && _hit.transform.childCount < 4)
                {
                    item = InstantiatePlacable();

                }
                if (Item.transform.CompareTag("Item/Placable/Medium") && _hit.transform.childCount < 3)
                {
                    item = InstantiatePlacable();
                    Instantiate(_empty, _hit.transform).GetComponent<PlacableEmpties>().PlacableItem = item;

                }
            }
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
        GameObject item = Instantiate(Item, _child.position, Item.transform.localRotation, _hit.transform);
        item.transform.localScale = new Vector3(1, 1, 1);
        item.GetComponent<Collider>().enabled = true;
        item.GetComponent<ItemStats>().IsPlaced = true;
        Destroy(Item);
        _itemRotation = 0;
        SetPointerBackToOrigin();
        return item;
    }


    public void SetPointerBackToOrigin()
    {
        transform.position = _originPoint;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<ItemPlacement>().enabled = false;
        GetComponent<PlayerPointer>().CanMove = false;
    }
}
