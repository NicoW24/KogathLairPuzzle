using UnityEngine;

namespace Game.Core
{
    public class DoorObject : MonoBehaviour
    {
        Animator _animator;
        Vector3 _firstPosition;

        void Awake()
        {
            _firstPosition = transform.position;
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            GameManager.Instance.OnPlayAgain += PlayAgain;
        }

        void OnDestroy()
        {
            GameManager.Instance.OnPlayAgain -= PlayAgain;
        }

        void PlayAgain()
        {
            this.transform.position = _firstPosition;
        }

        public void OpenDoor()
        {
            _animator.SetBool("OpenDoor", true);
        }
        public void CloseDoor()
        {
            _animator.SetBool("OpenDoor", false);
        }
    }
}
