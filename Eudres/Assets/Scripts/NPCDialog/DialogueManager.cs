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

    [Header("Check if the scene is a tutorial")]
    [SerializeField] private bool isTutorialScene = false;
    [SerializeField] string teaPreparationScene = "TeaPreparation";
    [SerializeField] string tutorialTeaPreparationScene = "TutorialTeaPreparation";
    [SerializeField] private GameObject tutorialStepPanel;
    [SerializeField] private GameObject tutorialOverPanel;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI npcDialogueText;
    public Image npcPortraitImage;

    [Header("Questions UI")]
    public Transform questionButtonContainer;
    public GameObject questionButtonPrefab;
    public Button makeTeaButton;

    [Header("Feedback UI")]
    public TextMeshProUGUI feedbackText;

    [Header("Click To Continue")]
    public GameObject clickToContinueOverlay;

    [Header("End Of Day UI")]
    public GameObject endOfDayPanel;
    public TextMeshProUGUI endOfDayText;
    public GameObject endOfDayClickToContinueOverlay;
    [TextArea(2, 4)] public List<string> endOfDayLines = new List<string>();

    private Coroutine typingCoroutine;
    private int currentSetIndex = 0;

    private bool isTyping = false;
    private bool isTypingNpcSpeech = false;
    private string fullTextBuffer = "";
    private System.Action onTypingComplete;
    private TextMeshProUGUI activeTypingTarget;
    
    private bool isServeComplete = false;
    
    private int endOfDayLineIndex = 0;
    private bool firstKeywordAdded = false;
    private bool touchscreenEnabled = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (Touchscreen.current != null)
        {
            touchscreenEnabled = true;
        }

        if (tutorialOverPanel)
        {
            tutorialOverPanel.SetActive(false);
        }
        if (tutorialStepPanel)
        {
            tutorialStepPanel.SetActive(false);
        }

        KeywordsTracker.Instance.ClearBubbles();

        if (TeaSession.IsReturningFromPreparation)
            ShowFeedbackAndServe();
    }

    void Update()
    {
        bool wasTapped = false;
        if(touchscreenEnabled)
        {
            wasTapped = Touchscreen.current.primaryTouch.press.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame;
        }
        else
        {
            wasTapped = Mouse.current.leftButton.wasPressedThisFrame;
        }

        if (isTyping && wasTapped)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            activeTypingTarget.maxVisibleCharacters = fullTextBuffer.Length;
            isTyping = false;

            if (isTypingNpcSpeech)
            {
                npcPortraitImage.sprite = TeaSession.CurrentNPC.npcPortrait;
                npcPortraitImage.SetNativeSize();
            }

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
        npcPortraitImage.sprite = npc.npcPortrait;
        npcPortraitImage.SetNativeSize();

        ClearQuestionButtons();

        DisplayDialogue(recipe.introText, onComplete: () =>
        {
            clickToContinueOverlay.SetActive(true);
        });
    }

    public void OnClickToContinue()
    {
        if (tutorialStepPanel)
        {
            tutorialStepPanel.SetActive(false);
        }

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

            if (isTutorialScene && !firstKeywordAdded && tutorialStepPanel != null)
            {
                firstKeywordAdded = true;
                tutorialStepPanel.SetActive(true);
            }
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
        if (isTutorialScene)
            makeTeaButton.onClick.AddListener(() => SceneManager.LoadScene(tutorialTeaPreparationScene));
        else
            makeTeaButton.onClick.AddListener(() => SceneManager.LoadScene(teaPreparationScene));
    }
    
    void ShowFeedbackAndServe()
    {
        TeaSession.IsReturningFromPreparation = false;
        dialoguePanel.SetActive(true);
        clickToContinueOverlay.SetActive(false);
        makeTeaButton.gameObject.SetActive(false);

        npcNameText.text = TeaSession.CurrentNPC.npcName;
        npcPortraitImage.sprite = TeaSession.CurrentNPC.npcPortrait;
        npcPortraitImage.SetNativeSize();
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

        if (isTutorialScene)
        {
            TeaSession.NPCsServedToday = 0;
            TeaSession.ServedNPCsToday.Clear();
            if (tutorialOverPanel)
            {
                tutorialOverPanel.SetActive(true);
            }
            return;
        }

        if (TeaSession.NPCsServedToday >= TeaSession.NPCsPerDay)
        {
            ShowEndOfDayScreen();
        }
        else
        {
            dialoguePanel.SetActive(false);
            NPCDialogueTrigger.Instance.SpawnNextNPC();
        }
    }
    
    void ShowEndOfDayScreen()
    {
        dialoguePanel.SetActive(false);
        endOfDayPanel.SetActive(true);
        endOfDayClickToContinueOverlay.SetActive(false);

        endOfDayLineIndex = 0;
        ShowNextEndOfDayLine();
    }

    void ShowNextEndOfDayLine()
    {
        if (endOfDayLines == null || endOfDayLineIndex >= endOfDayLines.Count)
        {
            FinishEndOfDay();
            return;
        }

        DisplayDialogue(endOfDayLines[endOfDayLineIndex], endOfDayText, onComplete: () =>
        {
            endOfDayClickToContinueOverlay.SetActive(true);
        });
    }
    
    public void OnEndOfDayContinue()
    {
        endOfDayClickToContinueOverlay.SetActive(false);
        endOfDayLineIndex++;

        if (endOfDayLines != null && endOfDayLineIndex < endOfDayLines.Count)
        {
            ShowNextEndOfDayLine();
        }
        else
        {
            FinishEndOfDay();
        }
    }

    void FinishEndOfDay()
    {
        endOfDayPanel.SetActive(false);

        TeaSession.NPCsServedToday = 0;
        TeaSession.ServedNPCsToday.Clear();

        SceneManager.LoadScene("MainMenu");
    }

    void ClearQuestionButtons()
    {
        foreach (Transform child in questionButtonContainer)
            Destroy(child.gameObject);
    }
    
    void DisplayDialogue(string text, System.Action onComplete = null)
    {
        DisplayDialogue(text, npcDialogueText, onComplete);
    }

    void DisplayDialogue(string text, TextMeshProUGUI target, System.Action onComplete = null)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(text, target, onComplete));
    }

    IEnumerator TypeText(string text, TextMeshProUGUI target, System.Action onComplete)
    {
        isTyping = true;
        fullTextBuffer = text;
        onTypingComplete = onComplete;
        activeTypingTarget = target;

        isTypingNpcSpeech = target == npcDialogueText && TeaSession.CurrentNPC != null;
        if (isTypingNpcSpeech)
        {
            Sprite talking = TeaSession.CurrentNPC.talkingSprite != null
                ? TeaSession.CurrentNPC.talkingSprite
                : TeaSession.CurrentNPC.npcPortrait;
            npcPortraitImage.sprite = talking;
            npcPortraitImage.SetNativeSize();
        }

        target.text = text;
        target.maxVisibleCharacters = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            target.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
        onTypingComplete = null;

        if (isTypingNpcSpeech)
        {
            npcPortraitImage.sprite = TeaSession.CurrentNPC.npcPortrait;
            npcPortraitImage.SetNativeSize();
        }

        onComplete?.Invoke();
    }
}