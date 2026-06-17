using System;
using UnityEditor;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] private Tooltip tooltip;

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

    public void Show(string text)
    {
        tooltip.SetText(text);
        tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }

}
