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

    private Vector3 initialPosition;
    public bool isDraggable = true;

    void Start()
    {
        initialPosition = rectTransform.anchoredPosition;

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

    public Vector3 GetMousePositionInWorldSpace()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        return position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3 screenPos = eventData.position;
        screenPos.z = 0f;
        rectTransform.position = screenPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3 screenPos = eventData.position;
        screenPos.z = 0f;

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

        rectTransform.position = screenPos;
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

        rectTransform.anchoredPosition = initialPosition;
        rectTransform.DOKill();
        rectTransform.rotation = Quaternion.identity;
    }
}
