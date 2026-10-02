using Game.Core;
using System.Collections.Generic;
using UnityEngine;

public class DamageTriggerObject : MonoBehaviour
{
    [SerializeField] CharacterStat _characterStat;
    [SerializeField] string _targetTag = "Player";
    bool canDealDamage = false;

    private void OnTriggerStay(Collider other)
    {
        if (!canDealDamage) return;

        if (_targetTag == other.tag)
        {
            CharacterStat enemy = other.GetComponent<CharacterStat>();

            if (enemy != null)
            {
                if(enemy != _characterStat)
                    enemy.TakeDamage(_characterStat.GetAttack());


                DisableDamage();
            }
        }
    }
    /// <summary>
    /// Used in animation to deal damage
    /// </summary>
    public void EnableDamage()
    {
        canDealDamage = true;
    }
    /// <summary>
    /// Used in animation to stop damage deal
    /// </summary>
    public void DisableDamage()
    {
        canDealDamage = false;
    }
    /// <summary>
    /// Function for animation after dead animation
    /// </summary>
    public void DyingFunction()
    {
        _characterStat.Dead();
    }
}