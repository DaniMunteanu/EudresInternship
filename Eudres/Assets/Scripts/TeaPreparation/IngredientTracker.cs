using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IngredientTracker : MonoBehaviour
{
    [Header("In case the scene is a tutorial")]
    [SerializeField] GameObject tutorialStepKeywordDescription;
    [SerializeField] GameObject tutorialStepKeywordFound;
    [SerializeField] GameObject tutorialStepIngredientDrag;
    [SerializeField] GameObject tutorialStepIngredientAdded;
    [SerializeField] GameObject tutorialStepBreathingMinigame;

    [SerializeField] private KeywordGroup requiredKeywords;
    [SerializeField] private Recipe currentRecipe;
    [SerializeField] private GameObject ingredientTextsParent;
    [SerializeField] private GameObject draggableIngredientsParent;
    [SerializeField] private Ingredient tutorialIngredient;

    private TMP_Text[] ingredientTexts;
    private int currentIngredientIndex = 0;
    private int maxIngredients = 6;
    private bool tutorialIngredientAlreadyAdded = false;
    
    void Start()
    {
        if (tutorialStepKeywordDescription)
        {
            ClearTutorialSteps();
            tutorialStepKeywordDescription.SetActive(true);
            tutorialStepIngredientDrag.SetActive(true);

            HighlightIngredient(tutorialIngredient);
            TooltipManager.Instance.HighlightKeyword(requiredKeywords.keywords[0]);
        }

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
                    break;
                }
            }

            if (ingredient == tutorialIngredient && tutorialStepKeywordDescription && tutorialIngredientAlreadyAdded == false)
            {
                ResetIngredientHighlights();
                TooltipManager.Instance.tooltip.ResetHighlight();
                TooltipManager.Instance.shouldHighlight = false;

                tutorialStepIngredientDrag.SetActive(false);
                tutorialStepIngredientAdded.SetActive(true);

                tutorialIngredientAlreadyAdded = true;
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

    void HighlightIngredient(Ingredient ingredient)
    {
        foreach (Transform subparent in draggableIngredientsParent.transform)
        {
            foreach (Transform child in subparent)
            {
                DraggableIngredient draggableIngredient = child.GetComponent<DraggableIngredient>();
                if (draggableIngredient.ingredientData != ingredient)
                {
                    draggableIngredient.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                    draggableIngredient.tooltipTrigger.enabled = false;
                }
            }
        }
    }

    void ResetIngredientHighlights()
    {
        foreach (Transform subparent in draggableIngredientsParent.transform)
        {
            foreach (Transform child in subparent)
            {
                DraggableIngredient draggableIngredient = child.GetComponent<DraggableIngredient>();
                draggableIngredient.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                draggableIngredient.tooltipTrigger.enabled = true;
            }
        }
    }

    public void ClearTutorialSteps()
    {
        TooltipManager.Instance.tutorialStepShowing = false;
        TooltipManager.Instance.Hide();

        if (tutorialStepKeywordDescription)
            tutorialStepKeywordDescription.SetActive(false);
        if (tutorialStepKeywordFound)
            tutorialStepKeywordFound.SetActive(false);
        if (tutorialStepIngredientAdded)
            tutorialStepIngredientAdded.SetActive(false);
        if (tutorialStepBreathingMinigame)
            tutorialStepBreathingMinigame.SetActive(false);
    }
}
