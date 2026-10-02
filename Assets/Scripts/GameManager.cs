using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        public UnityAction OnPausePlayerController;
        public UnityAction OnResumePlayerController;
        public UnityAction OnPlayAgain;

        public static GameManager Instance;

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void PlayAgain()
        {
            OnPlayAgain?.Invoke();

            //resume player movement
            OnResumePlayerController.Invoke();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void MinigameStarted()
        {
            Cursor.lockState = CursorLockMode.None;

            //pause player movement
            OnPausePlayerController.Invoke();
        }

        public void MinigameFinished()
        {
            Cursor.lockState = CursorLockMode.Locked;

            //resume player movement
            OnResumePlayerController.Invoke();
        }
    }
}

