using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    public string details;

    private IEnumerator tooltipShowCoroutine;

    void Start()
    {
        tooltipShowCoroutine = ShowText(3.0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //I tried displaying the tooltip with a small delay, doesn't work properly yet
        //StartCoroutine(tooltipShowCoroutine);

        TooltipManager.Instance.Show(text, details);
    }

    IEnumerator ShowText(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TooltipManager.Instance.Show(text, details);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(tooltipShowCoroutine);
        TooltipManager.Instance.Hide();
    }
}
