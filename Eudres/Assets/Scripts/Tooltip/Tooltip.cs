using System;
using TMPro;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tooltip : MonoBehaviour
{
    public TMP_Text textField;
    
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private InputSystemUIInputModule inputModule;

    public void SetText(string content)
    {
        textField.text = content;
        RefreshSize();
    }

    public void RefreshSize()
    {
        layoutElement.enabled = textField.preferredWidth >= layoutElement.preferredWidth;
    }

    public void Update()
    {
        Vector2 newPosition = inputModule.point.action.ReadValue<Vector2>();

        float pivotX = newPosition.x / Screen.width;
        float pivotY = newPosition.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = newPosition;
    }
}
