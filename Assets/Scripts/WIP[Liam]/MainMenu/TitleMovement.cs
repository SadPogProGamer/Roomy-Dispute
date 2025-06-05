using UnityEngine;

public class TitleMovement : MonoBehaviour
{
    private float _rotationLerpIndex = .5f;
    private float _scaleLerpIndex = .5f;

    private Vector3 _originalScale;

    [SerializeField]
    private bool _IsSubtractingFromRotationLerp, _IsSubtractingFromScaleLerp;

    private void Start()
    {
        _originalScale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (_rotationLerpIndex > 1)
            _IsSubtractingFromRotationLerp = true;
        
        if(_rotationLerpIndex < 0) 
            _IsSubtractingFromRotationLerp = false;

        if (_IsSubtractingFromRotationLerp)
        {
            _rotationLerpIndex -= Time.deltaTime * 1.5f;
        }
        else
        {
            _rotationLerpIndex += Time.deltaTime * 1.5f;
        }

        
        
        if (_scaleLerpIndex > 1)
            _IsSubtractingFromScaleLerp = true;

        if (_scaleLerpIndex < 0)
            _IsSubtractingFromScaleLerp = false;

        if (_IsSubtractingFromScaleLerp)
        {
            _scaleLerpIndex -= Time.deltaTime/1.5f;
        }
        else
        {
            _scaleLerpIndex += Time.deltaTime/1.5f;
        }

        transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 15), Quaternion.Euler(0, 0, -15), _rotationLerpIndex);
        transform.localScale = Vector3.Lerp(_originalScale, _originalScale * 2, _scaleLerpIndex);
    }
}
