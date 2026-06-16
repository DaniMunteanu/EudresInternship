using UnityEngine;

public class Cup : MonoBehaviour, IDropArea
{
    [SerializeField] IngredientTracker ingredientTracker;

    [SerializeField] private Collider2D cupCollider;
    [SerializeField] private SpriteRenderer cupSpriteRenderer;
    [SerializeField] private Sprite emptyCupSprite;
    [SerializeField] private Sprite filledCupSprite;

    void Start()
    {
        if (cupSpriteRenderer != null)
            cupSpriteRenderer.sprite = emptyCupSprite;
    }

    public void OnIngredientDropped(Ingredient ingredient)
    {
        if (cupSpriteRenderer != null)
            cupSpriteRenderer.sprite = filledCupSprite;

        Debug.Log("Ingredient dropped: " + ingredient.ingredientName);

        ingredientTracker.AddIngredient(ingredient);
    }

    public void ClearCup()
    {
        cupSpriteRenderer.sprite = emptyCupSprite;
    }
}
