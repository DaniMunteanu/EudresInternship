using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public NPCData npcData;

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
            Debug.LogWarning($"Obiectul {gameObject.name} nu are NPCData asignat în Inspector!");
        }
    }
}