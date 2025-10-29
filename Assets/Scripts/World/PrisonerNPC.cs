using UnityEngine;

public class PrisonerNPC : MonoBehaviour, IInteractable
{
    [Header("Info")]
    [SerializeField] private string npcName = "Rival Prisoner";

    [Header("Combat Stats")]
    public float currentHealth = 50f;
    public float maxHealth = 50f;

    public string InteractionPrompt => $"Challenge {npcName}";

    public void Interact(PlayerStats player)
    {
        // Check if player is not null
        if (player != null)
        {
            // === THIS IS THE MISSING LINE ===
            // Tell the PlayerStats script to change the state
            player.EnterCombatState();
        }

        // debug log only at this moment
        Debug.Log($"Player challenged {npcName}!");
    }


    // --- NEW FUNCTION ---
    // This function will be called by PlayerCombat
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"{npcName} took {damageAmount} damage. Health remaining: {currentHealth}");

        // Check if the NPC is defeated
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // For now, just log it and disable the NPC
        Debug.Log($"{npcName} has been defeated!");

        // (Optional: Find the player and switch them back to Overworld state)
        // FindObjectOfType<PlayerStats>().ExitCombatState();

        // Disable the collider and sprite
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        // You could also play a death animation here
        // Destroy(gameObject, 2f); // Or destroy it after 2 seconds
    }
}
