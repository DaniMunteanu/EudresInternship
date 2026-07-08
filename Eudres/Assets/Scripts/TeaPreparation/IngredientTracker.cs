using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IngredientTracker : MonoBehaviour
{
    [SerializeField] private KeywordGroup requiredKeywords;
    [SerializeField] private Recipe currentRecipe;
    [SerializeField] private GameObject ingredientTextsParent;

    private TMP_Text[] ingredientTexts;
    private int currentIngredientIndex = 0;
    private int maxIngredients = 6;
    
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
        KeywordsTracker.Instance.ShowBubbles();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (IsIngredientValid(ingredient))
        {
            ingredientTexts[currentIngredientIndex].text = ingredient.ingredientName;
            currentIngredientIndex++;

            currentRecipe.ingredients.Add(ingredient);

            AudioManager.Instance.PlaySFX(AudioManager.Instance.ingredientDrop);

            for (int i = 0; i < ingredient.keywords.Length; i++)
            {
                if (requiredKeywords.keywords.Contains(ingredient.keywords[i]))
                {
                    KeywordsTracker.Instance.PopBubble(ingredient.keywords[i]);
                    return;
                }
            }
        }
    }

    public bool IsIngredientValid(Ingredient ingredient)
    {
        if (currentRecipe.ingredients.Contains(ingredient))
        {
            IndicationText.Instance.DisplayText("I already added this ingredient");
            return false;
        }

        if (currentIngredientIndex == maxIngredients)
        {
            IndicationText.Instance.DisplayText("Can't add more ingredients");
            return false;
        }

        int count = 0;

        for (int index = 0; index < currentRecipe.ingredients.Count; index++)
        {
            if (currentRecipe.ingredients[index].ingredientType == ingredient.ingredientType)
            {
                count++;
                if (count == ingredient.ingredientType.maxIngredients)
                {
                    IndicationText.Instance.DisplayText("Can't add more of this ingredient type");
                    return false;
                }
            }
        }
        
        return true;
    }
}
