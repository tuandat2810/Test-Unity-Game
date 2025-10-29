using UnityEngine;

// Automatically add required components
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    // We need references to other components on the Player
    private PlayerStats playerStats;
    private Animator anim;
    // We don't need PlayerMovement, as it runs independently.

    [Header("Combat Stats")]
    public float punchStaminaCost = 10f; // As per your design doc

    [Header("Punch Attack")]
    public float punchDamage = 5f;        // How much damage the punch deals
    public float punchRange = 0.5f;     // How far the punch reaches
    public Transform punchPoint;        // The center of the punch (set this in Inspector)
    public LayerMask enemyLayer;        // Which layer to hit (set this in Inspector)

    void Start()
    {
        // Get the components
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // === NEW STATE CHECK ===
        // First, check if we are in the "Combat" state.
        if (playerStats.currentState != PlayerStats.PlayerState.Combat)
        {
            return; // If not, do nothing.
        }

        // --- ALL LOGIC BELOW ONLY RUNS IF STATE IS "COMBAT" ---

        // Check for Left Mouse Button ("Fire1" is the default)
        if (Input.GetButtonDown("Fire1"))
        {
            Punch();
        }

        // (Future) Check for Right Mouse Button (Block)
        // if (Input.GetButtonDown("Fire2")) { Block(); }

        // (Future) Check for Spacebar (Dodge)
        // if (Input.GetKeyDown(KeyCode.Space)) { Dodge(); }
    }

    private void Punch()
    {
        // Check for stamina
        if (playerStats.currentStamina < punchStaminaCost)
        {
            Debug.Log("Not enough stamina to punch!");
            return; // Stop the function here
        }

        // 1. Use stamina
        playerStats.UseStamina(punchStaminaCost);

        // 2. Play animation (Future)
        // anim.SetTrigger("Punch"); 

        Debug.Log("Player Punched!");

        // 3. --- NEW: DETECT HIT ---
        // Find all colliders in a small circle in front of the player
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, enemyLayer);

        // 4. --- NEW: DEAL DAMAGE ---
        // Loop through all enemies hit and apply damage
        foreach (Collider2D enemy in hitEnemies)
        {
            // Try to get the NPC script from the object we hit
            PrisonerNPC npc = enemy.GetComponent<PrisonerNPC>();
            if (npc != null)
            {
                // If it's a valid NPC, make it take damage
                npc.TakeDamage(punchDamage);
            }
        }
    }

    // This draws a red circle in the Scene view so you can see your punch range
    private void OnDrawGizmosSelected()
    {
        if (punchPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRange);
    }
}