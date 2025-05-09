using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{

    [SerializeField] private Image _cashAppImage;
    [SerializeField] private Image _sabotageAppImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IsCashAppPressed()
    {
        _sabotageAppImage.enabled = false;
        _cashAppImage.enabled = false;
        ShowFurnitureImages();
    }

    private void ShowFurnitureImages()
    {
        
    }

    public void IsSabotageAppPressed()
    {
        _sabotageAppImage.enabled = false;
        _cashAppImage.enabled = false;
        ShowSabotageImages();
    }

    private void ShowSabotageImages()
    {
       
    }

    void OnCancel(InputAction action)
    {
        _sabotageAppImage.enabled = true;
        _cashAppImage.enabled = true;
    }
}
