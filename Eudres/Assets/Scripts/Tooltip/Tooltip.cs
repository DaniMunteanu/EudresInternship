using System;
using TMPro;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tooltip : MonoBehaviour
{
    public TMP_Text titleTextField;
    public TMP_Text detailsTextField;
    public TMP_Text[] keywordTextFields;

    public bool keepPosition = false;

    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private InputSystemUIInputModule inputModule;

    public void SetText(string titleContent, string detailsContent)
    {
        titleTextField.text = titleContent;
        detailsTextField.text = detailsContent;
        RefreshSize();
    }

    public void RefreshSize()
    {
        layoutElement.enabled = detailsTextField.preferredWidth >= layoutElement.preferredWidth;
    }

    public void Update()
    {
        if (keepPosition)
        {
            return;
        }

        Vector2 newPosition = inputModule.point.action.ReadValue<Vector2>();

        float pivotX = newPosition.x / Screen.width;
        float pivotY = newPosition.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = newPosition;
    }

    public void HighlightKeyword(Keyword keyword)
    {
        foreach (var keywordTextField in keywordTextFields)
        {
            if (keywordTextField.text == keyword.name)
            {
                keywordTextField.color = Color.green;
                return;
            }
        }
    }

    public void ResetHighlight()
    {
        foreach (var keywordTextField in keywordTextFields)
        {
            keywordTextField.color = Color.white;
        }
    }

}
