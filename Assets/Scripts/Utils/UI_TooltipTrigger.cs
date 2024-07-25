using System.Collections;
using Language;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public class UI_TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string HeaderKey;
        public string ContentKey;
        public float ShowDelay = 0.5f;
        
        private LanguageManager _languageManager;
        private string _header;
        [Multiline] private string _content;
        private Coroutine _showTooltipCoroutine;

        private void Start()
        {
            _languageManager = LanguageManager.Instance;
            _content = _languageManager.GetValue(ContentKey);
            _header = _languageManager.GetValue(HeaderKey);
            UI_TooltipSystem.Hide();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _showTooltipCoroutine = StartCoroutine(ShowTooltipAfterDelay());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_showTooltipCoroutine != null)
            {
                StopCoroutine(_showTooltipCoroutine);
            }
            UI_TooltipSystem.Hide();
        }

        private IEnumerator ShowTooltipAfterDelay()
        {
            yield return new WaitForSeconds(ShowDelay);
            UI_TooltipSystem.Show(_content, _header);
        }
    }
}