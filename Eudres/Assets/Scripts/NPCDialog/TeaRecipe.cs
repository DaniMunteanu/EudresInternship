using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTeaRecipe", menuName = "TeaShop/Tea Recipe")]
public class TeaRecipe : ScriptableObject
{
    [Header("Introducerea NPC-ului")]
    [TextArea(2, 4)] public string introText;

    [Header("Seturi de intrebari (3 seturi x 3 intrebari)")]
    public List<QuestionSet> questionSets;

    [Header("Reteta corecta")]
    public List<string> requiredIngredients;
}

[System.Serializable]
public class QuestionSet
{
    public List<DialogueExchange> exchanges;
}

[System.Serializable]
public class DialogueExchange
{
    public string playerQuestion;
    [TextArea(2, 4)] public string npcResponse;
}