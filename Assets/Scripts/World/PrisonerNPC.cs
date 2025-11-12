using UnityEngine;
using System.Collections;

// Require the NPC to have a Rigidbody2D for movement
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class PrisonerNPC : MonoBehaviour, IInteractable
{
    [Header("Info")]
    [SerializeField] private string npcName = "Rival Prisoner";

    [Header("Effects")]
    public GameObject damageTextPrefab; 
    public Vector3 textSpawnOffset = new Vector3(0, 1f, 0); 
    public Material flashMaterial;
    public float flashDuration = 0.1f;
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

    [Header("Knockback Settings")]
    public float knockbackForce = 3f; // Force this NPC deals
    public float knockbackDuration = 0.1f; // Stun duration this NPC deals

    private AudioSource audioSource;
    [Header("Audio")]
    public AudioClip hitSound;

    // Components
    private Rigidbody2D rb;
    private PlayerStats player; // Store the player reference
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashCoroutine;

    // === NPC STATE MACHINE ===
    private enum State
    {
        Idle,       // Standing still (in Overworld)
        Chasing,    // Chasing the player (in Combat)
        Attacking,   // Attacking the player (in Combat)
        Stunned     // Temporarily unable to act    
    }
    private State currentState;
    private Coroutine knockbackCoroutine;   


    // --- SETUP & INTERACTION ---
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Ensure no gravity
        currentState = State.Idle;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        audioSource = GetComponent<AudioSource>();
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
        // If stunned (from knockback), do nothing.
        if (currentState == State.Stunned)
        {
            rb.linearVelocity = Vector2.zero; // Make sure we are stopped
            return;
        }

        // If the player doesn't exist (e.g., scene change), go to Idle
        if (player == null)
        {
            currentState = State.Idle;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // --- NEW STATE MACHINE LOGIC ---
        
        // Calculate distance ONCE
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Use a "switch" to manage states cleanly
        switch (currentState)
        {
            case State.Idle:
                // If player comes into detection range, start chasing
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = State.Chasing;
                }
                rb.linearVelocity = Vector2.zero; // Stay still
                break;
                
            case State.Chasing:
                // If close enough, switch to Attacking
                if (distanceToPlayer <= attackRange)
                {
                    currentState = State.Attacking;
                    rb.linearVelocity = Vector2.zero; // Stop chasing immediately
                }
                // If player gets too far, go back to Idle
                else if (distanceToPlayer > detectionRange)
                {
                    currentState = State.Idle;
                }
                else // Still in range, keep chasing
                {
                    ChasePlayer(); // This sets rb.velocity
                }
                break;
                
            case State.Attacking:
                // We are in attack range. Stay in this state.
                // Keep velocity at 0 (set in Chase state)
                rb.linearVelocity = Vector2.zero;
                
                // Try to attack (AttackPlayer handles its own cooldown)
                AttackPlayer(); 

                // Check if we should stop attacking (player ran away)
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Chasing;
                }
                break;
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

            // Apply knockback to the player
            Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
            player.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);   

            Debug.Log($"{npcName} attacked Player! Player health: {player.currentHealth}");
            // (Optional: Play NPC attack animation here)
        }
    }

    // --- TAKING DAMAGE & DYING ---
    public void TakeDamage(float damageAmount)
    {
        // Play hit sound
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"{npcName} took {damageAmount} damage. Health: {currentHealth}");

        FlashEffect();

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

    public void FlashEffect()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;
        flashCoroutine = null;
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

    // This function will be called by PlayerCombat
    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }
        knockbackCoroutine = StartCoroutine(KnockbackRoutine(direction, force, duration));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force, float duration)
    {
        // 1. Enter Stunned state
        currentState = State.Stunned;
        
        // 2. Apply the knockback force
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        // 3. Wait for the stun duration
        yield return new WaitForSeconds(duration);

        // 4. Reset velocity and return to chasing
        rb.linearVelocity = Vector2.zero;
        if (currentHealth > 0) // Don't chase if dead
        {
            currentState = State.Chasing;
        }
        
        knockbackCoroutine = null;
    }
}