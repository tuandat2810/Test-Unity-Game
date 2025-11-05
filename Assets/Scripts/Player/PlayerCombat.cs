using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(InventoryManager))]
public class PlayerCombat : MonoBehaviour
{
    // References
    private PlayerStats playerStats;
    private Animator anim;
    private PlayerMovement playerMovement; 

    [Header("Stamina Costs")]
    public float punchStaminaCost = 10f;

    [Header("Punch Attack")]
    public float punchDamage = 5f;
    public float punchRange = 0.5f;
    public LayerMask enemyLayer;

    [Header("Attack Offset")]
    public Vector2 attackOffset = new Vector2(0.5f, 0f); 

    [Header("Attack Effects")]
    public GameObject slashEffectPrefab;

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
        // (Inventory input checks are correct)
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventoryManager.UseItem(0); 
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventoryManager.UseItem(1); 
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventoryManager.UseItem(2); 
        
        // Check state
        if (playerStats.currentState != PlayerStats.PlayerState.Combat)
            return; 

        // Check for attack input
        if (Input.GetKeyDown(KeyCode.J))
        {
            Punch();
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

        // 2. Get direction
        Vector2 punchDirection = playerMovement.LastFacingDirection;

        // 3. Trigger Animation
        if (punchDirection.y > 0.5f) anim.SetTrigger("PunchUp");
        else if (punchDirection.y < -0.5f) anim.SetTrigger("PunchDown");
        else if (punchDirection.x < -0.5f) anim.SetTrigger("PunchLeft");
        else anim.SetTrigger("PunchRight");

        // --- CALCULATE POSITION (FOR BOTH FX AND HITBOX) ---
        float angle = Mathf.Atan2(punchDirection.y, punchDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Vector2 rotatedOffset = rotation * attackOffset;
        Vector2 spawnPosition = (Vector2)transform.position + rotatedOffset;

        // --- SPAWN SLASH EFFECT ---
        if (slashEffectPrefab != null)
        {
            Instantiate(slashEffectPrefab, spawnPosition, rotation);
        }

        // --- DETECT HITS ---
        // We removed the old "punchPointPosition" variable
        // We now use "spawnPosition" for the hitbox center
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(spawnPosition, punchRange, enemyLayer); // <-- FIXED LINE

        // --- DEAL DAMAGE ---
        foreach (Collider2D enemy in hitEnemies)
        {
            PrisonerNPC npc = enemy.GetComponent<PrisonerNPC>();
            if (npc != null)
            {
                npc.TakeDamage(punchDamage);
            }
        }
    }

    // (OnDrawGizmosSelected function is already correct)
    private void OnDrawGizmosSelected()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        Vector2 punchDirection = playerMovement ? playerMovement.LastFacingDirection : new Vector2(1, 0);

        float angle = Mathf.Atan2(punchDirection.y, punchDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 rotatedOffset = rotation * attackOffset;
        Vector2 punchPointPosition = (Vector2)transform.position + rotatedOffset;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPointPosition, punchRange);
    }
}