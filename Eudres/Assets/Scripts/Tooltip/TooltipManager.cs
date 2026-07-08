using System;
using UnityEditor;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private Tooltip tooltip;

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
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
        for (int index = 0; index < tooltip.keywordTextFields.Length; index++)
        {
            tooltip.keywordTextFields[index].text = "";
        }
    }

}
