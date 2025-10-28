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
    // public float punchDamage = 5f; // We can add this later

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
        // Check if we have enough stamina
        if (playerStats.currentStamina >= punchStaminaCost)
        {
            // 1. Use stamina
            playerStats.UseStamina(punchStaminaCost);

            // 2. Play animation (We will set this up in the Animator later)
            // anim.SetTrigger("Punch"); 

            Debug.Log("Player Punched! Stamina left: " + playerStats.currentStamina);

            // 3. (Future) Detect enemy and deal damage
            // DealDamageToEnemy();
        }
        else
        {
            Debug.Log("Not enough stamina to punch!");
        }
    }
}