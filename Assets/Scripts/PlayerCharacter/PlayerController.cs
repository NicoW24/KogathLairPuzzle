using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Game.Core
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] float _moveSpeed = 5f;
        [SerializeField] float _rotationSpeed = 15f;
        [SerializeField] float _gravity = -9.81f;

        [Header("Animation Settings")]
        [SerializeField] Animator _animator;

        [SerializeField] InputActionReference _attackActionRef;

        CharacterController _controller;
        PlayerInput _playerInput;
        Transform _mainCameraTransform;
        InputAction _moveAction;
        CharacterStat _characterStat;

        Vector2 _moveInput;
        Vector3 _verticalVelocity;

        bool _canMove = true;

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            _characterStat = GetComponent<CharacterStat>();
            _moveAction = _playerInput.actions["Move"];

            if (Camera.main != null)
            {
                _mainCameraTransform = Camera.main.transform;
            }
        }

        void OnAttackPerformed(InputAction.CallbackContext context)
        {
            if (!_canMove)
            {
                return;
            }
            _animator.SetTrigger("Attack");
        }

        bool IsAttacking()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(1);
            return stateInfo.IsName("Attack") && stateInfo.normalizedTime < 1.0f;
        }

        public void Die()
        {
            _animator.SetTrigger("Die");
        }

        void Start()
        {
            GameManager.Instance.OnPausePlayerController += PausePlayerController;
            GameManager.Instance.OnResumePlayerController += ResumePlayerController;

            if (_attackActionRef != null && _attackActionRef.action != null)
            {
                _attackActionRef.action.Enable();
                _attackActionRef.action.performed += OnAttackPerformed;
            }
        }

        void OnDestroy()
        {
            GameManager.Instance.OnPausePlayerController -= PausePlayerController;
            GameManager.Instance.OnResumePlayerController -= ResumePlayerController;

            if (_attackActionRef != null && _attackActionRef.action != null)
            {
                _attackActionRef.action.performed -= OnAttackPerformed;
                _attackActionRef.action.Disable();
            }
        }

        void Update()
        {
            if (!_canMove || IsAttacking()|| _characterStat.IsDead())
            {
                return;
            }
            ReadInput();
            MoveAndRotate();
            ApplyGravity();
            UpdateAnimator();
        }

        #region Movement Function
        /// <summary>
        /// Read input from player control
        /// </summary>
        void ReadInput()
        {
            _moveInput = _moveAction.ReadValue<Vector2>();
        }

        /// <summary>
        /// Moves and rotates the player
        /// </summary>
        void MoveAndRotate()
        {
            if (_mainCameraTransform == null) return;

            Vector3 camForward = _mainCameraTransform.forward;
            Vector3 camRight = _mainCameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * _moveInput.y) + (camRight * _moveInput.x);

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );
                _controller.Move(moveDirection.normalized * (_moveSpeed * Time.deltaTime));
            }
        }
        /// <summary>
        /// Apply gravity to player character
        /// </summary>
        void ApplyGravity()
        {
            if (_controller.isGrounded && _verticalVelocity.y < 0)
            {
                _verticalVelocity.y = -2f;
            }

            _verticalVelocity.y += _gravity * Time.deltaTime;
            _controller.Move(_verticalVelocity * Time.deltaTime);
        }
        /// <summary>
        /// Play animation for player character
        /// </summary>
        void UpdateAnimator(float value = 0)
        {
            if (_animator == null) return;

            if(value == 0)
            {
                value = _moveInput.magnitude;
            }
            _animator.SetFloat("Speed", value);
        }
        #endregion

        /// <summary>
        /// Pause player character controller
        /// </summary>
        void PausePlayerController()
        {
            _canMove = false;
            //set animation to idle
            UpdateAnimator(0);
        }
        /// <summary>
        /// Resume player character controller
        /// </summary>
        void ResumePlayerController()
        {
            _canMove = true;
        }
    }
}