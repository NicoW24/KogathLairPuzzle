using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        public UnityAction OnPausePlayerController;
        public UnityAction OnResumePlayerController;

        public static GameManager Instance;

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        public void MinigameStarted()
        {
            //pause player movement
            OnPausePlayerController.Invoke();
            UIManager.Instance.TogglePause();
        }

        public void MinigameFinished()
        {
            //resume player movement
            OnResumePlayerController.Invoke();
            UIManager.Instance.TogglePause();
        }
    }
}

