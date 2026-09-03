using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    [SerializeField] private Image bubbleImage;
    [SerializeField] private TMP_Text bubbleText;
    [SerializeField] private RectTransform rectTransform;
    public Keyword keyword;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = rectTransform.anchoredPosition;
    }

    void OnEnable()
    {   
        // We set this only once when the bubble is enabled, so we don't reset the position every time we pop a bubble.
        //initialPosition = rectTransform.anchoredPosition;

        bubbleImage.color = new Color(255, 255, 255, 0);
        bubbleText.color = new Color(0, 0, 0, 0);

        if (keyword)
        {
            bubbleText.text = keyword.name;
        }

        bubbleImage.DOFade(1, 1).SetLink(gameObject);
        bubbleText.DOFade(1, 1).SetLink(gameObject);
    }

    void OnDisable()
    {
        rectTransform.DOKill();
        bubbleImage.DOKill();
        bubbleText.DOKill();
    }

    public void Highlight()
    {
        bubbleText.color = Color.darkGreen;
    }

    public void ResetHighlight()
    {
        bubbleText.color = Color.black;
    }

    public void Float()
    {
        float xDuration = Random.Range(2.0f, 4.0f);
        float yDuration = Random.Range(2.0f, 4.0f);

        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + 5f, xDuration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 10f, yDuration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }

    public void Pop()
    {
        // Stop the floating animation
        rectTransform.DOKill();

        // Animate the bubble popping
        Sequence popSequence = DOTween.Sequence();
        popSequence.Append(bubbleImage.DOFade(0, 0.5f));
        popSequence.Join(bubbleText.DOFade(0, 0.5f));
        popSequence.Join(rectTransform.DOScale(rectTransform.localScale * 1.5f, 0.5f));
        popSequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            rectTransform.localScale = new Vector3(1.75f, 1.75f, 1.75f); // Reset scale for next time
            rectTransform.anchoredPosition = initialPosition; // Reset position for next time
        });
    }

    public void ShiftRight(int offset)
    {
        rectTransform.DOKill(); // Stop any ongoing animations for the bubble being shifted
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + offset, rectTransform.anchoredPosition.y);
        Float(); // Restart the floating animation for the shifted bubble
    }

    public void ResetPosition()
    {
        rectTransform.DOKill();
        rectTransform.anchoredPosition = initialPosition;
        Float();
    }
    
}
