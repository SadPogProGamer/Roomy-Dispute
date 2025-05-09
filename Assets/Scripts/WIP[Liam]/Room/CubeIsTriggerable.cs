using UnityEngine;

public class CubeIsTriggerable : MonoBehaviour
{
    public bool IsTriggerable;


    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Item"))
            IsTriggerable = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Item"))
            IsTriggerable = false;
    }
}
