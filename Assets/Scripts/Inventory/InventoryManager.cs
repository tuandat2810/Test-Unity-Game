using System.Collections.Generic; // To use Lists
using UnityEngine;
using UnityEngine.UI; // To control UI Images

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Data")]
    public List<ItemData> hotbarItems = new List<ItemData>(9); // 9-slot list
    public int maxSlots = 9;

    [Header("UI References")]
    public GameObject hotbarPanel; // Kéo HotbarPanel vào đây
    public GameObject slotPrefab;  // Kéo Slot Prefab vào đây

    // This list stores the actual UI slots we create
    private List<Image> hotbarSlotIcons = new List<Image>();

    void Start()
    {
        // Initialize the list with 9 nulls
        for (int i = 0; i < maxSlots; i++)
        {
            hotbarItems.Add(null);
        }
        
        // Create the 9 UI slots
        SetupUI();
    }

    void SetupUI()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            // Create a new slot from the prefab
            GameObject slotGO = Instantiate(slotPrefab, hotbarPanel.transform);
            
            // Get the Image component (from the "icon" child)
            // (Note: This assumes SlotPrefab has a child Image named "Icon")
            Image iconImage = slotGO.transform.Find("Icon").GetComponent<Image>();
            iconImage.enabled = false; // Hide it initially
            
            hotbarSlotIcons.Add(iconImage);
        }
    }

    public void UpdateUI()
    {
        // Loop through all 9 slots
        for (int i = 0; i < maxSlots; i++)
        {
            if (hotbarItems[i] != null)
            {
                // If there is an item, show its icon
                hotbarSlotIcons[i].sprite = hotbarItems[i].icon;
                hotbarSlotIcons[i].enabled = true;
            }
            else
            {
                // If slot is empty, hide the icon
                hotbarSlotIcons[i].enabled = false;
            }
        }
    }

    // --- PUBLIC FUNCTIONS (Other scripts call these) ---

    public bool AddItem(ItemData item)
    {
        // Find the first empty slot
        for (int i = 0; i < maxSlots; i++)
        {
            if (hotbarItems[i] == null)
            {
                // Add the item and update the UI
                hotbarItems[i] = item;
                UpdateUI();
                return true; // Succeeded
            }
        }
        
        Debug.Log("Hotbar is full!");
        return false; // Failed
    }

    public void UseItem(int slotIndex)
    {
        // Check if the slot is valid
        if (slotIndex < 0 || slotIndex >= maxSlots || hotbarItems[slotIndex] == null)
        {
            return; // Empty slot
        }

        ItemData itemToUse = hotbarItems[slotIndex];
        
        // --- THIS IS WHERE YOU USE THE ITEM ---
        Debug.Log($"Using item: {itemToUse.itemName}");

        // Get the PlayerStats
        PlayerStats stats = GetComponent<PlayerStats>();

        // Apply item effects
        if (itemToUse.itemType == ItemData.ItemType.Consumable)
        {
            stats.Heal(itemToUse.healAmount);
            stats.RestoreStamina(itemToUse.staminaAmount);
            stats.RestoreSanity(itemToUse.sanityAmount);

            // Remove the item after use
            hotbarItems[slotIndex] = null;
            UpdateUI();
        }
        // (Add logic for "Equipment" later)
    }
}