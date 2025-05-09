using UnityEngine;

public class TaxiManager : MonoBehaviour
{

    [HideInInspector] public bool _hasPassenger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _hasPassenger = !_hasPassenger;
        }
    }
}
