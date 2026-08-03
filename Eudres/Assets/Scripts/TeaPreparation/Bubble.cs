using DG.Tweening;
using TMPro;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private TMP_Text bubbleText;
    public Keyword keyword;

    void OnEnable()
    {
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

        transform.DOMoveX(transform.position.x + 5f, xDuration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        transform.DOMoveY(transform.position.y + 10f, yDuration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }
}
