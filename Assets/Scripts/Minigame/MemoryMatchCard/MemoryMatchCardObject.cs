using UnityEngine;
using UnityEngine.UI;

namespace Game.Minigame
{
    public class MemoryMatchCardObject : MonoBehaviour
    {
        public Image cardImage;
        public int cardId;

        Sprite _frontSprite;
        Sprite _backSprite;
        Button _button;
        Animator _animator;
        CanvasGroup _canvasGroup;
        MemoryMatchCardManager _manager;
        bool _isFlipped = false;

        void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnCardClicked);
            _animator = GetComponent<Animator>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Setup(int id, Sprite front, Sprite back, MemoryMatchCardManager manager)
        {
            cardId = id;
            _frontSprite = front;
            _backSprite = back;
            _manager = manager;
            SetInteractable(true);

            ShowBack();
        }

        void OnCardClicked()
        {
            if (_isFlipped || !_manager.CanSelectCard()) return;

            ShowFront();
            _manager.OnCardSelected(this);
        }

        public void ShowFront()
        {
            _isFlipped = true;
            PlayFlipAnimation();
        }

        public void ShowBack()
        {
            _isFlipped = false;
            PlayFlipAnimation();
        }

        public void ChangeSprite()
        {
            if (_isFlipped)
            {
                cardImage.sprite = _frontSprite;
            }
            else
            {
                cardImage.sprite = _backSprite;
            }
        }

        void PlayFlipAnimation()
        {
            _animator.SetBool("Flip", _isFlipped);
        }

        public void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;

            if (!interactable)
                _canvasGroup.alpha = 0.6f;
            else
                _canvasGroup.alpha = 1f;
        }
    }
}

