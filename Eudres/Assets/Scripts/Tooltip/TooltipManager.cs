using System;
using UnityEditor;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] public Tooltip tooltip;

    private bool shouldHighlight = false;
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
        tooltip.SetText(text, details);
        tooltip.gameObject.SetActive(true);
        for (int index = 0; index < keywords.Length; index++)
        {
            tooltip.keywordTextFields[index].text = keywords[index];
        }

        if (shouldHighlight)
        {
            tooltip.HighlightKeyword(keywordToHighlight);
        }
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
        for (int index = 0; index < tooltip.keywordTextFields.Length; index++)
        {
            tooltip.keywordTextFields[index].text = "";
        }
        shouldHighlight = false;
    }

    public void HighlightKeyword(Keyword keyword)
    {
        shouldHighlight = true;
        keywordToHighlight = keyword;
    }

}
