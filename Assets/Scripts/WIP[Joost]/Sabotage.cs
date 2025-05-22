using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class SabotageTool : MonoBehaviour
{
    public enum SabotageMode { Fire, Target, Break }

    [SerializeField]
    private GameObject _playerPhone;

    [Header("Settings")]
    public SabotageMode Mode = SabotageMode.Fire;

    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private Color highlightColor = Color.green;
    [SerializeField] private LayerMask selectableLayers;
    public Transform aimOrigin; 

    private GameObject _currentTarget;
    private Material[] _originalMaterials;

    public event Action OnComplete;

    private void Update()
    {
        if (aimOrigin == null) return;

        // Match pointer alignment (same as ItemPlacement)
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        // ✅ Filter raycast by layers
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, selectableLayers))
        {
            GameObject candidate = hit.collider.gameObject;

            if (candidate == this.gameObject)
            {
                Debug.Log("[Sabotage] Hit self — skipping");
                return;
            }

            Debug.Log($"[Sabotage] Hit: {candidate.name}");

            if (candidate.TryGetComponent(out ItemStats stats))
            {
                Debug.Log($"[Sabotage] Found ItemStats: HP={stats.HP}, IsWood={stats.IsWood}");

                bool isValidTarget = Mode switch
                {
                    SabotageMode.Fire => stats.IsWood,
                    SabotageMode.Target => true,
                    SabotageMode.Break => stats.IsGlass,
                    _ => false
                };


                if (isValidTarget)
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

    public void OnComfirm()
    {
        if (_currentTarget != null)
        {
            Destroy(_currentTarget);
            _currentTarget = null;
            OnComplete?.Invoke();
        }
    }

    private void SelectNewTarget(GameObject newTarget)
    {
        ClearCurrentTarget();

        _currentTarget = newTarget;

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

    public void OnReturn()
    {
        if (GetComponent<SabotageTool>().enabled)
        {
            _playerPhone.SetActive(true);
            _playerPhone.transform.parent.GetComponent<ButtonSelect>().CancelAction();
            _playerPhone.transform.parent.GetComponent<ButtonSelect>()._sabotageCount++;
            GetComponent<ItemPlacement>().SetPointerBackToOrigin();
            GetComponent<SabotageTool>().enabled = false;
        }
    }
}
