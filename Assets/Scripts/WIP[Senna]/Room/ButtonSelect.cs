using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [Header("PlayerPointers")]
    [SerializeField] private GameObject _player1Pointer;

    [Header("PlayerPhones")]
    [SerializeField] private GameObject _player1Phone;


    [Header("UI Stuff")]
    public GameObject MoneyManager;
    //public GameObject ScoreManager;

    [Header("App Icons")]
    [SerializeField] private GameObject _shoppingAppIcon;
    [SerializeField] private GameObject _sabotageIcon;
    [SerializeField] private GameObject _cashAppIcon;

    [Header("Money App")]
    [SerializeField] private GameObject _moneyApp;


    [Header("Furniture Apps")]
    [SerializeField] private GameObject[] _cheapFurniture;
    [SerializeField] private GameObject[] _mediumFurniture;
    [SerializeField] private GameObject[] _expensiveFurniture;

    [Header("Furniture App Text")]
    [SerializeField] private GameObject _furnitureText;

    [Header("Sabotage Apps")]
    [SerializeField] private GameObject _fireApp;
    [SerializeField] private GameObject _bombApp;
    [SerializeField] private GameObject _targetApp;
    [SerializeField] private GameObject _breakApp;

    [Header("Sabotage Apps Text")]
    [SerializeField] private GameObject _fireAppText;
    [SerializeField] private GameObject _bombAppText;
    [SerializeField] private GameObject _targetAppText;
    [SerializeField] private GameObject _breakAppText;

    [SerializeField] private TextMeshProUGUI _sabotageCountText;

    [HideInInspector] public int _sabotageCount = 3;

    [Header("Event Timer")]
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private SabotageTool _SabotageTool;

    private GameObject _lastSelectedButton;
    private bool _canMove, _didLoop, _previousInteractibilityOfCheapFurniture, _currentInteractibilityOfCheapFurniture;

    private float _inputCooldown = 0.2f;
    private float _lastInputTime = 0f;


    private int _cheapFurnitureIndex, _previousCheapFurnitureIndex;
    private int _mediumFurnitureIndex, _previousMediumFurnitureIndex;
    private int _expensiveFurnitureIndex, _previousExpensiveFurnitureIndex;
    


    private void Start()
    {
        DisableSabotageApps();
        DisableFurnitureApps();

        GiveRandomNumberForCheapFurniture();
        GiveRandomNumberForExpensiveFurniture();
        GiveRandomNumberForMediumFurniture();

        _sabotageCountText.text = _sabotageCount.ToString();

        // Ensure the first selected button is set and valid
        if (_eventSystem != null && _shoppingAppIcon != null)
        {
            _eventSystem.SetSelectedGameObject(_shoppingAppIcon);
        }
    }

    private void Update()
    {
        if (Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex] != null && _player1Phone.activeSelf)
        {
            HandleInput();
            //CheckCurrentSelectedButton();
            CheckCancelButton();
            SubmitCurrent();
            _sabotageCountText.text = _sabotageCount.ToString();
            if(_eventSystem.currentSelectedGameObject == _cheapFurniture[_cheapFurnitureIndex])
            {
                StartCoroutine(CheckBlaBla());
            }
            else
            {
                _previousInteractibilityOfCheapFurniture = false;
                _currentInteractibilityOfCheapFurniture = false;
            }

            if (_shoppingAppIcon.activeSelf && _eventSystem.currentSelectedGameObject == null)
            {
                _eventSystem.SetSelectedGameObject(_shoppingAppIcon);
            }
        }
    }

    private void SetCorrectSabotageText()
    {
        if (_eventSystem.currentSelectedGameObject == _fireApp)
        {
            _fireAppText.SetActive(true);
            _bombAppText.SetActive(false);
            _targetAppText.SetActive(false);
            _breakAppText.SetActive(false);
        }
        else 
        if (_eventSystem.currentSelectedGameObject == _bombApp)
        {
            _fireAppText.SetActive(false);
            _bombAppText.SetActive(true);
            _targetAppText.SetActive(false);
            _breakAppText.SetActive(false);
        }
        else
        if (_eventSystem.currentSelectedGameObject == _targetApp)
        {
            _fireAppText.SetActive(false);
            _bombAppText.SetActive(false);
            _targetAppText.SetActive(true);
            _breakAppText.SetActive(false);
        }
        else 
        if (_eventSystem.currentSelectedGameObject == _breakApp)
        {
            _fireAppText.SetActive(false);
            _bombAppText.SetActive(false);
            _targetAppText.SetActive(false);
            _breakAppText.SetActive(true);
        }
        else
        {
            _fireAppText.SetActive(false);
            _bombAppText.SetActive(false);
            _targetAppText.SetActive(false);
            _breakAppText.SetActive(false);
        }
    }

    //you can change the name i just didnt wanna think :,)
    private IEnumerator CheckBlaBla()
    {
        yield return null;

        _previousInteractibilityOfCheapFurniture = _currentInteractibilityOfCheapFurniture;
        _currentInteractibilityOfCheapFurniture = _cheapFurniture[_cheapFurnitureIndex].GetComponent<Button>().interactable;

        if (_previousInteractibilityOfCheapFurniture && !_currentInteractibilityOfCheapFurniture)
        {
            StartCoroutine(CheckFurnitureSelectabilityPlacement(_mediumFurniture[_mediumFurnitureIndex]));
        }
        else
                if (!_cheapFurniture[_cheapFurnitureIndex].GetComponent<Button>().interactable)
        {
            StartCoroutine(CheckFurnitureSelectabilityPlacement(_mediumFurniture[_mediumFurnitureIndex]));
        }
    }
    private IEnumerator CheckFurnitureSelectabilityPlacement(GameObject furniture)
    {
        yield return null;
        
        if (furniture != null)
        {
            _eventSystem.SetSelectedGameObject(furniture);
        }

        if (_eventSystem.currentSelectedGameObject.activeSelf)
        {
            if (_eventSystem.currentSelectedGameObject == _cheapFurniture[_cheapFurnitureIndex] && !_cheapFurniture[_cheapFurnitureIndex].GetComponent<Button>().interactable)
                _eventSystem.SetSelectedGameObject(_mediumFurniture[_mediumFurnitureIndex]);
            else

            if (_eventSystem.currentSelectedGameObject == _mediumFurniture[_mediumFurnitureIndex] && !_mediumFurniture[_mediumFurnitureIndex].GetComponent<Button>().interactable)
            {
                _eventSystem.SetSelectedGameObject(_expensiveFurniture[_expensiveFurnitureIndex]);
            }
            else

            if (_eventSystem.currentSelectedGameObject == _expensiveFurniture[_expensiveFurnitureIndex] && !_expensiveFurniture[_expensiveFurnitureIndex].GetComponent<Button>().interactable)
            {
                _eventSystem.SetSelectedGameObject(_mediumFurniture[_mediumFurnitureIndex]);
            }


            if (!_expensiveFurniture[_expensiveFurnitureIndex].GetComponent<Button>().interactable && !_mediumFurniture[_mediumFurnitureIndex].GetComponent<Button>().interactable /*&& !_cheapFurniture[_cheapFurnitureIndex].GetComponent<Button>().interactable*/)
            {
                _eventSystem.SetSelectedGameObject(_cheapFurniture[_cheapFurnitureIndex]);
            }
        }
        else
            CancelAction();
    }

    private void CheckCancelButton()
    {
        if (Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex].buttonEast.wasPressedThisFrame &&
            _eventSystem.currentSelectedGameObject != _shoppingAppIcon &&
            _eventSystem.currentSelectedGameObject != _sabotageIcon && 
            _eventSystem.currentSelectedGameObject != _cashAppIcon)
        {
            CancelAction();
        }
    }

    public void SetFurnitureBack()
    {
        _cheapFurnitureIndex = _previousCheapFurnitureIndex;
        _mediumFurnitureIndex = _previousMediumFurnitureIndex;
        _expensiveFurnitureIndex = _previousExpensiveFurnitureIndex;
    }

    public void CancelAction()
    {
        EnableBigApps();
        _eventSystem.SetSelectedGameObject(_lastSelectedButton);
        DisableSabotageApps();
        DisableFurnitureApps();
        DisableCashApp();
        SetCorrectSabotageText();
        _furnitureText.SetActive(false);
    }

    private void CheckCurrentSelectedButton()
    {
        // Check if currentSelectedGameObject is not null before accessing its name
        if (_eventSystem.currentSelectedGameObject != null)
        {
            Debug.Log("Current Selected: " + _eventSystem.currentSelectedGameObject.name);
        }
        else
        {
            Debug.Log("No object selected.");
        }
    }

    private void HandleInput()
    {
        Vector2 dpadInput = Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex].dpad.ReadValue();

        if (Time.time - _lastInputTime < _inputCooldown)
            return; // Skip this frame if still in cooldown

        if (dpadInput.y > 0)
        {
            MoveSelection(Vector2.up);
            _lastInputTime = Time.time;
        }
        else if (dpadInput.y < 0)
        {
            MoveSelection(Vector2.down);
            _lastInputTime = Time.time;
        }
        else if (dpadInput.x > 0)
        {
            MoveSelection(Vector2.right);
            _lastInputTime = Time.time;
        }
        else if (dpadInput.x < 0)
        {
            MoveSelection(Vector2.left);
            _lastInputTime = Time.time;
        }
    }


    private void MoveSelection(Vector2 direction)
    {
        GameObject currentSelected = _eventSystem.currentSelectedGameObject;

        if (currentSelected != null)
        {
            if (direction == Vector2.up)
            {
                if (currentSelected == _sabotageIcon)
                {
                    _eventSystem.SetSelectedGameObject(_shoppingAppIcon);
                }

                else if (currentSelected == _cashAppIcon)
                {
                    _eventSystem.SetSelectedGameObject(_sabotageIcon);
                }

                else if (currentSelected == _targetApp)
                {
                    _eventSystem.SetSelectedGameObject(_fireApp);
                    SetCorrectSabotageText();
                }

                else if (currentSelected == _bombApp)
                {
                    _eventSystem.SetSelectedGameObject(_breakApp);
                    SetCorrectSabotageText();
                }

                if (currentSelected == _mediumFurniture[_mediumFurnitureIndex])
                {
                    StartCoroutine(CheckFurnitureSelectabilityPlacement(_cheapFurniture[_cheapFurnitureIndex]));
                    _canMove = false;
                }

                else if (currentSelected == _expensiveFurniture[_expensiveFurnitureIndex])
                {
                    StartCoroutine(CheckFurnitureSelectabilityPlacement(_mediumFurniture[_mediumFurnitureIndex]));
                    _canMove = false;
                }

            }

            else if (direction == Vector2.down)
            {
                if (currentSelected == _shoppingAppIcon)
                {
                    _eventSystem.SetSelectedGameObject(_sabotageIcon);
                }

                else if (currentSelected == _sabotageIcon)
                {
                    _eventSystem.SetSelectedGameObject(_cashAppIcon);
                }

                else if (currentSelected == _fireApp && _targetApp.GetComponent<Button>().interactable)
                {
                    _eventSystem.SetSelectedGameObject(_targetApp);
                    SetCorrectSabotageText();
                }
                else if (currentSelected == _breakApp && _bombApp.GetComponent<Button>().interactable)
                {
                    _eventSystem.SetSelectedGameObject(_bombApp);
                    SetCorrectSabotageText();
                }


                if (currentSelected == _cheapFurniture[_cheapFurnitureIndex])
                {
                    StartCoroutine(CheckFurnitureSelectabilityPlacement(_mediumFurniture[_mediumFurnitureIndex]));
                    _canMove = false;
                }

                if (currentSelected == _mediumFurniture[_mediumFurnitureIndex])
                {
                    StartCoroutine(CheckFurnitureSelectabilityPlacement(_expensiveFurniture[_expensiveFurnitureIndex]));
                    _canMove = false;
                }

            }

            else if (direction == Vector2.left)
            {
                if (currentSelected == _bombApp)
                {
                    _eventSystem.SetSelectedGameObject(_targetApp);
                    SetCorrectSabotageText();
                }
                if (currentSelected == _breakApp)
                {
                    _eventSystem.SetSelectedGameObject(_fireApp);
                    SetCorrectSabotageText();
                }
            }

            else if (direction == Vector2.right)
            {
                if (currentSelected == _fireApp)
                {
                    _eventSystem.SetSelectedGameObject(_breakApp);
                    SetCorrectSabotageText();
                }
                if (currentSelected == _targetApp && _bombApp.GetComponent<Button>().interactable)
                {
                    _eventSystem.SetSelectedGameObject(_bombApp);
                    SetCorrectSabotageText();
                }
            }
        }
    }

    public void OnShoppingAppButtonClick()
    {
        Debug.Log("Shopping App Button Clicked");
        DisableBigApps();
        EnableFurnitureApps();
        StartCoroutine(CheckFurnitureSelectabilityPlacement(_cheapFurniture[_cheapFurnitureIndex]));
        _lastSelectedButton = _shoppingAppIcon;
        _furnitureText.SetActive(true);
    }

    public void OnSabotageButtonClick()
    {
        if (_sabotageCount <= 0)
        {
            return;
        }
        
        Debug.Log("Sabotage Button Clicked");
        EnableSabotageApps();
        DisableBigApps();
        _eventSystem.SetSelectedGameObject(_fireApp);
        _lastSelectedButton = _sabotageIcon;
        SetCorrectSabotageText();

        if (_sabotageCount < 3)
        {
            _bombApp.GetComponent<Button>().interactable = false;
            if (_sabotageCount < 2)
                _targetApp.GetComponent<Button>().interactable = false;
        }
    }

    public void OnCashAppButtonClick()
    {
        Debug.Log("Cash Button Clicked");
        EnableCashApp();
        DisableBigApps();
        _eventSystem.SetSelectedGameObject(_moneyApp.transform.GetChild(_moneyApp.transform.childCount - 1).gameObject);
        _lastSelectedButton = _cashAppIcon;
    }

    private void EnableCashApp()
    {
        _moneyApp.SetActive(true);
    }
    private void DisableCashApp()
    {
        _moneyApp.SetActive(false);
    }

    public void OnFireAppButtonClick()
    {
        Debug.Log("Fire App Button Clicked");

        var pointer = _player1Pointer.GetComponent<PlayerPointer>();
        var mesh = _player1Pointer.GetComponent<MeshRenderer>();
        var sabotage = _player1Pointer.GetComponent<SabotageTool>();

        pointer.CanMove = true;
        mesh.enabled = true;

        // Assign pointer's transform as the aim origin
        sabotage.aimOrigin = _player1Pointer.transform;
        sabotage.enabled = true;
        sabotage.OnComplete += OnSabotageComplete;
        sabotage.OnCancel += OnSabotageCancel;

        _player1Pointer.GetComponent<ItemPlacement>().enabled = false;

        _player1Phone.SetActive(false);
    }


    public void OnBombAppButtonClick()
    {
        Debug.Log("Bomb App Button Clicked");

        var pointer = _player1Pointer.GetComponent<PlayerPointer>();
        var mesh = _player1Pointer.GetComponent<MeshRenderer>();
        var sabotage = _player1Pointer.GetComponent<SabotageTool>();

        pointer.CanMove = true;
        mesh.enabled = true;

        sabotage.Mode = SabotageTool.SabotageMode.Bomb;
        sabotage.aimOrigin = _player1Pointer.transform;
        sabotage.enabled = true;
        sabotage.OnComplete += OnSabotageComplete;
        sabotage.OnCancel += OnSabotageCancel;

        _player1Pointer.GetComponent<ItemPlacement>().enabled = false;
        _player1Phone.SetActive(false);
    }


    public void OnTargetAppButtonClick()
    {
        Debug.Log("Target App Button Clicked");

        var pointer = _player1Pointer.GetComponent<PlayerPointer>();
        var mesh = _player1Pointer.GetComponent<MeshRenderer>();
        var sabotage = _player1Pointer.GetComponent<SabotageTool>();

        pointer.CanMove = true;
        mesh.enabled = true;

        sabotage.Mode = SabotageTool.SabotageMode.Target;
        sabotage.aimOrigin = _player1Pointer.transform;
        sabotage.enabled = true;
        sabotage.OnComplete += OnSabotageComplete;
        sabotage.OnCancel += OnSabotageCancel;

        _player1Pointer.GetComponent<ItemPlacement>().enabled = false;
        _player1Phone.SetActive(false);
    }


    public void OnBreakAppButtonClick()
    {
        Debug.Log("Break App Button Clicked");

        var pointer = _player1Pointer.GetComponent<PlayerPointer>();
        var mesh = _player1Pointer.GetComponent<MeshRenderer>();
        var sabotage = _player1Pointer.GetComponent<SabotageTool>();

        pointer.CanMove = true;
        mesh.enabled = true;

        sabotage.Mode = SabotageTool.SabotageMode.Break;
        sabotage.aimOrigin = _player1Pointer.transform;
        sabotage.enabled = true;
        sabotage.OnComplete += OnSabotageComplete;
        sabotage.OnCancel += OnSabotageCancel;

        _player1Pointer.GetComponent<ItemPlacement>().enabled = false;
        _player1Phone.SetActive(false);
    }
    private void OnSabotageComplete()
    {
        Debug.Log("oncomplete ran");

        var pointer = _player1Pointer.GetComponent<PlayerPointer>();
        var mesh = _player1Pointer.GetComponent<MeshRenderer>();
        var sabotage = _player1Pointer.GetComponent<SabotageTool>();
        var placement = _player1Pointer.GetComponent<ItemPlacement>();
        int countSubtractionValue;

        pointer.CanMove = false;
        mesh.enabled = false;

        sabotage.enabled = false;

        sabotage.OnComplete -= OnSabotageComplete;
        sabotage.OnCancel -= OnSabotageCancel;

        placement.Item = null;
        placement.enabled = false;

        _player1Phone.SetActive(true);
        CancelAction();

        if (sabotage.Mode == SabotageTool.SabotageMode.Bomb)
            countSubtractionValue = 3;
        else
            if (sabotage.Mode == SabotageTool.SabotageMode.Target)
            countSubtractionValue = 2;
        else
            countSubtractionValue = 1;

        _sabotageCount -= countSubtractionValue;
    }


    private void OnSabotageCancel()
    {
        Debug.Log("oncancel ran");

        var pointer = _player1Pointer.GetComponent<PlayerPointer>();
        var mesh = _player1Pointer.GetComponent<MeshRenderer>();
        var sabotage = _player1Pointer.GetComponent<SabotageTool>();
        var placement = _player1Pointer.GetComponent<ItemPlacement>();

        pointer.CanMove = false;
        mesh.enabled = false;

        sabotage.enabled = false;

        sabotage.OnComplete -= OnSabotageComplete;
        sabotage.OnCancel -= OnSabotageCancel;

        placement.Item = null;
        placement.enabled = false;

        _player1Phone.SetActive(true);
        CancelAction();
    }




    public void OnItemButtonClickPlayer1(GameObject item)
    {
        _previousCheapFurnitureIndex = _cheapFurnitureIndex;
        _previousExpensiveFurnitureIndex = _expensiveFurnitureIndex;
        _previousMediumFurnitureIndex = _mediumFurnitureIndex;

        GiveRandomNumberForCheapFurniture();
        GiveRandomNumberForMediumFurniture();
        GiveRandomNumberForExpensiveFurniture();

        _player1Pointer.GetComponent<MeshRenderer>().enabled = true;
        _player1Pointer.GetComponent<ItemPlacement>().enabled = true;
        _player1Pointer.GetComponent<PlayerPointer>().CanMove = true;

        GameObject spawnedItem = Instantiate(item);
        spawnedItem.GetComponent<ItemStats>().PlayerIndex = _player1Pointer.GetComponent<PlayerPointer>().PlayerIndex;
        spawnedItem.GetComponent<ItemStats>().PlayerPhone = _player1Phone;
        foreach (Material material in spawnedItem.GetComponent<MeshRenderer>().materials)
        {
            material.color *= _player1Pointer.GetComponent<MeshRenderer>().material.color;
        }
        _player1Pointer.GetComponent<ItemPlacement>().Item = spawnedItem;
        
        if (spawnedItem.tag.Contains("Wall"))
        {
            _player1Pointer.transform.position = _player1Pointer.GetComponent<ItemPlacement>().OriginPointWall;
        }
        else
        {
            _player1Pointer.transform.position = _player1Pointer.GetComponent<ItemPlacement>().OriginPointFloor;
        }

        DisableFurnitureApps();
        EnableBigApps();
        _eventSystem.SetSelectedGameObject(_shoppingAppIcon);
        _player1Phone.SetActive(false);
        MoneyManager.GetComponent<MoneyManager>().DecreaseMoney(_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex, Math.Abs(item.GetComponent<ItemStats>().Cost));
        //ScoreManager.GetComponent<ScoreManager>().IncreaseScore(_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex, Math.Abs(item.GetComponent<ItemStats>().Points));
    }

    private void GiveRandomNumberForExpensiveFurniture()
    {
        _expensiveFurnitureIndex = UnityEngine.Random.Range(0, _expensiveFurniture.Length);
    }

    private void GiveRandomNumberForMediumFurniture()
    {
        _mediumFurnitureIndex = UnityEngine.Random.Range(0, _mediumFurniture.Length);
    }

    private void GiveRandomNumberForCheapFurniture()
    {
        _cheapFurnitureIndex = UnityEngine.Random.Range(0, _cheapFurniture.Length);
    }

    private void DisableSabotageApps()
    {
        _fireApp.SetActive(false);
        _bombApp.SetActive(false);
        _targetApp.SetActive(false);
        _breakApp.SetActive(false);
    }

    private void EnableSabotageApps()
    {
        _fireApp.SetActive(true);
        _bombApp.SetActive(true);
        _targetApp.SetActive(true);
        _breakApp.SetActive(true);
    }

    private void DisableFurnitureApps()
    {
        foreach (GameObject furniture in _cheapFurniture)
        {
            furniture.SetActive(false);
        }
        foreach (GameObject furniture in _mediumFurniture)
        {
            furniture.SetActive(false);
        }
        foreach (GameObject furniture in _expensiveFurniture)
        {
            furniture.SetActive(false);
        }
    }

    private void EnableFurnitureApps()
    {
        _cheapFurniture[_cheapFurnitureIndex].SetActive(true);
        _mediumFurniture[_mediumFurnitureIndex].SetActive(true);
        _expensiveFurniture[_expensiveFurnitureIndex].SetActive(true);
    }

    private void DisableBigApps()
    {
        _shoppingAppIcon.SetActive(false);
        _sabotageIcon.SetActive(false);
        _cashAppIcon.SetActive(false);
    }

    private void EnableBigApps()
    {
        _shoppingAppIcon.SetActive(true);
        _sabotageIcon.SetActive(true);
        _cashAppIcon.SetActive(true);
    }

    private void SubmitCurrent()
    {
        if (Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex] != null && Gamepad.all[_player1Pointer.GetComponent<PlayerPointer>().PlayerIndex].buttonSouth.wasPressedThisFrame)
        {
            if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject.GetComponent<Button>().interactable)
                _eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void MakeItSoThatNothingIsSelected()
    {
        _eventSystem.SetSelectedGameObject(null);
    }
}