using UnityEngine;

namespace Utils
{
    public class UI_TooltipSystem : MonoBehaviour
    {
        public UI_Tooltip Tooltip;
        
        private static UI_TooltipSystem _instance;

        public void Awake()
        {
            _instance = this;
        }

        public static void Show(string content, string header = "")
        {
            _instance.Tooltip.SetText(content, header);
            _instance.Tooltip.gameObject.SetActive(true);
        }
        
        public static void Hide()
        {
            _instance.Tooltip.gameObject.SetActive(false);
        }
    }
}