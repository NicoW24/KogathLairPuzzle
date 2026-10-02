using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Core
{
    public class CharacterHPBar : MonoBehaviour
    {
        [SerializeField] Slider _hpSlider;
        [SerializeField] float _animationSpeed = 3f;
        [SerializeField] TextMeshProUGUI _valueText;

        float _maxHealth;
        float _currentHealth;
        Coroutine _animCoroutine;

        public void SetupHPBar(float maxHP)
        {
            _maxHealth = maxHP;
            _currentHealth = _maxHealth;

            if (_hpSlider != null)
            {
                _hpSlider.minValue = 0f;
                _hpSlider.maxValue = _maxHealth;
                _hpSlider.value = _currentHealth;
            }
            UpdateHPValueText();
        }

        public void UpdateHpBar(float currentHealth)
        {
            _currentHealth = currentHealth;

            if (_animCoroutine != null)
            {
                StopCoroutine(_animCoroutine);
            }
            _animCoroutine = StartCoroutine(AnimateHealthDecrease());
        }

        void UpdateHPValueText()
        {
            _valueText.text = $"{_hpSlider.value}/{_maxHealth}";
        }

        IEnumerator AnimateHealthDecrease()
        {
            while (_hpSlider.value > _currentHealth)
            {
                _hpSlider.value = Mathf.MoveTowards(
                    _hpSlider.value,
                    _currentHealth,
                    _animationSpeed * _maxHealth * Time.deltaTime
                );
                UpdateHPValueText();
                yield return null;
            }

            _hpSlider.value = _currentHealth;
        }
    }
}

