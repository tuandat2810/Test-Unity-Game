using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Recipe Info")]
    public string recipeName;
    public Sprite icon; // Icon for the recipe UI

    [Header("Ingredients")]
    public List<ItemData> requiredItems; // List of items needed (e.g. Brush, Stone)

    [Header("Result")]
    public ItemData outputItem; // The item you get (e.g. Shiv)
    public int outputAmount = 1; // How many you get
}