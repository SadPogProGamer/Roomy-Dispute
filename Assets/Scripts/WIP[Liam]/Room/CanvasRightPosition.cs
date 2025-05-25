using UnityEngine;

public class CanvasRightposition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,transform.parent.parent.localRotation.y,0);
    }
}
