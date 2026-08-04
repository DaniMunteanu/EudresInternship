using UnityEngine;

public class Cup : MonoBehaviour, IDropArea
{
    [SerializeField] IngredientTracker ingredientTracker;
    [SerializeField] private SpriteRenderer fillingSpriteRenderer;

    void Start()
    {
        if (fillingSpriteRenderer != null)
            fillingSpriteRenderer.sprite = null;
    }

    public void OnIngredientDropped(Ingredient ingredient)
    {
        if (ingredient.ingredientType.typeName == "Base" && fillingSpriteRenderer.sprite == null)
        {
            fillingSpriteRenderer.sprite = ingredient.cupChangeSprite;
        }
        
        ingredientTracker.AddIngredient(ingredient);
    }

    public void ClearCup()
    {
        fillingSpriteRenderer.sprite = null;
    }
}
