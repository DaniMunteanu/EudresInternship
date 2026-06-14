using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "TeaShop/NPC")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;

    [Tooltip("2-3 retete posibile pentru acest NPC")]
    public List<TeaRecipe> possibleRecipes;

    // Returneaza o reteta aleatorie la fiecare vizita
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