using UnityEngine;
using UnityEngine.UI;

public class DestinationUI : MonoBehaviour
{
    [SerializeField] private Text _destinationText;
    
    [SerializeField] private TaxiManager _taxiManager;

    [SerializeField] private DropoffManager _dropoffManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _destinationText.text = "Pick up passenger add its location";
    }

    // Update is called once per frame
    void Update()
    {
     
        if (_taxiManager._hasPassenger)
        {
            _destinationText.text = "Pick up passenger add its location";
        }
        else
        {
            _destinationText.text = "Destination: " + _dropoffManager._dropoffZones[_dropoffManager._randomIndex].GetLocationName();
        }
    }
}