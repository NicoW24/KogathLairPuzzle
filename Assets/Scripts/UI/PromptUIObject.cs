using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class PromptUIObject : MonoBehaviour
    {
        public static PromptUIObject Instance;
        [SerializeField] TextMeshProUGUI _promptTextUIObject;
        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            HideUI();
        }

        public void ShowUI(string promptText)
        {
            _promptTextUIObject.text = promptText;
            _promptTextUIObject.gameObject.SetActive(true);
        }

        public void HideUI()
        {
            _promptTextUIObject.gameObject.SetActive(false);
        }
    }
}

