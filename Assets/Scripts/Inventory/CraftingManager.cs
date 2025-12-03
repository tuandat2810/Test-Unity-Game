using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryManager))]
public class CraftingManager : MonoBehaviour
{
    private InventoryManager inventory;

    void Start()
    {
        inventory = GetComponent<InventoryManager>();
    }

    // Check if we have all ingredients for the recipe
    public bool CanCraft(CraftingRecipe recipe)
    {
        // Create a temporary copy of inventory to simulate usage
        // (We don't want to count the same item twice if we need 2 of them)
        List<ItemData> tempInventory = new List<ItemData>(inventory.hotbarItems);

        foreach (CraftingRecipe.Ingredient ing in recipe.ingredients)
        {
            int amountNeeded = ing.count;
            
            for (int i = 0; i < tempInventory.Count; i++)
            {
                if (tempInventory[i] == ing.item)
                {
                    tempInventory[i] = null; // Mark this slot as "used" locally
                    amountNeeded--;
                    
                    if (amountNeeded <= 0) break; // Got enough of this ingredient
                }
            }

            // If after checking all slots we still need more, we can't craft
            if (amountNeeded > 0) return false;
        }

        // Also check if we have space for the output item
        // (Unless the output replaces an ingredient slot, but let's be safe)
        // Simple check: Do we have ANY null slots left after simulation? 
        // Or simpler: just try to AddItem later.
        
        return true;
    }

    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log("Not enough ingredients!");
            return;
        }

        // 1. Remove ingredients
        foreach (CraftingRecipe.Ingredient ing in recipe.ingredients)
        {
            int amountToRemove = ing.count;

            for (int i = 0; i < inventory.maxSlots; i++)
            {
                if (inventory.hotbarItems[i] == ing.item)
                {
                    inventory.hotbarItems[i] = null; // Remove item
                    amountToRemove--;

                    if (amountToRemove <= 0) break; // Removed enough, move to next ingredient
                }
            }
        }

        // 2. Add output item
        inventory.AddItem(recipe.outputItem);

        // 3. Refresh UI
        inventory.UpdateUI();
        
        Debug.Log($"Crafted: {recipe.outputItem.itemName}");
    }
}