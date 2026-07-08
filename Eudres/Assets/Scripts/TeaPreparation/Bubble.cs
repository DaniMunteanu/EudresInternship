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
    }
}
