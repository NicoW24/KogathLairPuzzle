using UnityEngine;
using Unity.Cinemachine;
using Game.Core;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] CinemachineInputAxisController _cinemachineInput;
        bool _isPaused = false;
        public static UIManager Instance;

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {
            GameManager.Instance.OnPausePlayerController += TogglePause;
            GameManager.Instance.OnResumePlayerController += TogglePause;
        }

        void OnDestroy()
        {
            GameManager.Instance.OnPausePlayerController -= TogglePause;
            GameManager.Instance.OnResumePlayerController -= TogglePause;
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _cinemachineInput.enabled = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _cinemachineInput.enabled = true;
            }
        }
    }
}
