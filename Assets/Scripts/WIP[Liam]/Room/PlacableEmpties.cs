using UnityEngine;

public class PlacableEmpties : MonoBehaviour
{
    public GameObject PlacableItem;
    void Update()
    {
        if (PlacableItem == null) Destroy(gameObject);
    }
}
