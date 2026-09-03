using UnityEngine;
using UnityEngine.UI;

public class Cup : MonoBehaviour, IDropArea
{
    [SerializeField] IngredientTracker ingredientTracker;
    [SerializeField] private Image fillingImage;
    [SerializeField] private Image frontFillingImage;

    void Start()
    {
        if (fillingImage != null && frontFillingImage != null)
        {   
            ClearCup();
        }
    }

    public void OnIngredientDropped(Ingredient ingredient)
    {
        if (ingredient.ingredientType.typeName == "Base" && fillingImage.sprite == null)
        {
            fillingImage.sprite = ingredient.cupChangeSprite;
            fillingImage.color = Color.white; // Make the image visible
            fillingImage.SetNativeSize(); // Adjust the size of the image to match the sprite

            frontFillingImage.sprite = ingredient.cupChangeSprite;
            frontFillingImage.color = Color.white; // Make the image visible
            frontFillingImage.SetNativeSize(); // Adjust the size of the image to match the sprite
        }
        
        ingredientTracker.AddIngredient(ingredient);
    }

    public void ClearCup()
    {
        fillingImage.sprite = null;
        fillingImage.color = new Color(1f, 1f, 1f, 0f); // Make the image transparent

        frontFillingImage.sprite = null;
        frontFillingImage.color = new Color(1f, 1f, 1f, 0f); // Make the image transparent
    }
}
