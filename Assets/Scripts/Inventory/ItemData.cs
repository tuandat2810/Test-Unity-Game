using UnityEngine;

// This line allows us to create new ItemData assets from the "Create" menu
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject 
{
    // Basic info
    public string itemName = "New Item";
    [TextArea(3, 10)]
    public string description = "Item Description";
    public Sprite icon = null;

    // Type of item (matches your design doc)
    public enum ItemType
    {
        Consumable, // Nước tăng lực, Sách
        Equipment,  // Găng tay, Áo tù
        Material    // Vật liệu chế tạo
    }
    public ItemType itemType;

    // --- Specific Data ---
    // We can add data here that only some items use

    [Header("Consumable Stats")]
    public float healAmount = 0;
    public float staminaAmount = 0;
    public float sanityAmount = 0;

    // You can add more...
    // public float damageIncrease = 0;
}