using Game.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] string _messageWin;
        [SerializeField] string _messageLose;

        [SerializeField] TextMeshProUGUI _messageText;
        [SerializeField] Button _buttonPlayAgain;
        [SerializeField] Button _buttonExit;

        void Awake()
        {
            _buttonPlayAgain.onClick.AddListener(PlayAgain);
            _buttonExit.onClick.AddListener(ExitGame);
        }

        void OnDestroy()
        {
            _buttonPlayAgain.onClick.RemoveListener(PlayAgain);
            _buttonExit.onClick.RemoveListener(ExitGame);
        }

        void PlayAgain()
        {
            GameManager.Instance.PlayAgain();
            gameObject.SetActive(false);
        }

        void ExitGame()
        {
            GameManager.Instance.ExitGame();
            gameObject.SetActive(false);
        }

        public void ShowWinPanel()
        {
            _messageText.text = _messageWin;
            GameManager.Instance.OnPausePlayerController();
            gameObject.SetActive(true);
        }

        public void ShowLosePanel()
        {
            _messageText.text = _messageLose;
            GameManager.Instance.OnPausePlayerController();
            gameObject.SetActive(true);
        }
    }
}

