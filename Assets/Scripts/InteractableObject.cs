using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] string _promptMessage = "Press E to interact";
        public UnityEvent OnInteract = new UnityEvent();

        public string GetInteractPrompt()
        {
            return _promptMessage;
        }

        public void Interact(GameObject interactor)
        {
            OnInteract.Invoke();
        }
    }
}
