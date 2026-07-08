using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableIngredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Ingredient ingredientData;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D ingredientCollider;
    [SerializeField] private TooltipTrigger tooltipTrigger;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        if (spriteRenderer != null && ingredientData != null)
        {
            spriteRenderer.sprite = ingredientData.ingredientSprite;
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
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        transform.position = worldPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ingredientCollider.enabled = false;
        Collider2D hitCollider = Physics2D.OverlapPoint(transform.position);
        ingredientCollider.enabled = true;

        if (hitCollider != null && hitCollider.TryGetComponent<IDropArea>(out IDropArea dropArea))
        {
            dropArea.OnIngredientDropped(ingredientData);
        }

        transform.position = initialPosition;
    }
}
