using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public string recipeDescription;
    public List<Ingredient> ingredients;
}
