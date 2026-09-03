using DG.Tweening;
using UnityEngine;

public class ResizeOnEnable : MonoBehaviour
{
    private Vector3 originalScale;
    [SerializeField] private float delay = 0f;
    [SerializeField] private float multiplier = 1.2f;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (gameObject.GetComponent<ResizeOnHover>() != null)
        {
            gameObject.GetComponent<ResizeOnHover>().isResizable = false;
        }
        originalScale = rectTransform.localScale;
        ScaleObject();
    }

    void OnDisable()
    {
        rectTransform.localScale = originalScale;
    }

    void ScaleObject()
    {
        rectTransform.DOScale(originalScale * multiplier, 0.5f)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetLink(gameObject)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        if (gameObject.GetComponent<ResizeOnHover>() != null)
                            {
                                gameObject.GetComponent<ResizeOnHover>().isResizable = true;
                            }
                    });
    }
}
