using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameTrigger : MonoBehaviour
{
    [Header("Check if the scene is a tutorial")]
    [SerializeField] bool isTutorialScene = false;
    [SerializeField] GameObject tutorialStepBreathingMinigame;
    [SerializeField] string shopCounterScene = "ShopCounter";
    [SerializeField] string tutorialShopCounterScene = "TutorialShopCounter";

    [SerializeField] private IngredientTracker ingredientTracker;
    [SerializeField] private BreathingMinigame minigame;
    [SerializeField] private Button serveButton;
    [SerializeField] private Button resetButton;
    private bool minigameStarted = false;

    void Start()
    {
        minigame.cupController.OnMinigameComplete += OnMinigameWon;
        minigame.gameObject.SetActive(false);  
    }

    public void CheckTea()
    {
        if (KeywordsTracker.Instance.AreAllBubblesPopped() && !minigameStarted)
        {
            StartMinigame();
        }
        else
        {
            IndicationText.Instance.DisplayText("I think I can do something better than this!");
        }
    }

    public void StartMinigame()
    {
        if (ingredientTracker)
        {
            ingredientTracker.HighlightIngredient(null);
        }

        if (isTutorialScene)
        {
            tutorialStepBreathingMinigame.SetActive(true);
        }

        serveButton.interactable = false;
        resetButton.interactable = false;

        minigame.gameObject.SetActive(true); 
        minigameStarted = true;
    }

    void OnMinigameWon()
    {
        minigame.gameObject.SetActive(false);
        minigameStarted = false;
        TeaSession.IsReturningFromPreparation = true;
        if (isTutorialScene)
            SceneManager.LoadScene(tutorialShopCounterScene);
        else
            SceneManager.LoadScene(shopCounterScene);
    }
}
