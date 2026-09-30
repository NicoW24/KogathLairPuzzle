using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] string _promptMessage = "Press E to interact";
        [SerializeField] Color _interactColor = Color.green;
        public UnityEvent OnInteract = new UnityEvent();

        Renderer _objectRenderer;

        void Awake()
        {
            _objectRenderer = GetComponent<Renderer>();
        }

        public string GetInteractPrompt()
        {
            return _promptMessage;
        }

        public void Interact(GameObject interactor)
        {
            if (_objectRenderer != null)
            {
                _objectRenderer.material.color = _interactColor;
            }
            OnInteract.Invoke();
            Debug.Log($"{interactor.name} interacted with {gameObject.name}");
        }
    }
}
