using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    public string text;
    public string details = "Especially helpful for those suffering from:";
    public string[] keywords = new string[3];

    public void OnSelect(BaseEventData eventData)
    {
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
        eventData.selectedObject = null;
    }
}
