using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isResizable = true;

    private Vector3 originalScale;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isResizable)
            return;

        transform.localScale = originalScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    void Start()
    {
        originalScale = transform.localScale;
    }
}
