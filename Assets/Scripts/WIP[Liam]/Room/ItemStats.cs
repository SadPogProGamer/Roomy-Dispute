using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public int HP, Cost;
    public bool IsGlass, IsWood, IsPlaced;
    public float _placementTime;
   
    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) Destroy(gameObject);
    }
}
