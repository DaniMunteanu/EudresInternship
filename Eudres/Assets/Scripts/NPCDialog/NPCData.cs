using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "TeaShop/NPC")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public Sprite talkingSprite;

    [Header("Feedback dupa servire")]
    [TextArea(2, 4)] public string serveFeedback;

    [Tooltip("retete NPC")]
    public List<TeaRecipe> possibleRecipes;

    public TeaRecipe GetRandomRecipe()
    {
        if (possibleRecipes == null || possibleRecipes.Count == 0)
        {
            Debug.LogError($"{npcName} nu are retete definite!");
            return null;
        }
        int index = Random.Range(0, possibleRecipes.Count);
        return possibleRecipes[index];
    }
}