using UnityEngine;
using UnityEngine.UI;

public class Cup : MonoBehaviour, IDropArea
{
    [SerializeField] IngredientTracker ingredientTracker;
    [SerializeField] private Image fillingImage;
    [SerializeField] private Image frontFillingImage;
    [SerializeField] private TeaParticles teaParticles;
    
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
        else if (ingredient.ingredientType.typeName != "Base")
        {
            // Handle non-base ingredients
            teaParticles.PlayParticles(Color.white); // COLOR SHOULD BE CHANGED TO THE INGREDIENT'S COLOR
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
