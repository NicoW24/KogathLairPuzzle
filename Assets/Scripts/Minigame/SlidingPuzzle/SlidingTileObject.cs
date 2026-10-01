using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlidingTileObject : MonoBehaviour
{
    [SerializeField] TMP_Text _numberText;
    Button _button;

    public int CorrectIndex;
    public int CurrentIndex;

    Coroutine _animateCoroutine;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Setup(int number, int startIndex, Action<SlidingTileObject> onClickAction)
    {
        CorrectIndex = number - 1;
        CurrentIndex = startIndex;
        _numberText.text = number.ToString();

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onClickAction(this));
    }

    public void UpdateIndex(int newIndex)
    {
        CurrentIndex = newIndex;
    }

    public bool IsInCorrectPosition()
    {
        return CurrentIndex == CorrectIndex;
    }

    public void SlideToPosition(Vector3 targetPosition, float duration)
    {
        if (_animateCoroutine != null) StopCoroutine(_animateCoroutine);
        _animateCoroutine = StartCoroutine(AnimateSlide(targetPosition, duration));
    }

    IEnumerator AnimateSlide(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }
}