using UnityEngine;

public class DropoffZone : MonoBehaviour
{
    [Tooltip("Name of the destination this passenger wants to go to.")]
    public string destinationName = "Default Destination";

    public string GetLocationName()
    {
        return destinationName;
    }
}