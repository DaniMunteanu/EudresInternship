using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableIngredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public Ingredient ingredientData;
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Collider2D ingredientCollider;
    [SerializeField] public TooltipTrigger tooltipTrigger;

    private Vector2 initialPosition;
    private Quaternion initialRotation;
    public bool isDraggable = true;

    void Start()
    {
        initialPosition = rectTransform.anchoredPosition;
        initialRotation = rectTransform.rotation;

        if (image != null && ingredientData != null)
        {
            image.sprite = ingredientData.ingredientSprite;
            tooltipTrigger.text = ingredientData.ingredientName;

            for (int i = 0; i < ingredientData.keywords.Length; i++)
            {
                tooltipTrigger.keywords[i] = ingredientData.keywords[i].name;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        RectTransform parentRect = rectTransform.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        RectTransform parentRect = rectTransform.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }

        //Check if the ingredient is over the cup
        ingredientCollider.enabled = false;
        Collider2D hitCollider = Physics2D.OverlapPoint(rectTransform.position);
        ingredientCollider.enabled = true;

        if (hitCollider != null && hitCollider.TryGetComponent<IDropArea>(out IDropArea dropArea))
        {
            rectTransform.DORotate(new Vector3(0, 0, 45f), 0.2f)
                .SetEase(Ease.InOutSine)
                .SetLink(gameObject)
                .SetUpdate(true);
        }
        else
        {
            rectTransform.DORotate(Vector3.zero, 0.2f)
                .SetEase(Ease.InOutSine)
                .SetLink(gameObject)
                .SetUpdate(true);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        //Check if the ingredient is over the cup
        ingredientCollider.enabled = false;
        Collider2D hitCollider = Physics2D.OverlapPoint(rectTransform.position);
        ingredientCollider.enabled = true;

        if (hitCollider != null && hitCollider.TryGetComponent<IDropArea>(out IDropArea dropArea))
        {
            dropArea.OnIngredientDropped(ingredientData);
        }

        rectTransform.DOKill();

        rectTransform.anchoredPosition = initialPosition;
        rectTransform.rotation = initialRotation;
    }
}
