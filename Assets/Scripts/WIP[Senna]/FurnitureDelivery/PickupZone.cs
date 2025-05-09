using UnityEngine;

public class PickupZone : MonoBehaviour
{
    [Tooltip("Name of the destination this passenger needs to be picked up.")]
    public string destinationName = "Default Destination";



    public string GetPickupName()
    {
        return destinationName;
    }
}