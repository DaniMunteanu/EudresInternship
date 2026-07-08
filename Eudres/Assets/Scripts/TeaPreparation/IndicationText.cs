using System.Collections;
using TMPro;
using UnityEngine;

public class IndicationText : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    public static IndicationText Instance;
    private Coroutine currentCoroutine;

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
        textField.text = "";
    }

    public void DisplayText(string text)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        textField.text = text;
        currentCoroutine = StartCoroutine(ClearTextAfterDelay());
    }

    IEnumerator ClearTextAfterDelay()
    {
        // wait for 2 seconds
        yield return new WaitForSeconds(2.0f);
        textField.text = "";
    }
}
