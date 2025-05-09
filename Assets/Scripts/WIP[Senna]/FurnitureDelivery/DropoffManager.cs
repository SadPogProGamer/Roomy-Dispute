using UnityEngine;

public class DropoffManager : MonoBehaviour
{
    [SerializeField] public DropoffZone[] _dropoffZones;

    [HideInInspector] public int _randomIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetRandomDropoffZone();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetRandomDropoffZone()
    {
        if (_dropoffZones.Length > 0)
        {
            int randomIndex = Random.Range(0, _dropoffZones.Length);

            _dropoffZones[randomIndex].gameObject.SetActive(true);
            Debug.Log("Randomly selected PickupZone index: " + randomIndex);
            Debug.Log("PickupZone: " + _dropoffZones[randomIndex].GetLocationName());
            _randomIndex = randomIndex;
        }
        else
        {
            Debug.LogWarning("No PickupZones available.");
        }
    }
}