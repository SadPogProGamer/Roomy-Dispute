using UnityEngine;

public class MoneyApp : MonoBehaviour
{
    private int _buttonIndex, _previousButtonIndex;
    // Update is called once per frame

    private void Start()
    {
        _buttonIndex = Random.Range(0, 8);
    }
    void Update()
    {
        if (_buttonIndex != _previousButtonIndex)
        {
            for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
            {
                if(childIndex == _buttonIndex)
                {
                    transform.GetChild(childIndex).gameObject.SetActive(true);
                }
                else { transform.GetChild(childIndex).gameObject.SetActive(false); }
            }
        }
    }


    public void OnUp()
    {
        if (_buttonIndex == 0)
        {

        }
        else 
    }
    public void OnDown()
    {

    }
    public void OnLeft()
    {

    }
    public void OnRight()
    {

    }
    public void OnA()
    {

    }
    public void OnX()
    {

    }
    public void OnY()
    {

    }
    public void OnB()
    {

    }
}
