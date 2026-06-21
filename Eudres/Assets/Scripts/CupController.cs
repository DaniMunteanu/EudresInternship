using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CupController : MonoBehaviour
{
    [Header("References")]
    public GameObject greenBox;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI cycleCounterText;
    public Slider progressBar;
    public PlayerController teaBag;

    [Header("Settings")]
    public int requiredCycles = 3;

    private enum Phase { WaitingToInhale, Inhaling, WaitingToExhale, Exhaling, Complete }
    private Phase currentPhase = Phase.WaitingToInhale;

    private int completedCycles = 0;
    private bool isTeaBagInCup = false;

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 1f;
        }

        UpdateUI();
    }

    void Update()
    {
        if (currentPhase == Phase.Complete) return;
        if (teaBag == null) return;

        UpdateProgressBar();
        UpdatePhase();
        UpdateUI();
    }

    void UpdatePhase()
    {
        switch (currentPhase)
        {
            case Phase.WaitingToInhale:
                if (teaBag.IsHeld)
                    currentPhase = Phase.Inhaling;
                break;

            case Phase.Inhaling:
                if (teaBag.IsAtBottom())
                    currentPhase = Phase.WaitingToExhale;
                break;

            case Phase.WaitingToExhale:
                if (!teaBag.IsHeld)
                    currentPhase = Phase.Exhaling;
                break;

            case Phase.Exhaling:
                if (teaBag.IsAtTop())
                {
                    completedCycles++;
                    currentPhase = completedCycles >= requiredCycles
                        ? Phase.Complete
                        : Phase.WaitingToInhale;
                }
                break;
        }
    }

    void UpdateProgressBar()
    {
        if (progressBar == null) return;
        progressBar.value = teaBag.NormalizedPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isTeaBagInCup = true;
        if (greenBox != null) greenBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isTeaBagInCup = false;
        if (greenBox != null) greenBox.SetActive(false);
    }

    void UpdateUI()
    {
        if (cycleCounterText != null)
            cycleCounterText.text = $"{completedCycles} / {requiredCycles}";

        if (instructionText == null) return;

        switch (currentPhase)
        {
            case Phase.WaitingToInhale:
                instructionText.text = "Hold to inhale...";
                break;
            case Phase.Inhaling:
                instructionText.text = "Inhaling...";
                break;
            case Phase.WaitingToExhale:
                instructionText.text = "Release to exhale...";
                break;
            case Phase.Exhaling:
                instructionText.text = "Exhaling...";
                break;
            case Phase.Complete:
                instructionText.text = "Well done!";
                break;
        }
    }
}
