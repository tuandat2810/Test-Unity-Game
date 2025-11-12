using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(AudioSource))]
public class PlayerCombat : MonoBehaviour
{
    // References
    private PlayerStats playerStats;
    private Animator anim;
    private PlayerMovement playerMovement; 

    private AudioSource audioSource;

    [Header("Punch Attack")]
    public float punchRange = 0.5f;
    public LayerMask enemyLayer;
    public Vector2 attackOffset = new Vector2(0.5f, 0f); 
    public GameObject slashEffectPrefab;
    public AudioClip punchSwooshSound;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f; // Force the player deals
    public float knockbackDuration = 0.15f; // Stun duration the player deals

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        audioSource = GetComponent<AudioSource>();  
    }

    void Update()
    {
    }

    public void PerformSkill(SkillData skill)
    {
        // 0. Check if combat state or not
        if (playerStats.currentState != PlayerStats.PlayerState.Combat)
            return;

        // 1. Check Stamina
        if (playerStats.currentStamina < skill.staminaCost)
        {
            Debug.Log($"Not enough stamina for {skill.skillName}!");
            return;
        }

        // 2. Use Stamina
        playerStats.UseStamina(skill.staminaCost);
        Vector2 punchDirection = playerMovement.LastFacingDirection;

        // 3. Trigger Animation
        if (skill.skillType == SkillData.SkillType.Punch)
        {
            // Sound Effect
            if (punchSwooshSound != null)
            {
                audioSource.PlayOneShot(punchSwooshSound);
            }

                // Animation
            if (punchDirection.y > 0.5f) anim.SetTrigger("PunchUp");
            else if (punchDirection.y < -0.5f) anim.SetTrigger("PunchDown");
            else if (punchDirection.x < -0.5f) anim.SetTrigger("PunchLeft");
            else anim.SetTrigger("PunchRight");
        }
        else if (skill.skillType == SkillData.SkillType.Kick)
        {
            // Implement Kick logic here
        }

        // 4. Calculate Position
        float angle = Mathf.Atan2(punchDirection.y, punchDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 rotatedOffset = rotation * attackOffset;
        Vector2 spawnPosition = (Vector2)transform.position + rotatedOffset;

        // 5. Spawn Effect
        if (slashEffectPrefab != null)
        {
            Instantiate(slashEffectPrefab, spawnPosition, rotation);
        }

        // 6. Detect Hits
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(spawnPosition, punchRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            PrisonerNPC npc = enemy.GetComponent<PrisonerNPC>();
            if (npc != null)
            {   
                // 1. Calculate Damage
                float baseDamage = skill.damage;
                float totalDamage = playerStats.GetTotalDamage(baseDamage);
                npc.TakeDamage(totalDamage);    

                // 2. Apply Knockback to Enemy
                Vector2 knockbackDirection = (npc.transform.position - transform.position).normalized;
                npc.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
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