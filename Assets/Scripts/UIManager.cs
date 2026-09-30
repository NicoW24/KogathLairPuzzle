using UnityEngine;
using Unity.Cinemachine;

namespace Game.Core
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
