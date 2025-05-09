using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [SerializeField] private PickupZone[] _pickupZones;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetRandomPickupZone();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetRandomPickupZone()
    {
        if (_pickupZones.Length > 0)
        {
            int randomIndex = Random.Range(0, _pickupZones.Length);

            _pickupZones[randomIndex].gameObject.SetActive(true);
            Debug.Log("Randomly selected PickupZone index: " + randomIndex);
            Debug.Log("PickupZone: " + _pickupZones[randomIndex].GetPickupName());
            
        }
        else
        {
            Debug.LogWarning("No PickupZones available.");
        }
    }
}