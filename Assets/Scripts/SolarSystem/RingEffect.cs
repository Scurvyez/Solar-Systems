using UnityEngine;
using UnityEngine.EventSystems;

public class RingEffect : MonoBehaviour
{
    public GameObject ring;
    private readonly float newX = 90.0f;

    public void Start()
    {
        ring.transform.localRotation = Quaternion.Euler(newX, ring.transform.localRotation.y, ring.transform.localRotation.z);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ring.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ring.SetActive(false);
    }
}
