using UnityEngine;

// Require the NPC to have a Rigidbody2D for movement
[RequireComponent(typeof(Rigidbody2D))]
public class PrisonerNPC : MonoBehaviour, IInteractable
{
    [Header("Info")]
    [SerializeField] private string npcName = "Rival Prisoner";

    [Header("Effects")]
    public GameObject damageTextPrefab; 
    public Vector3 textSpawnOffset = new Vector3(0, 1f, 0); 
    // ---

    [Header("Combat Stats")]
    public float currentHealth = 50f;
    public float maxHealth = 50f;
    public float moveSpeed = 2f;      // NPC's movement speed
    public float attackDamage = 5f;   // Damage dealt by NPC
    public float attackRange = 1.2f;  // Attack range 
    public float attackCooldown = 1.5f; // Time between attacks
    public float detectionRange = 5f; // "Sight" range to start chasing

    private float lastAttackTime = 0f;

    // Components
    private Rigidbody2D rb;
    private PlayerStats player; // Store the player reference

    // === NPC STATE MACHINE ===
    private enum State
    {
        Idle,       // Standing still (in Overworld)
        Chasing,    // Chasing the player (in Combat)
        Attacking   // Attacking the player (in Combat)
    }
    private State currentState;

    // --- SETUP & INTERACTION ---
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Ensure no gravity
        currentState = State.Idle;
    }

    // This implements the IInteractable contract
    public string InteractionPrompt => $"Challenge {npcName}";

    // Called by PlayerInteraction when 'F' is pressed
    public void Interact(PlayerStats player)
    {
        if (player != null)
        {
            player.EnterCombatState(); // Put the Player in combat

            this.player = player; // Store the player to attack them
            currentState = State.Chasing; // Put the NPC in combat (chase state)

            Debug.Log($"Player challenged {npcName}! NPC is now Chasing.");
        }
    }

    // --- NPC BRAIN (UPDATE) ---
    void Update()
    {
        // If not in combat (Idle) or the player is null, do nothing
        if (currentState == State.Idle || player == null)
        {
            rb.linearVelocity = Vector2.zero; // Ensure NPC stands still
            return;
        }

        // Calculate distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Decide action based on distance
        if (distanceToPlayer <= attackRange)
        {
            // 1. If close enough: Attack
            currentState = State.Attacking;
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            // 2. If within sight: Chase
            currentState = State.Chasing;
            ChasePlayer();
        }
        else
        {
            // 3. If too far: Go back to Idle (but still in combat)
            currentState = State.Idle; // Stand still for now
            rb.linearVelocity = Vector2.zero;
        }
    }

    // --- BEHAVIORS ---
    private void ChasePlayer()
    {
        // Calculate move direction
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        // (Optional: Add logic to flip NPC sprite based on direction)
    }

    private void AttackPlayer()
    {
        rb.linearVelocity = Vector2.zero; // Stop moving to attack

        // Check attack cooldown
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Ready to attack again
            lastAttackTime = Time.time;

            // Deal damage to the player
            player.TakeDamage(attackDamage);

            Debug.Log($"{npcName} attacked Player! Player health: {player.currentHealth}");
            // (Optional: Play NPC attack animation here)
        }
    }

    // --- TAKING DAMAGE & DYING ---
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"{npcName} took {damageAmount} damage. Health: {currentHealth}");

        // --- SPAM DAMAGE TEXT LOGIC ---
        if (damageTextPrefab != null)
        {
            // 1. Create the text object from the prefab
            Vector3 spawnPosition = transform.position + textSpawnOffset;
            GameObject textGO = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity);

            // 2. Get the script and set the text
            DamageText damageTextScript = textGO.GetComponent<DamageText>();
            if (damageTextScript != null)
            {
                // Set text (e.g., "5") and color (e.g., White)
                damageTextScript.SetText(damageAmount.ToString(), Color.white);
            }
        }

        // Check if the NPC is defeated
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{npcName} has been defeated!");

        // --- VERY IMPORTANT: Return Player to Overworld state ---
        if (player != null)
        {
            player.ExitCombatState();
        }

        // Disable brain, collider, and sprite
        this.enabled = false; // Disable this script (stops Update)
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        // You could also play a death animation here
        // Destroy(gameObject, 2f); // Or destroy it after 2 seconds
    }
}