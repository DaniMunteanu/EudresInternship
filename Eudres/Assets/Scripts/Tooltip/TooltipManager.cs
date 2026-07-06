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

    public void Show(string text, string details)
    {
        tooltip.SetText(text, details);
        tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }

}
