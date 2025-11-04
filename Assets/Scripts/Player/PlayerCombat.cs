using UnityEngine;

// We removed SpriteRenderer, as we now have 4 dedicated animations
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(InventoryManager))]
public class PlayerCombat : MonoBehaviour
{
    // References
    private PlayerStats playerStats;
    private Animator anim;
    private PlayerMovement playerMovement; // Needed to get direction

    [Header("Stamina Costs")]
    public float punchStaminaCost = 10f;

    [Header("Punch Attack")]
    public float punchDamage = 5f;
    public float punchRange = 0.5f;
    public LayerMask enemyLayer;

    [Header("Punch Offset")]
    public float punchOffset = 0.5f; // How far the punch reaches out

    [Header("Attack Effects")]
    public GameObject slashEffectPrefab; 
    public float effectOffset = 0.5f;

    private InventoryManager inventoryManager;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        inventoryManager = GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (playerStats.currentState != PlayerStats.PlayerState.Combat)
            return; // Not in combat

        // Check for Left Mouse Button
        if (Input.GetKeyDown(KeyCode.J))
        {
            Punch();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            inventoryManager.UseItem(0); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            inventoryManager.UseItem(1); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            inventoryManager.UseItem(2); 
        }
    }

    private void Punch()
    {
        if (playerStats.currentStamina < punchStaminaCost)
        {
            Debug.Log("Not enough stamina to punch!");
            return;
        }

        // 1. Use Stamina
        playerStats.UseStamina(punchStaminaCost);

        // 2. Get the facing direction from PlayerMovement
        Vector2 punchDirection = playerMovement.LastFacingDirection;

        // 3. Trigger the correct 4-way animation
        // This logic checks Y-axis (Up/Down) first
        if (punchDirection.y > 0.5f)
        {
            anim.SetTrigger("PunchUp");
        }
        else if (punchDirection.y < -0.5f)
        {
            anim.SetTrigger("PunchDown");
        }
        // If not Up/Down, check X-axis (Left/Right)
        else if (punchDirection.x < -0.5f)
        {
            anim.SetTrigger("PunchLeft");
        }
        else // (punchDirection.x > 0.5f) or default
        {
            anim.SetTrigger("PunchRight");
        }

        // --- NEW: SPAWN SLASH EFFECT ---
        if (slashEffectPrefab != null)
        {
            Vector2 spawnPos = (Vector2)transform.position + (punchDirection * effectOffset);

            float angle = Mathf.Atan2(punchDirection.y, punchDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Instantiate(slashEffectPrefab, spawnPos, rotation);
        }

        // 4. Calculate the Hitbox Position
        // (This code works for all 4 directions)
        Vector2 punchPointPosition = (Vector2)transform.position + (punchDirection * punchOffset);

        // 5. Detect Hits
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punchPointPosition, punchRange, enemyLayer);

        // 6. Deal Damage
        foreach (Collider2D enemy in hitEnemies)
        {
            PrisonerNPC npc = enemy.GetComponent<PrisonerNPC>();
            if (npc != null)
            {
                npc.TakeDamage(punchDamage);
            }
        }
    }

    // Draws the red hitbox circle in the editor
    private void OnDrawGizmosSelected()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        Vector2 punchDirection = playerMovement ? playerMovement.LastFacingDirection : new Vector2(1, 0);
        Vector2 punchPointPosition = (Vector2)transform.position + (punchDirection * punchOffset);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPointPosition, punchRange);
    }
}