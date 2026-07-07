using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public Sprite ingredientSprite;
    public IngredientType ingredientType;
    public Keyword[] keywords;
}

public enum IngredientType
{
    Base,
    Fruit,
    Addon
}
