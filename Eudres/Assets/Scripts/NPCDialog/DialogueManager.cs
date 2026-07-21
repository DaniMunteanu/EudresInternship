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

    private bool isServeComplete = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        KeywordsTracker.Instance.ClearBubbles();

        if (TeaSession.IsReturningFromPreparation)
            ShowFeedbackAndServe();
    }

    void Update()
    {
        if (isTyping && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            npcDialogueText.maxVisibleCharacters = fullTextBuffer.Length;
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
        TeaSession.SelectedIngredients.Clear();
        currentSetIndex = 0;

        dialoguePanel.SetActive(true);
        feedbackText.text = "";
        makeTeaButton.gameObject.SetActive(false);
        clickToContinueOverlay.SetActive(false);

        npcNameText.text = npc.npcName;

        ClearQuestionButtons();

        DisplayDialogue(recipe.introText, onComplete: () =>
        {
            clickToContinueOverlay.SetActive(true);
        });
    }

    public void OnClickToContinue()
    {
        clickToContinueOverlay.SetActive(false);

        if (isServeComplete)
        {
            isServeComplete = false;
            FinishServingAndProceed();
        }
        else
        {
            ShowQuestionSet(currentSetIndex);
        }
    }

    void ShowQuestionSet(int setIndex)
    {
        var sets = TeaSession.CurrentRecipe.questionSets;

        if (sets == null || setIndex >= sets.Count)
        {
            ShowMakeTeaButton();
            return;
        }

        npcDialogueText.text = "";
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
                DisplayDialogue(response, onComplete: () => AdvanceToNextSet(exchange.keyword));
            });
        }
    }

    void AdvanceToNextSet(Keyword keyword = null)
    {
        if (keyword != null)
        {
            KeywordsTracker.Instance.AddBubble(keyword);
        }

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

    void ShowFeedbackAndServe()
    {
        TeaSession.IsReturningFromPreparation = false;
        dialoguePanel.SetActive(true);
        clickToContinueOverlay.SetActive(false);
        makeTeaButton.gameObject.SetActive(false);

        npcNameText.text = TeaSession.CurrentNPC.npcName;
        feedbackText.text = "";

        DisplayDialogue(TeaSession.CurrentNPC.serveFeedback, onComplete: () =>
        {
            isServeComplete = true;
            clickToContinueOverlay.SetActive(true);
        });
    }

    void FinishServingAndProceed()
    {
        TeaSession.ServedNPCsToday.Add(TeaSession.CurrentNPC);
        TeaSession.NPCsServedToday++;

        feedbackText.text = "";
        ClearQuestionButtons();

        if (TeaSession.NPCsServedToday >= TeaSession.NPCsPerDay)
        {
            TeaSession.NPCsServedToday = 0;
            TeaSession.ServedNPCsToday.Clear();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            dialoguePanel.SetActive(false);
            NPCDialogueTrigger.Instance.SpawnNextNPC();
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

        npcDialogueText.text = text;
        npcDialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            npcDialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
        onTypingComplete = null;
        onComplete?.Invoke();
    }
}