using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IngredientTracker : MonoBehaviour
{
    [SerializeField] private Recipe currentRecipe;
    [SerializeField] private GameObject ingredientTextsParent;

    private TMP_Text[] ingredientTexts;
    private int currentIngredientIndex = 0;
    private int maxIngredients = 0;
    
    void Start()
    {
        ingredientTexts = ingredientTextsParent.GetComponentsInChildren<TMP_Text>();
        maxIngredients = ingredientTexts.Length;
        Clear();
    }

    public void Clear()
    {
        foreach (var text in ingredientTexts)
        {
            text.text = "";
        }
        currentIngredientIndex = 0;

        currentRecipe.ingredients.Clear();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (currentIngredientIndex < maxIngredients)
        {
            ingredientTexts[currentIngredientIndex].text = ingredient.ingredientName;
            currentIngredientIndex++;

            currentRecipe.ingredients.Add(ingredient);
        }
        else
        {
            print("Maximum number of ingredients reached!");
        }
    }
}
