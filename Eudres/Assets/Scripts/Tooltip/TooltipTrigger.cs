using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    public string details = "Especially helpful for those suffering from:";
    public string[] keywords = new string[3];
    private bool touchscreenEnabled = false;
    private bool isDragging = false;

    private void Start()
    {
        if (Touchscreen.current != null)
        {
            touchscreenEnabled = true;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (isDragging)
            return;
        TooltipManager.Instance.Show(text, details, keywords, transform.position);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
        eventData.selectedObject = null;
        isDragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (touchscreenEnabled || isDragging)
            return;
        
        TooltipManager.Instance.Show(text, details, keywords, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (touchscreenEnabled)
            return;

        TooltipManager.Instance.Hide();
    }
}
