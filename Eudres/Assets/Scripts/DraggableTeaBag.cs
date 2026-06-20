using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTeaBag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private BreathingMinigame minigame;
    private Canvas canvas;

    void Awake()
    {
        minigame = GetComponentInParent<BreathingMinigame>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        minigame?.OnTeaBagBeginDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        minigame?.OnTeaBagDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        minigame?.OnTeaBagEndDrag();
    }
}
