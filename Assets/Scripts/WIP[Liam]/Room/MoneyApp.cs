using UnityEngine;

public class MoneyApp : MonoBehaviour
{
    private int _buttonIndex, _previousButtonIndex;
    [SerializeField]
    private GameObject _buttonsParent;
    // Update is called once per frame

    private void Start()
    {
        _buttonIndex = Random.Range(0, 3);
        _previousButtonIndex = -1;//-1 so that it doesnt do the while loop at the start
    }
    void Update()
    {
        while (_buttonIndex == _previousButtonIndex)
        {

        }

        if (_buttonIndex != _previousButtonIndex)
        {
            for (int childIndex = 0; childIndex < _buttonsParent.transform.childCount; childIndex++)
            {
                if(childIndex == _buttonIndex)
                {
                    _buttonsParent.transform.GetChild(childIndex).gameObject.SetActive(true);
                }
                else { _buttonsParent.transform.GetChild(childIndex).gameObject.SetActive(false); }
            }
        }
    }


    public void OnUp()
    {
        if (_buttonIndex == 0)
        {

        }
    }
    public void OnDown()
    {
        if (_buttonIndex == 1)
        {

        }
    }
    public void OnLeft()
    {
        if (_buttonIndex == 2)
        {

        }
    }
    public void OnRight()
    {
        if (_buttonIndex == 3)
        {

        }
    }
    //public void OnA()
    //{

    //}
    //public void OnX()
    //{

    //}
    //public void OnY()
    //{

    //}
    //public void OnB()
    //{

    //}

}
