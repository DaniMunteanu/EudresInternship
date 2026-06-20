using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class BreathingMinigameSetup
{
    [MenuItem("Tools/Setup Breathing Minigame")]
    static void Setup()
    {
        // Canvas
        GameObject canvasGO = new GameObject("BreathingMinigame");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Background panel
        GameObject panel = CreateUIObject("Panel", canvasGO);
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.15f, 0.85f);
        StretchFull(panel.GetComponent<RectTransform>());

        // Cana (cup) - jos pe ecran
        GameObject cupGO = CreateUIObject("Cup", panel);
        Image cupImage = cupGO.AddComponent<Image>();
        cupImage.color = new Color(0.6f, 0.35f, 0.1f, 1f);
        RectTransform cupRT = cupGO.GetComponent<RectTransform>();
        cupRT.sizeDelta = new Vector2(120f, 100f);
        cupRT.anchoredPosition = new Vector2(0f, -120f);

        // TeaBag - sus pe ecran, deasupra canei
        GameObject teaBagGO = CreateUIObject("TeaBag", panel);
        teaBagGO.AddComponent<Image>().color = new Color(0.55f, 0.35f, 0.15f, 1f);
        teaBagGO.AddComponent<DraggableTeaBag>();
        RectTransform teaBagRT = teaBagGO.GetComponent<RectTransform>();
        teaBagRT.sizeDelta = new Vector2(50f, 80f);
        teaBagRT.anchoredPosition = new Vector2(0f, 100f);

        // InstructionText
        GameObject instructionGO = CreateUIObject("InstructionText", panel);
        TextMeshProUGUI instructionText = instructionGO.AddComponent<TextMeshProUGUI>();
        instructionText.text = "Drag the tea bag into the cup";
        instructionText.fontSize = 24;
        instructionText.alignment = TextAlignmentOptions.Center;
        instructionText.color = Color.white;
        RectTransform instructionRT = instructionGO.GetComponent<RectTransform>();
        instructionRT.sizeDelta = new Vector2(600f, 60f);
        instructionRT.anchoredPosition = new Vector2(0f, -200f);

        // CycleCounterText
        GameObject counterGO = CreateUIObject("CycleCounterText", panel);
        TextMeshProUGUI counterText = counterGO.AddComponent<TextMeshProUGUI>();
        counterText.text = "0 / 3";
        counterText.fontSize = 20;
        counterText.alignment = TextAlignmentOptions.Center;
        counterText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        RectTransform counterRT = counterGO.GetComponent<RectTransform>();
        counterRT.sizeDelta = new Vector2(300f, 50f);
        counterRT.anchoredPosition = new Vector2(0f, -260f);

        // BreathingMinigame script pe canvas
        BreathingMinigame minigame = canvasGO.AddComponent<BreathingMinigame>();

        // Wireup
        SerializedObject so = new SerializedObject(minigame);
        so.FindProperty("teaBag").objectReferenceValue = teaBagRT;
        so.FindProperty("cup").objectReferenceValue = cupRT;
        so.FindProperty("instructionText").objectReferenceValue = instructionText;
        so.FindProperty("cycleCounterText").objectReferenceValue = counterText;
        so.ApplyModifiedProperties();

        Selection.activeGameObject = canvasGO;
        Debug.Log("Breathing Minigame creat! Apasa Play si trage pliculetul in cana.");
    }

    [MenuItem("Tools/Fix Breathing Texts")]
    static void FixTexts()
    {
        // Cauta Canvas in scena
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Nu am gasit Canvas in scena!");
            return;
        }

        // Fix Canvas render mode
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        EditorUtility.SetDirty(canvas);

        // Cauta toate TextMeshProUGUI din scena
        TextMeshProUGUI[] texts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

        foreach (TextMeshProUGUI tmp in texts)
        {
            RectTransform rt = tmp.GetComponent<RectTransform>();

            // Reset scale
            rt.localScale = Vector3.one;

            if (tmp.gameObject.name == "instructionText" || tmp.gameObject.name == "InstructionText")
            {
                tmp.fontSize = 24;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.color = Color.white;
                rt.sizeDelta = new Vector2(600f, 60f);
                rt.anchoredPosition = new Vector2(0f, -200f);
                EditorUtility.SetDirty(tmp.gameObject);
                Debug.Log("Fixed InstructionText");
            }

            if (tmp.gameObject.name == "cycleCounterText" || tmp.gameObject.name == "CycleCounterText")
            {
                tmp.fontSize = 20;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                rt.sizeDelta = new Vector2(300f, 50f);
                rt.anchoredPosition = new Vector2(0f, -260f);
                EditorUtility.SetDirty(tmp.gameObject);
                Debug.Log("Fixed CycleCounterText");
            }
        }

        Debug.Log("Texte fixate! Salveaza scena cu Ctrl+S.");
    }

    static GameObject CreateUIObject(string name, GameObject parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    static void StretchFull(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
