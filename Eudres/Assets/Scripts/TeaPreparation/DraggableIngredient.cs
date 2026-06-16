using UnityEngine;

public class DraggableIngredient : MonoBehaviour
{
    [SerializeField] private Ingredient ingredientData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (spriteRenderer != null && ingredientData != null)
        {
            spriteRenderer.sprite = ingredientData.ingredientSprite;
        }
    }

    
    void Update()
    {
        
    }
}
