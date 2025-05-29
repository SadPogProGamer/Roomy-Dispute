using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Progress;

public class SabotageTool : MonoBehaviour
{
    public enum SabotageMode { Fire, Target, Break, Bomb }

    [SerializeField]
    private GameObject _playerPhone;
    private GameObject _player1Pointer;
    private ButtonSelect _buttonselect;

    [Header("Settings")]
    public SabotageMode Mode = SabotageMode.Fire;

    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private Color highlightColor = Color.green;
    [SerializeField] private LayerMask selectableLayers;
    [SerializeField] private float bombRadius = 3f;
    [SerializeField] private GameObject bombPreview;
    private Vector3 _bombTargetPoint;

    public Transform aimOrigin; 

    private GameObject _currentTarget;
    private Material[] _originalMaterials;

    public event Action OnComplete;
    public event Action OnCancel;

    private void Update()
    {
        if (aimOrigin == null) return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
        if (Mode == SabotageMode.Bomb)
        {
            if (Physics.Raycast(ray, out RaycastHit bombHit, rayDistance, selectableLayers))
            {
                _bombTargetPoint = bombHit.point;

                if (bombPreview != null)
                {
                    bombPreview.SetActive(true);
                    bombPreview.transform.position = _bombTargetPoint + Vector3.up * 0.01f;
                    bombPreview.transform.localScale = new Vector3(bombRadius * 2, 0.1f, bombRadius * 2);
                }
            }

            return;
        }
        
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
        if (Mode == SabotageMode.Bomb)
        {
            Collider[] hits = Physics.OverlapSphere(_bombTargetPoint, bombRadius, selectableLayers);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out ItemStats stats))
                    Destroy(hit.gameObject);
            }

            if (bombPreview != null)
                bombPreview.SetActive(false);

            OnComplete?.Invoke();
            return;
        }
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
            if (bombPreview != null)
                bombPreview.SetActive(false);
            ClearCurrentTarget();
            OnCancel?.Invoke();
        }
    }
}
