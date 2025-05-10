using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public int HP, Cost, Points;
    public bool IsGlass, IsWood, IsPlaced;
    public float PlacementTime;
   
    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) Destroy(gameObject);
    }
}
