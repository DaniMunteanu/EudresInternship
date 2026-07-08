using UnityEngine;

[CreateAssetMenu(fileName = "IngredientType", menuName = "Scriptable Objects/IngredientType")]
public class IngredientType : ScriptableObject
{
    public string typeName;
    public int maxIngredients;
}
