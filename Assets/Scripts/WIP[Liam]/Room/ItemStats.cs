using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public int HP, Cost, Points;
    public bool IsGlass, IsWood, IsPlaced;
    public float PlacementTime, Timer;
    public GameObject PlayerPhone;
    private bool phoneHasNotBeenActivated = true;
    
   
    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) Destroy(gameObject);

        if (IsPlaced && Timer < PlacementTime)
        {
            Timer += Time.deltaTime;
        }

        if (Timer >= PlacementTime && phoneHasNotBeenActivated)
        {
            PlayerPhone.SetActive(true);
            phoneHasNotBeenActivated = false;
        }
    }
}
