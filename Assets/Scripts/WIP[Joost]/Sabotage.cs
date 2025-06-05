using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Progress;

public class SabotageTool : MonoBehaviour
{
    public enum SabotageMode { Fire, Target, Break, Bomb }

    [Header("Audio")]
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip targetSound;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip bombSound;
    private AudioSource _audioSource;


    [SerializeField]
    private GameObject _playerPhone;
    private GameObject _player1Pointer;
    private ButtonSelect _buttonselect;

    [Header("Settings")]
    public SabotageMode Mode = SabotageMode.Fire;

    [SerializeField] private float rayDistance = Mathf.Infinity;
    [SerializeField] private Color highlightColor = Color.green;
    [SerializeField] private LayerMask selectableLayers;
    [SerializeField] private float bombRadius = 3f;
    [SerializeField] private GameObject bombPreview;
    private Vector3 _bombTargetPoint;

    public Transform aimOrigin;

    public bool SabotageHasBeenActivated;

    private GameObject _currentTarget;
    private Material[] _originalMaterials;

    public event Action OnComplete;
    public event Action OnCancel;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


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
    private void PlaySabotageSound()
    {
        if (_audioSource == null) return;

        AudioClip clipToPlay = Mode switch
        {
            SabotageMode.Fire => fireSound,
            SabotageMode.Target => targetSound,
            SabotageMode.Break => breakSound,
            SabotageMode.Bomb => bombSound,
            _ => null
        };

        if (clipToPlay != null)
            _audioSource.PlayOneShot(clipToPlay);
    }

    public void OnComfirm()
    {
        if (SabotageHasBeenActivated)
        {
            if (Mode == SabotageMode.Bomb)
            {
                Collider[] hits = Physics.OverlapSphere(_bombTargetPoint, bombRadius, selectableLayers);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent(out ItemStats stats))
                    {
                        stats.RemovePoints();
                        stats.ActivatePhone();
                        Destroy(hit.gameObject);
                    }
                }

                if (bombPreview != null)
                    bombPreview.SetActive(false);
                PlaySabotageSound();
                OnComplete?.Invoke();
                return;
            }
            if (_currentTarget != null)
            {
                _currentTarget.GetComponent<ItemStats>().RemovePoints();
                _currentTarget.GetComponent<ItemStats>().ActivatePhone();
                Destroy(_currentTarget);
                _currentTarget = null;
                PlaySabotageSound();
                OnComplete?.Invoke();

            }
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