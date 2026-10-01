using UnityEngine;
using Game.Core;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game.Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        [SerializeField] protected Button _buttonCancel;
        public UnityEvent OnMinigameWin;

        public virtual void StartMinigame()
        {
            GameManager.Instance.MinigameStarted();
        }

        public virtual void MinigameFinish() 
        {
            GameManager.Instance.MinigameFinished();
        }

        public virtual void MinigameWin() 
        {
            MinigameFinish();
        }

        public virtual void SetupMinigame() { }

        public virtual void CancelMinigame() 
        {
            MinigameFinish();
        }
    }
}

