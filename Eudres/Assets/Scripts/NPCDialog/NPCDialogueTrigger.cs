using UnityEngine;
using System.Collections.Generic;

public class NPCDialogueTrigger : MonoBehaviour
{
    public static NPCDialogueTrigger Instance;

    [Tooltip("Toti NPC-ii care pot aparea intr-o zi")]
    public List<NPCData> allNPCs;

    public NPCData npcData;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (TeaSession.IsReturningFromPreparation)
        {
            return;
        }

        PickNextNPC();
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        if (npcData != null)
        {
            TeaRecipe recipe = npcData.GetRandomRecipe();
            if (recipe != null)
            {
                DialogueManager.Instance.StartDialogue(npcData, recipe);
            }
        }
        else
        {
            Debug.LogWarning($"Obiectul {gameObject.name} nu are NPCData asignat!");
        }
    }

    public void SpawnNextNPC()
    {
        PickNextNPC();
        TriggerDialogue();
    }

    void PickNextNPC()
    {
        List<NPCData> available = allNPCs.FindAll(npc => !TeaSession.ServedNPCsToday.Contains(npc));

        if (available.Count == 0)
            available = allNPCs;

        npcData = available[Random.Range(0, available.Count)];
    }
}