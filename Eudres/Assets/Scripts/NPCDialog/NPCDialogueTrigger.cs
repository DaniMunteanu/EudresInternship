using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [Tooltip("Toate NPC-urile care pot aparea la tejghea")]
    public List<NPCData> possibleNPCs;

    void Start()
    {

        if (TeaSession.IsReturningFromPreparation)
        {
            return;
        }

        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        if (possibleNPCs != null && possibleNPCs.Count > 0)
        {
            NPCData npcData = possibleNPCs[Random.Range(0, possibleNPCs.Count)];
            TeaRecipe recipe = npcData.GetRandomRecipe();
            if (recipe != null)
            {
                DialogueManager.Instance.StartDialogue(npcData, recipe);
            }
        }
        else
        {
            Debug.LogWarning($"Obiectul {gameObject.name} nu are NPC-uri asignate în Inspector!");
        }
    }
}