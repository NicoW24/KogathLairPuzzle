using UnityEngine;

namespace Game.Core
{
    public interface IInteractable
    {
        /// <summary>
        /// Get string prompt
        /// </summary>
        string GetInteractPrompt();

        /// <summary>
        /// Interact function
        /// </summary>
        void Interact(GameObject interactor);
    }
}
