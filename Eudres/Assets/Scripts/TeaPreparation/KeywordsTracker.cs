using UnityEngine;

public class KeywordsTracker : MonoBehaviour
{
    [SerializeField] private KeywordGroup requiredKeywords;
    public bool isPreparingTea = false;
    public Bubble[] bubbles = new Bubble[6];

    public static KeywordsTracker Instance;

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
        InitializeBubbles();
        if (isPreparingTea)
        {
            ShowBubbles();
        }
    }

    void InitializeBubbles()
    {
        for (int index = 0; index < bubbles.Length; index++)
        {
            bubbles[index].gameObject.SetActive(false);
        }
    }

    public void AddBubble(Keyword keyword)
    {
        for (int index = 0; index < bubbles.Length; index++)
        {
            if (bubbles[index].keyword == null)
            {
                requiredKeywords.keywords.Add(keyword);

                bubbles[index].keyword = keyword;
                bubbles[index].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void PopBubble(Keyword keyword)
    {
        for (int index = 0; index < requiredKeywords.keywords.Count; index++)
        {
            if (bubbles[index].keyword == keyword)
            {
                bubbles[index].gameObject.SetActive(false);
                break;
            }
        }
    }

    public void ShowBubbles()
    {
        for (int index = 0; index < requiredKeywords.keywords.Count; index++)
        {
            bubbles[index].keyword = requiredKeywords.keywords[index];
            bubbles[index].gameObject.SetActive(true);
        }
    }

    public void ClearBubbles()
    {
        requiredKeywords.keywords.Clear();

        for (int index = 0; index < bubbles.Length; index++)
        {
            bubbles[index].keyword = null;
            bubbles[index].gameObject.SetActive(false);
        }
    }

    public bool AreAllBubblesPopped()
    {
        for (int index = 0; index < requiredKeywords.keywords.Count; index++)
        {
            if (bubbles[index].gameObject.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    public void HighlightBubble(Keyword keyword)
    {
        for (int index = 0; index < bubbles.Length; index++)
        {
            if (bubbles[index].keyword == keyword)
            {
                bubbles[index].Highlight();
                break;
            }
        }
    }

    public void ResetBubbleHighlights()
    {
        for (int index = 0; index < bubbles.Length; index++)
        {
            if (bubbles[index].keyword != null)
            {
                bubbles[index].ResetHighlight();
            }
        }
    }
}
