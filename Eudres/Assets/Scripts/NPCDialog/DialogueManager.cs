using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public Image npcPortraitImage;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI npcDialogueText;

    [Header("Questions UI")]
    public Transform questionButtonContainer;
    public GameObject questionButtonPrefab;
    public Button makeTeaButton;

    [Header("Feedback UI")]
    public TextMeshProUGUI feedbackText;

    [Header("Click To Continue")]
    public GameObject clickToContinueOverlay;

    private Coroutine typingCoroutine;
    private int currentSetIndex = 0;

    private bool isTyping = false;
    private string fullTextBuffer = "";
    private System.Action onTypingComplete;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (TeaSession.IsReturningFromPreparation)
            ShowFeedbackAndServe();
    }

    void Update()
    {
        if (isTyping && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            npcDialogueText.text = fullTextBuffer;
            isTyping = false;

            System.Action callback = onTypingComplete;
            onTypingComplete = null;
            callback?.Invoke();
        }
    }

    public void StartDialogue(NPCData npc, TeaRecipe recipe)
    {
        TeaSession.CurrentNPC = npc;
        TeaSession.CurrentRecipe = recipe;
        TeaSession.IsReturningFromPreparation = false;
        currentSetIndex = 0;

        dialoguePanel.SetActive(true);
        feedbackText.text = "";
        makeTeaButton.gameObject.SetActive(false);
        clickToContinueOverlay.SetActive(false);

        npcNameText.text = npc.npcName;
        npcPortraitImage.sprite = npc.npcPortrait;

        ClearQuestionButtons();

        DisplayDialogue(recipe.introText, onComplete: () =>
        {
            clickToContinueOverlay.SetActive(true);
        });
    }

    public void OnClickToContinue()
    {
        clickToContinueOverlay.SetActive(false);
        ShowQuestionSet(currentSetIndex);
    }

    void ShowQuestionSet(int setIndex)
    {
        var sets = TeaSession.CurrentRecipe.questionSets;

        if (sets == null || setIndex >= sets.Count)
        {
            ShowMakeTeaButton();
            return;
        }

        ClearQuestionButtons();

        foreach (var exchange in sets[setIndex].exchanges)
        {
            GameObject btn = Instantiate(questionButtonPrefab, questionButtonContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = exchange.playerQuestion;

            string response = exchange.npcResponse;
            Button button = btn.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                ClearQuestionButtons();
                DisplayDialogue(response, onComplete: AdvanceToNextSet);
            });
        }
    }

    void AdvanceToNextSet()
    {
        currentSetIndex++;

        if (currentSetIndex < TeaSession.CurrentRecipe.questionSets.Count)
        {
            clickToContinueOverlay.SetActive(true);
        }
        else
        {
            ShowMakeTeaButton();
        }
    }

    IEnumerator ShowNextSetWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowQuestionSet(currentSetIndex);
    }

    void ShowMakeTeaButton()
    {
        makeTeaButton.gameObject.SetActive(true);
        makeTeaButton.onClick.RemoveAllListeners();
        makeTeaButton.onClick.AddListener(() => SceneManager.LoadScene("TeaPreparation"));
    }

    // Feedback dupa servire
    void ShowFeedbackAndServe()
    {
        TeaSession.IsReturningFromPreparation = false;
        dialoguePanel.SetActive(true);
        clickToContinueOverlay.SetActive(false);

        npcNameText.text = TeaSession.CurrentNPC.npcName;
        npcPortraitImage.sprite = TeaSession.CurrentNPC.npcPortrait;

        bool isCorrect = TeaSession.SelectedIngredients.Count ==
                         TeaSession.CurrentRecipe.requiredIngredients.Count;

        if (isCorrect)
        {
            foreach (string required in TeaSession.CurrentRecipe.requiredIngredients)
            {
                if (!TeaSession.SelectedIngredients.Contains(required))
                {
                    isCorrect = false;
                    break;
                }
            }
        }

        if (isCorrect)
        {
            feedbackText.color = Color.green;
            feedbackText.text = "Success! Perfect match.";
            DisplayDialogue("Wow! It smells incredible and tastes exactly how I wanted. Thank you!");
        }
        else
        {
            feedbackText.color = Color.red;
            feedbackText.text = "Failure... Recipe mismatch.";
            DisplayDialogue("Uh... what did you put in this? It tastes a bit strange. Not what I asked for.");
        }
    }

    void ClearQuestionButtons()
    {
        foreach (Transform child in questionButtonContainer)
            Destroy(child.gameObject);
    }

    void DisplayDialogue(string text, System.Action onComplete = null)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(text, onComplete));
    }

    IEnumerator TypeText(string text, System.Action onComplete)
    {
        isTyping = true;
        fullTextBuffer = text;
        onTypingComplete = onComplete;

        npcDialogueText.text = "";
        foreach (char c in text)
        {
            npcDialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
        onTypingComplete = null;
        onComplete?.Invoke();
    }
}