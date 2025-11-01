using UnityEngine;

// Automatically requires a Collider
[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    public ItemData itemToGive; // Assign your ScriptableObject (e.g., "NướcTăngLực") here

    private void Start()
    {
        // Ensure the collider is a trigger so we can walk through it
        GetComponent<Collider2D>().isTrigger = true;
    }

    // Called when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is the Player
        if (other.CompareTag("Player"))
        {
            // Get the Player's InventoryManager
            InventoryManager inv = other.GetComponent<InventoryManager>();
            
            // Try to add the item
            if (inv.AddItem(itemToGive))
            {
                // If adding was successful (inventory not full),
                // destroy this pickup object
                Destroy(gameObject); 
            }
        }
    }
}