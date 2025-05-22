using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class SabotageTool : MonoBehaviour
{
    [SerializeField] private LayerMask selectableLayers;
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private Color highlightColor = Color.green;
    [SerializeField] public Transform aimOrigin; // Where the ray starts from (e.g., camera or player head)

    private GameObject _currentTarget;
    private Material[] _originalMaterials;
    public event Action OnComplete;


    private void Update()
    {
        if (aimOrigin == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        // ✅ Now supports multiple selectable layers
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, selectableLayers))
        {
            GameObject candidate = hit.collider.gameObject;
           
            Debug.Log($"[Sabotage] Hit: {candidate.name}");

            if (candidate.TryGetComponent(out ItemStats stats))
            {
                Debug.Log($"[Sabotage] Found ItemStats: HP={stats.HP}, IsWood={stats.IsWood}");
                if (stats.IsWood)
                {
                    if (_currentTarget != candidate)
                        SelectNewTarget(candidate);
                }
                else
                {
                    ClearCurrentTarget();
                }
            }
            else
            {
                Debug.Log("[Sabotage] No ItemStats component on target");
                ClearCurrentTarget();
            }
        }
        else
        {
            ClearCurrentTarget();
        }
    }





    private void SelectNewTarget(GameObject newTarget)
    {
        ClearCurrentTarget();

        _currentTarget = newTarget;

        // Highlight by changing color (simple visual snap effect)
        Renderer renderer = _currentTarget.GetComponent<Renderer>();
        if (renderer != null)
        {
            _originalMaterials = renderer.materials;
            Material highlightMat = new Material(renderer.material);
            highlightMat.color = highlightColor;
            renderer.material = highlightMat;
        }
    }

    private void ClearCurrentTarget()
    {
        if (_currentTarget != null)
        {
            Renderer renderer = _currentTarget.GetComponent<Renderer>();
            if (renderer != null && _originalMaterials != null)
                renderer.materials = _originalMaterials;

            _currentTarget = null;
            _originalMaterials = null;
        }
    }

    // Call this from the PlayerInput system
    public void OnComfirm()
    {
        if (_currentTarget != null)
        {
            Destroy(_currentTarget);
            _currentTarget = null;
            OnComplete?.Invoke();
        }
    }


}
