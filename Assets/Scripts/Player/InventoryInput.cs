using UnityEngine;

// This script's only job is to listen for inventory hotkeys.
[RequireComponent(typeof(InventoryManager))]
public class InventoryInput : MonoBehaviour
{
    private InventoryManager inventoryManager;

    // Testing before UI integration
    public CraftingRecipe testRecipe;
    public CraftingManager craftingManager;

    [Header("UI References")]
    public GameObject craftingScreen;

    void Start()
    {
        // Get the InventoryManager component on this same GameObject
        inventoryManager = GetComponent<InventoryManager>();

        // Testing craft
        craftingManager = GetComponent<CraftingManager>();
    }

    void Update()
    {
        // These inputs should work regardless of combat state,
        // so we don't need to check the state.

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventoryManager.UseItem(0); // Use item in slot 0
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventoryManager.UseItem(1); // Use item in slot 1
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventoryManager.UseItem(2); // Use item in slot 2
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventoryManager.UseItem(3); // Use item in slot 3
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            inventoryManager.UseItem(4); // Use item in slot 4
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            inventoryManager.UseItem(5); // Use item in slot 5
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            inventoryManager.UseItem(6); // Use item in slot 6
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            inventoryManager.UseItem(7); // Use item in slot 7
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            inventoryManager.UseItem(8); // Use item in slot 8
        }


        // Toggle crafting screen
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isActive = craftingScreen.activeSelf;
            craftingScreen.SetActive(!isActive);

            // (Optional) Pause the game when crafting screen is active
            Time.timeScale = !isActive ? 0f : 1f;
        }
    }
}