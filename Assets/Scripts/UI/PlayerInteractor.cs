using Game.Core;
using Game.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] Transform _interactionOrigin;
    [SerializeField] float _interactDistance = 2.0f;
    [SerializeField] float _sphereRadius = 0.4f;
    [SerializeField] LayerMask _interactableLayer;

    [SerializeField] InputActionReference _interactActionRef;

    [SerializeField] bool _showGizmos = true;

    IInteractable _currentInteractable;
    RaycastHit _hitInfo;
    bool _hasHit;

    void Awake()
    {
        #if UNITY_EDITOR
        _showGizmos = true;
        #else
        _showGizmos = false;
        #endif
    }

    void OnEnable()
    {
        if (_interactActionRef != null && _interactActionRef.action != null)
        {
            _interactActionRef.action.Enable();
            _interactActionRef.action.performed += OnInteractPerformed;
        }
    }

    void OnDisable()
    {
        if (_interactActionRef != null && _interactActionRef.action != null)
        {
            _interactActionRef.action.performed -= OnInteractPerformed;
            _interactActionRef.action.Disable();
        }
    }
    void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact(gameObject);
            PromptUIObject.Instance.HideUI();
        }
    }

    void Update()
    {
        CheckForInteractable();
    }

    void CheckForInteractable()
    {
        Vector3 origin = _interactionOrigin != null ? _interactionOrigin.position : transform.position + Vector3.up * 1.2f;
        Vector3 direction = transform.forward;

        _hasHit = Physics.SphereCast(origin, _sphereRadius, direction, out _hitInfo, _interactDistance, _interactableLayer);

        if (_hasHit && _hitInfo.collider.TryGetComponent(out IInteractable interactable))
        {
            _currentInteractable = interactable;
            PromptUIObject.Instance.ShowUI(interactable.GetInteractPrompt());
        }
        else
        {
            _currentInteractable = null;
            PromptUIObject.Instance.HideUI();
        }
    }

    void OnDrawGizmos()
    {
        if (!_showGizmos) return;

        Vector3 origin = _interactionOrigin != null ? _interactionOrigin.position : transform.position + Vector3.up * 1.2f;
        Vector3 direction = transform.forward;

        Gizmos.color = (_hasHit && _currentInteractable != null) ? Color.green : Color.red;

        if (Application.isPlaying)
        {
            Vector3 endPoint = origin + direction * (_hasHit ? _hitInfo.distance : _interactDistance);

            Gizmos.DrawWireSphere(origin, _sphereRadius);
            Gizmos.DrawWireSphere(endPoint, _sphereRadius);
            Gizmos.DrawLine(origin, endPoint);

            if (_hasHit)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_hitInfo.point, 0.08f);
            }
        }
        else
        {
            Gizmos.DrawWireSphere(origin, _sphereRadius);
            Gizmos.DrawRay(origin, direction * _interactDistance);
        }
    }
}