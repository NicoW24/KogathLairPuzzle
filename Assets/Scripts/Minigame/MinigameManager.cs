using UnityEngine;
using Game.Core;

namespace Game.Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        public virtual void StartMinigame()
        {
            GameManager.Instance.MinigameStarted();
        }

        public virtual void MinigameFinish() 
        {
            GameManager.Instance.MinigameFinished();
        }

        public virtual void SetupMinigame() { }
    }
}

