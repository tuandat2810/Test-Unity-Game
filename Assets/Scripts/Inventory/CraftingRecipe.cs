using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Recipe Info")]
    public string recipeName;
    public Sprite icon;


    [System.Serializable] 
    public struct Ingredient 
    {
        public ItemData item;
        public int count;
    }

    [Header("Ingredients")]
    public List<Ingredient> ingredients; 

    [Header("Result")]
    public ItemData outputItem;
    public int outputAmount = 1;
}