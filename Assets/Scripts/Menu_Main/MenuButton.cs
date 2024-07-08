using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using Utils;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI ButtonText = null;
    public Color TextDefaultColor;
    public Color TextHighlightColor = new ();
    public Vector3 DefaultScale;
    public Vector3 HighlightScale;

    private bool _isMouseOver = false;

    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _isMouseOver ? HighlightScale : DefaultScale, ConstantsUtil.BUTTON_SCALE_LERP_SPEED);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
        ButtonText.color = TextHighlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
        ButtonText.color = TextDefaultColor;
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
}
