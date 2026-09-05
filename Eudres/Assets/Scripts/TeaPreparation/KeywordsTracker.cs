using DG.Tweening;
using UnityEngine;

public class KeywordsTracker : MonoBehaviour
{
    [SerializeField] private KeywordGroup requiredKeywords;
    public bool isPreparingTea = false;
    public Bubble[] bubbles = new Bubble[6];
    private const int BUBBLE_DISTANCE = 170; // Distance between bubbles in pixels

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
                bubbles[index].Float(); // Start floating animation for the new bubble
                break;
            }
        }
    }

    public void PopBubble(Keyword keyword)
    {
        for (int index = 0; index < bubbles.Length; index++)
        {
            if (bubbles[index].keyword == keyword)
            {
                bubbles[index].Pop();
                ShiftBubbles(index);
                break;
            }
        }
    }

    public void ShiftBubbles(int poppedIndex)
    {
        if (poppedIndex == requiredKeywords.keywords.Count - 1)
        {
            return;
        }

        for (int index = poppedIndex + 1; index < bubbles.Length; index++)
        {
            // skip inactive bubbles to avoid shifting them
            if (!bubbles[index].isActiveAndEnabled)
            {
                continue;
            }

            bubbles[index].ShiftRight(BUBBLE_DISTANCE);
        }
    }

    public void ShowBubbles()
    {
        for (int index = 0; index < requiredKeywords.keywords.Count; index++)
        {
            bubbles[index].keyword = requiredKeywords.keywords[index];
            bubbles[index].ResetPosition(); // Reset the position of the bubble before showing it
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
