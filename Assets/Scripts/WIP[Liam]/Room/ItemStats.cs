using UnityEngine;
using UnityEngine.UI;

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
            PlayerPhone.SetActive(true);
            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).GetComponent<Slider>().value = Timer;
            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).GetComponent<Slider>().maxValue = PlacementTime;
        }

        if (Timer >= PlacementTime && phoneHasNotBeenActivated)
        {
            PlayerPhone.transform.parent.GetComponent<ButtonSelect>().CancelAction();
            phoneHasNotBeenActivated = false;
        }
    }
}
