using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UnityEngine.EventSystems;

public class ExpandingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button Button;
    public Vector3 DefaultScale = new(1f, 1f, 1f);
    public Vector3 HighlightScale = new(2.0f, 2.0f, 2.0f);

    private bool _isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
    }

    private IEnumerator ScaleButton(Transform button, Vector3 startScale, Vector3 endScale, float duration)
    {
        float startTime = Time.deltaTime;
        while (Time.deltaTime < startTime + duration)
        {
            button.localScale = Vector3.Lerp(startScale, endScale, (Time.time - startTime) / duration);
            yield return null;
        }
        button.localScale = endScale;
    }

    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _isMouseOver ? HighlightScale : DefaultScale, ConstantsUtil.BUTTON_SCALE_LERP_SPEED);
    }
}
