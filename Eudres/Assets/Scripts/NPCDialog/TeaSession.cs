using System.Collections.Generic;

public static class TeaSession
{
    public static NPCData CurrentNPC;
    public static TeaRecipe CurrentRecipe;

    //Lista asta se va completa cand se prepara ceaiul in TeaPreparation
    public static List<string> SelectedIngredients = new List<string>();

    // Flag pentru a sti daca tocmai ne-am intors din TeaPreparation
    public static bool IsReturningFromPreparation = false;
}