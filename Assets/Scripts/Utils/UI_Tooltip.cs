using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class UI_Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI HeaderField;
        public TextMeshProUGUI ContentField;
        public LayoutElement LayoutElement;
        public int CharacterWrapLimit;
        public Vector2 PositionOffset;
        public RectTransform RectTransform;
        
        private void Start()
        {
            RectTransform = GetComponent<RectTransform>();
        }
        
        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                HeaderField.gameObject.SetActive(false);
            }
            else
            {
                HeaderField.gameObject.SetActive(true);
                HeaderField.text = header;
            }

            ContentField.text = content;
            int headerLength = HeaderField.text.Length;
            int contentLength = ContentField.text.Length;
            LayoutElement.enabled = headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit;
        }
        
        private void Update()
        {
            if (!Application.isEditor)
            {
                int headerLength = HeaderField.text.Length;
                int contentLength = ContentField.text.Length;
                LayoutElement.enabled = headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit;
            }

            Vector2 position = Input.mousePosition;
            Vector2 pivot = CalculatePivot(position);
            RectTransform.pivot = pivot;

            Vector3 offsetPosition = CalculateOffsetPosition(position, pivot);
            transform.position = offsetPosition;
        }
        
        private static Vector2 CalculatePivot(Vector2 position)
        {
            float pivotX = position.x / Screen.width < 0.5f ? 0f : 1f;
            float pivotY = position.y / Screen.height < 0.5f ? 0f : 1f;
            return new Vector2(pivotX, pivotY);
        }
        
        private Vector2 CalculateOffsetPosition(Vector2 position, Vector2 pivot)
        {
            Vector2 offset = new (
                pivot.x == 0 ? PositionOffset.x : -PositionOffset.x,
                pivot.y == 0 ? PositionOffset.y : -PositionOffset.y
            );
            return position + offset;
        }
    }
}
