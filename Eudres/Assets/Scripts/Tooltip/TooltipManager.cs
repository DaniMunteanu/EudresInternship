using System;
using UnityEditor;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [Header("In case the scene is a tutorial")]
    [SerializeField] GameObject tutorialStepKeywordFound;
    public bool tutorialStepShown = false;
    public bool tutorialStepShowing = false;

    [SerializeField] public Tooltip tooltip;

    public bool shouldHighlight = false;
    private Keyword keywordToHighlight;

    public static TooltipManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Hide();
    }

    public void Show(string text, string details, string[] keywords)
    {
        tooltip.keepPosition = false;
        tooltip.SetText(text, details);
        tooltip.gameObject.SetActive(true);
        for (int index = 0; index < keywords.Length; index++)
        {
            tooltip.keywordTextFields[index].text = keywords[index];
        }

        if (shouldHighlight)
        {
            KeywordsTracker.Instance.HighlightBubble(keywordToHighlight);
            tooltip.HighlightKeyword(keywordToHighlight);
        }

        if (tutorialStepShown == false && tutorialStepKeywordFound)
        {
            Invoke(nameof(ShowTutorialStep), 1.0f);
        }
    }

    void ShowTutorialStep()
    {
        tutorialStepKeywordFound.SetActive(true);
        tutorialStepShown = true;
        tutorialStepShowing = true;
    }

    public void Hide()
    {
        if (tutorialStepShowing && tutorialStepKeywordFound)
        {
            tooltip.keepPosition = true;
            return;
        }

        tooltip.gameObject.SetActive(false);
        for (int index = 0; index < tooltip.keywordTextFields.Length; index++)
        {
            tooltip.keywordTextFields[index].text = "";
        }
        
        KeywordsTracker.Instance.ResetBubbleHighlights();
    }

    public void HighlightKeyword(Keyword keyword)
    {
        shouldHighlight = true;
        keywordToHighlight = keyword;
    }

}
