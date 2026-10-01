using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class PromptUIObject : MonoBehaviour
    {
        public static PromptUIObject Instance;
        [SerializeField] TextMeshProUGUI promptTextUIObject;
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
            promptTextUIObject.text = promptText;
            promptTextUIObject.gameObject.SetActive(true);
        }

        public void HideUI()
        {
            promptTextUIObject.gameObject.SetActive(false);
        }
    }
}

