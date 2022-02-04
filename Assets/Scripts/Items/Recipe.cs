using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ingredient
{
    [SerializeField] private Item Item;
    [SerializeField] private int Quantity;

    public Item GetItem() { return Item; }
    public int GetQuantity() { return Quantity; }
}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Items/Recipe")]
public class Recipe : ScriptableObject
{
    public List<Ingredient> Inputs;
    public List<Ingredient> Outputs;
    public float Time = 1;
}
