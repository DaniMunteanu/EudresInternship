using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public Sprite ingredientSprite;
    public Sprite cupChangeSprite;
    public Sprite particleSprite;
    public IngredientType ingredientType;
    public Keyword[] keywords;
}
