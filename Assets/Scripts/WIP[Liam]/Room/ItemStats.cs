////using UnityEngine;
////using UnityEngine.UI;

////public class ItemStats : MonoBehaviour
////{
////    public int HP, Cost, Points;
////    public bool IsGlass, IsWood, IsPlaced;
////    public float PlacementTime, Timer;
////    public GameObject PlayerPhone;
////    private bool phoneHasNotBeenActivated = true;


////    // Update is called once per frame
////    void Update()
////    {
////        if (HP <= 0) Destroy(gameObject);

////        if (IsPlaced && Timer < PlacementTime && phoneHasNotBeenActivated)
////        {
////            Timer += Time.deltaTime;
////            PlayerPhone.SetActive(true);

////            for (int childIndex = 0; childIndex< PlayerPhone.transform.childCount - 1; childIndex++)
////            {
////                if(PlayerPhone.transform.GetChild(childIndex).gameObject.activeSelf)
////                    PlayerPhone.transform.GetChild(childIndex).gameObject.SetActive(false);
////            }

////            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).gameObject.SetActive(true);
////            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).GetComponent<Slider>().value = PlacementTime-Timer;
////            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).GetComponent<Slider>().maxValue = PlacementTime;
////        }

////        if (Timer >= PlacementTime && phoneHasNotBeenActivated)
////        {
////            for (int childIndex = 0; childIndex < PlayerPhone.transform.childCount - 1; childIndex++)
////            {
////                if (!PlayerPhone.transform.GetChild(childIndex).gameObject.activeSelf)
////                    PlayerPhone.transform.GetChild(childIndex).gameObject.SetActive(true);
////            }

////            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).gameObject.SetActive(false);
////            PlayerPhone.transform.parent.GetComponent<ButtonSelect>().CancelAction();
////            phoneHasNotBeenActivated = false;
////        }
////    }
////}
//using UnityEngine;
//using UnityEngine.UI;

//public class ItemStats : MonoBehaviour
//{
//    public int HP, Cost, Points;
//    public bool IsGlass, IsWood, IsPlaced;
//    public float PlacementTime, Timer;
//    public GameObject PlayerPhone;
//    private bool phoneHasNotBeenActivated = true;

//    private bool pointsAdded = false;  // To prevent adding points multiple times

//    void Update()
//    {
//        if (HP <= 0) Destroy(gameObject);

//        if (IsPlaced && !pointsAdded)
//        {
//            ScoreManager.Instance.AddPoints(Points);
//            pointsAdded = true;
//        }

//        if (IsPlaced && Timer < PlacementTime && phoneHasNotBeenActivated)
//        {
//            Timer += Time.deltaTime;
//            PlayerPhone.SetActive(true);

//            for (int childIndex = 0; childIndex < PlayerPhone.transform.childCount - 1; childIndex++)
//            {
//                if (PlayerPhone.transform.GetChild(childIndex).gameObject.activeSelf)
//                    PlayerPhone.transform.GetChild(childIndex).gameObject.SetActive(false);
//            }

//            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).gameObject.SetActive(true);
//            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).GetComponent<Slider>().value = Timer;
//            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).GetComponent<Slider>().maxValue = PlacementTime;
//        }

//        if (Timer >= PlacementTime && phoneHasNotBeenActivated)
//        {
//            for (int childIndex = 0; childIndex < PlayerPhone.transform.childCount - 1; childIndex++)
//            {
//                if (!PlayerPhone.transform.GetChild(childIndex).gameObject.activeSelf)
//                    PlayerPhone.transform.GetChild(childIndex).gameObject.SetActive(true);
//            }

//            PlayerPhone.transform.GetChild(PlayerPhone.transform.childCount - 1).gameObject.SetActive(false);
//            PlayerPhone.transform.parent.GetComponent<ButtonSelect>().CancelAction();
//            phoneHasNotBeenActivated = false;
//        }
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class ItemStats : MonoBehaviour
{
    public int HP, Cost, Points;
    public bool IsGlass, IsWood, IsPlaced;
    public float PlacementTime, Timer;
    public GameObject PlayerPhone;
    private bool phoneHasNotBeenActivated = true;

    private bool pointsAdded = false;

    public int playerIndex;  // NEW: Which player owns this item (0-3)

    void Update()
    {
        if (HP <= 0) Destroy(gameObject);

        if (IsPlaced && !pointsAdded)
        {
            ScoreManager.Instance.AddPoints(playerIndex, Points);
            pointsAdded = true;
        }

        // Your existing Update code for timer and phone UI here ...
    }
}
