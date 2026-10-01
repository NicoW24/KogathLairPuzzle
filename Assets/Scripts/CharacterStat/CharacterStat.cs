using UnityEngine;

namespace Game.Core
{
    public class CharacterStat : MonoBehaviour
    {
        [SerializeField] CharacterStatSO _characterStatDataSO;
        [SerializeField] float _currentHP;
        [SerializeField] float _currentDef;
        [SerializeField] float _currentAttack;

        [SerializeField] bool _isDead = false;

        void Start()
        {
            //init stat
            _currentHP = _characterStatDataSO.HP;
            _currentDef = _characterStatDataSO.Def;
            _currentAttack = _characterStatDataSO.Attack;
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
            }
        }

        public bool IsDead()
        {
            return _isDead;
        }
    }
}

