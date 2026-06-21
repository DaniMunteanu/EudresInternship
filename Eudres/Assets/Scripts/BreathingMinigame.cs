using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BreathingMinigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform teaBag;
    [SerializeField] private RectTransform cup;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private TextMeshProUGUI cycleCounterText;

    [Header("Settings")]
    [SerializeField] private int requiredCycles = 3;

    public event System.Action OnMinigameComplete;

    private enum Phase { WaitingToInhale, WaitingToExhale, Complete }
    private Phase currentPhase = Phase.WaitingToInhale;

    private Vector2 teaBagStartPosition;
    private int completedCycles = 0;
    private bool isDragging = false;

    void OnEnable()
    {
        completedCycles = 0;
        currentPhase = Phase.WaitingToInhale;

        if (teaBag != null)
            teaBagStartPosition = teaBag.anchoredPosition;

        UpdateUI();
    }

    void Update()
    {
        if (currentPhase == Phase.Complete) return;
        UpdateUI();
    }

    public void OnTeaBagBeginDrag()
    {
        isDragging = true;
    }

    public void OnTeaBagDrag(Vector2 pointerPosition)
    {
        if (!isDragging) return;
        teaBag.position = pointerPosition;
    }

    public void OnTeaBagEndDrag()
    {
        isDragging = false;

        bool isOverCup = IsOverCup();

        if (currentPhase == Phase.WaitingToInhale && isOverCup)
        {
            // Pliculetul a ajuns in cana
            SnapToCup();
            currentPhase = Phase.WaitingToExhale;
        }
        else if (currentPhase == Phase.WaitingToExhale && !isOverCup)
        {
            // Pliculetul a fost ridicat din cana
            ReturnToStart();
            completedCycles++;

            if (completedCycles >= requiredCycles)
            {
                currentPhase = Phase.Complete;
                StartCoroutine(CompleteMinigame());
            }
            else
            {
                currentPhase = Phase.WaitingToInhale;
            }
        }
        else
        {
            // Drop invalid — intoarce la pozitia corecta
            if (currentPhase == Phase.WaitingToInhale)
                ReturnToStart();
            else
                SnapToCup();
        }
    }

    bool IsOverCup()
    {
        if (cup == null || teaBag == null) return false;

        Rect cupRect = GetWorldRect(cup);
        return cupRect.Contains(teaBag.position);
    }

    Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y,
                        corners[2].x - corners[0].x,
                        corners[2].y - corners[0].y);
    }

    void SnapToCup()
    {
        teaBag.position = cup.position;
    }

    void ReturnToStart()
    {
        teaBag.anchoredPosition = teaBagStartPosition;
    }

    void UpdateUI()
    {
        if (cycleCounterText != null)
            cycleCounterText.text = $"{completedCycles} / {requiredCycles}";

        if (instructionText == null) return;

        switch (currentPhase)
        {
            case Phase.WaitingToInhale:
                instructionText.text = isDragging ? "Drag to inhale..." : "Drag the tea bag into the cup";
                break;
            case Phase.WaitingToExhale:
                instructionText.text = isDragging ? "Drag to exhale..." : "Now lift the tea bag out";
                break;
            case Phase.Complete:
                instructionText.text = "Well done!";
                break;
        }
    }

    IEnumerator CompleteMinigame()
    {
        yield return new WaitForSeconds(1f);
        OnMinigameComplete?.Invoke();
    }
}
