using UnityEngine;
using TMPro;

public class CupController : MonoBehaviour
{
    [Header("References")]
    public GameObject greenBox;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI cycleCounterText;

    [Header("Settings")]
    public int requiredCycles = 3;

    private enum Phase { WaitingToInhale, WaitingToExhale, Complete }
    private Phase currentPhase = Phase.WaitingToInhale;

    private int completedCycles = 0;
    private bool isTeaBagInCup = false;
    private PlayerController teaBag;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (currentPhase == Phase.Complete) return;

        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        teaBag = collision.GetComponent<PlayerController>();
        isTeaBagInCup = true;

        if (greenBox != null)
            greenBox.SetActive(true);

        if (currentPhase == Phase.WaitingToInhale)
            currentPhase = Phase.WaitingToExhale;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isTeaBagInCup = false;

        if (greenBox != null)
            greenBox.SetActive(false);

        if (currentPhase == Phase.WaitingToExhale)
        {
            completedCycles++;

            if (completedCycles >= requiredCycles)
            {
                currentPhase = Phase.Complete;
            }
            else
            {
                currentPhase = Phase.WaitingToInhale;
            }

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (cycleCounterText != null)
            cycleCounterText.text = $"{completedCycles} / {requiredCycles}";

        if (instructionText == null) return;

        bool isDragging = teaBag != null && teaBag.IsDragging;

        switch (currentPhase)
        {
            case Phase.WaitingToInhale:
                instructionText.text = isDragging ? "Drag to inhale..." : "Drag the tea bag into the cup";
                break;
            case Phase.WaitingToExhale:
                instructionText.text = isDragging ? "Drag to exhale..." : "Lift the tea bag out of the cup";
                break;
            case Phase.Complete:
                instructionText.text = "Well done!";
                break;
        }
    }
}
