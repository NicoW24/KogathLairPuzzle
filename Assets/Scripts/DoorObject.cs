using UnityEngine;

namespace Game.Core
{
    public class DoorObject : MonoBehaviour
    {
        Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
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
