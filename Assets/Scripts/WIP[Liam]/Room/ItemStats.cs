using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public int HP, Cost, Points;
    public bool IsGlass, IsWood, IsPlaced;
    public float PlacementTime, Timer;
    public GameObject PlayerPhone;
    
   
    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) Destroy(gameObject);

        if (IsPlaced && Timer < PlacementTime)
        {
            Timer += Time.deltaTime;
        }

        if (Timer >= PlacementTime)
            PlayerPhone.SetActive(true);
    }
}
