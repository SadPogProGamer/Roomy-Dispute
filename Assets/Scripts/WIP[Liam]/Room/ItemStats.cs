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

        if (IsPlaced && Timer < PlacementTime && phoneHasNotBeenActivated)
        {
            Timer += Time.deltaTime;
            PlayerPhone.SetActive(true);

            for (int childIndex = 0; childIndex< PlayerPhone.transform.childCount - 1; childIndex++)
            {
                if(PlayerPhone.transform.GetChild(childIndex).gameObject.activeSelf)
                    PlayerPhone.transform.GetChild(childIndex).gameObject.SetActive(false);
            }

            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).gameObject.SetActive(true);
            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).GetComponent<Slider>().value = PlacementTime-Timer;
            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).GetComponent<Slider>().maxValue = PlacementTime;
        }

        if (Timer >= PlacementTime && phoneHasNotBeenActivated)
        {
            for (int childIndex = 0; childIndex < PlayerPhone.transform.childCount - 1; childIndex++)
            {
                if (!PlayerPhone.transform.GetChild(childIndex).gameObject.activeSelf)
                    PlayerPhone.transform.GetChild(childIndex).gameObject.SetActive(true);
            }

            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).gameObject.SetActive(false);
            PlayerPhone.transform.parent.GetComponent<ButtonSelect>().CancelAction();
            phoneHasNotBeenActivated = false;
        }
    }
}
