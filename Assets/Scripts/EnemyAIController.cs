using UnityEngine;
using UnityEngine.AI;

namespace Game.Core
{
    public class EnemyAIController : MonoBehaviour
    {
        [Header("Target & Detection")]
        [SerializeField] Transform _player;
        [SerializeField] float _chaseRange = 10f;
        [SerializeField] float _attackRange = 2f;

        [Header("Attack Settings")]
        [SerializeField] float _attackCooldown = 1.5f;
        [SerializeField] CharacterStat _characterStat;

        [SerializeField] Animator _animator;
        [SerializeField] Vector3 _firstPos;
        NavMeshAgent _agent;
        float _lastAttackTime;

        void Awake()
        {
            _firstPos = transform.position;

            _agent = GetComponent<NavMeshAgent>();
            _characterStat = GetComponent<CharacterStat>();

            if (_player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    _player = playerObj.transform;
                }
            }
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
            _animator.Rebind();
            _animator.Play("Idle", 0, 0f);

            CharacterController controller = GetComponent<CharacterController>();
            controller.enabled = false;
            //reset pos
            this.transform.position = _firstPos;
            controller.enabled = true;
            //reset variable
            _lastAttackTime = Time.time;
        }

        void Update()
        {
            if (_player == null || _characterStat.IsDead()) return;

            if (IsAttacking())
            {
                _agent.isStopped = true;
                _animator.SetBool("IsWalking", false);
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (distanceToPlayer <= _attackRange)
            {
                _animator.SetBool("IsWalking", false);
                _agent.isStopped = true;
                RotateTowardsPlayer();

                if (Time.time >= _lastAttackTime + _attackCooldown)
                {
                    AttackPlayer();
                    _lastAttackTime = Time.time;
                }
            }
            else if (distanceToPlayer <= _chaseRange)
            {
                _animator.SetBool("IsWalking", true);
                _agent.isStopped = false;
                _agent.SetDestination(_player.position);
            }
            else
            {
                _animator.SetBool("IsWalking", false);
                _agent.isStopped = true;
            }
        }

        bool IsAttacking()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Attack") && stateInfo.normalizedTime < 1.0f;
        }

        void RotateTowardsPlayer()
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        void AttackPlayer()
        {
            _animator.SetTrigger("Attack");
        }

        public void Die()
        {
            _animator.SetTrigger("Die");
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _chaseRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}

