using DG.Tweening;
using TMPro;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private TMP_Text bubbleText;
    [SerializeField] private RectTransform rectTransform;
    public Keyword keyword;

    private Vector3 initialPosition;

    void OnEnable()
    {
        initialPosition = rectTransform.anchoredPosition;
        if (keyword)
        {
            bubbleText.text = keyword.name;
        }
        Float();
    }

    public void Highlight()
    {
        bubbleText.color = Color.green;
    }

    public void ResetHighlight()
    {
        bubbleText.color = Color.black;
    }

    public void Float()
    {
        float xDuration = Random.Range(2.0f, 4.0f);
        float yDuration = Random.Range(2.0f, 4.0f);

        rectTransform.DOAnchorPosX(initialPosition.x + 5f, xDuration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        rectTransform.DOAnchorPosY(initialPosition.y + 10f, yDuration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }
    
}
