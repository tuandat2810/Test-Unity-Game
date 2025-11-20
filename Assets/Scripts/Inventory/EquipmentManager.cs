using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(InventoryManager))]
public class EquipmentManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private InventoryManager inventory;

    // This array holds the currently equipped items
    // We use the EquipmentSlot enum as an index
    private ItemData[] currentEquipment;

    public ItemData GetEquippedItem(ItemData.EquipmentSlot slot)
    {
        return currentEquipment[(int)slot];
    }

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        inventory = GetComponent<InventoryManager>();

        // Initialize the array
        int numSlots = System.Enum.GetNames(typeof(ItemData.EquipmentSlot)).Length;
        currentEquipment = new ItemData[numSlots];
    }

    // This is the main function
    public void EquipItem(ItemData itemToEquip)
    {
        int slotIndex = (int)itemToEquip.equipmentSlot;

        // Check if we already have something in that slot
        ItemData oldItem = currentEquipment[slotIndex];
        if (oldItem != null)
        {
            // Unequip the old item and return it to inventory
            inventory.AddItem(oldItem);
        }

        // Put the new item in the slot
        currentEquipment[slotIndex] = itemToEquip;

        // Remove the new item from the hotbar (optional, but recommended)
        // inventory.RemoveItem(itemToEquip);

        RecalculateStats();
    }

    public void UnequipItem(int slotIndex)
    {
        ItemData itemToUnequip = currentEquipment[slotIndex];
        if (itemToUnequip != null)
        {
            // Add it back to inventory
            if (inventory.AddItem(itemToUnequip))
            {
                // Clear the slot
                currentEquipment[slotIndex] = null;
                RecalculateStats();
            }
            // If inventory is full, we can't unequip (optional logic)
        }
    }

    // This function adds up all bonuses and tells PlayerStats
    private void RecalculateStats()
    {
        int totalHealthBonus = 0;
        int totalDamageBonus = 0;

        // Loop through all equipped items
        foreach (ItemData item in currentEquipment)
        {
            if (item != null)
            {
                totalHealthBonus += item.bonusHealth;
                totalDamageBonus += item.bonusDamage;
            }
        }

        // Send the final totals to PlayerStats
        playerStats.UpdateEquipmentStats(totalHealthBonus, totalDamageBonus);
    }
}
