using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipeSet", menuName = "Items/RecipeSet")]
public class RecipeSet : ScriptableObject
{
    public List<Recipe> Recipes;
}
