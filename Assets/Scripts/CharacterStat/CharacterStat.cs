using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class CharacterStat : MonoBehaviour
    {
        [SerializeField] CharacterStatSO _characterStatDataSO;
        [SerializeField] float _currentHP;
        [SerializeField] float _currentDef;
        [SerializeField] float _currentAttack;

        [SerializeField] CharacterHPBar _characterHPBar;
        public UnityEvent OnDead = new UnityEvent();
        public UnityEvent AfterDeadEvent = new UnityEvent();

        [SerializeField] bool _isDead = false;

        void Start()
        {
            SetupInitStat();
            GameManager.Instance.OnPlayAgain += PlayAgain;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnPlayAgain -= PlayAgain;
        }

        void PlayAgain()
        {
            //reset stat and variable
            SetupInitStat();
            _isDead = false;
            gameObject.SetActive(true);
        }

        void SetupInitStat()
        {
            //init stat
            _currentHP = _characterStatDataSO.HP;
            _currentDef = _characterStatDataSO.Def;
            _currentAttack = _characterStatDataSO.Attack;

            //set max hp to healthbar object
            _characterHPBar.SetupHPBar(_currentHP);
        }

        public void ChangeAttack(float attack)
        {
            _currentAttack = attack;
        }

        public float GetAttack()
        {
            return _currentAttack;
        }

        public void TakeDamage(float damage)
        {
            float damageVal = damage - _currentDef;
            if (damageVal <= 0)
            {
                damageVal = 1;
            }
            _currentHP -= damageVal;
            if(_currentHP <= 0)
            {
                _isDead = true;
                OnDead?.Invoke();
            }
            _characterHPBar.UpdateHpBar(_currentHP);
        }

        public void Dead()
        {
            gameObject.SetActive(false);
            AfterDeadEvent?.Invoke();
        }

        public bool IsDead()
        {
            return _isDead;
        }
    }
}

