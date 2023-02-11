using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button Button;
    public TextMeshProUGUI ButtonText;

    public Color TextDefaultColor = new (185, 141, 255);
    public Color TextHighlightColor = new ();

    public Vector3 DefaultScale = new(1f, 1f, 1f);
    public Vector3 HighlightScale = new(2.0f, 2.0f, 2.0f);

    private bool IsMouseOver = false;
    private float ScaleLerpSpeed = 0.025f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOver = true;
        ButtonText.color = TextHighlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOver = false;
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

    public void Update()
    {
        if (IsMouseOver)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, HighlightScale, ScaleLerpSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, DefaultScale, ScaleLerpSpeed);
        }
    }
}
