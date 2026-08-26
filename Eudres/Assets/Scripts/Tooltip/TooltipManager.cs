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
    [SerializeField] private Camera mainCamera;

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

    public void Show(string text, string details, string[] keywords, Vector3 position = default)
    {
        tooltip.keepPosition = false;
        tooltip.SetText(text, details);
        tooltip.gameObject.SetActive(true);

        //tooltip.transform.position = mainCamera.WorldToScreenPoint(position) + new Vector3(0, -50, 0);
        tooltip.GetComponent<RectTransform>().position = position + new Vector3(0, -20, 0);

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
            ShowTutorialStep();
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
