using System.Collections.Generic;

public static class TeaSession
{
    public static NPCData CurrentNPC;
    public static TeaRecipe CurrentRecipe;

    public static List<string> SelectedIngredients = new List<string>();

    public static bool IsReturningFromPreparation = false;

    public const int NPCsPerDay = 3;
    public static int NPCsServedToday = 0;
    public static List<NPCData> ServedNPCsToday = new List<NPCData>();
}