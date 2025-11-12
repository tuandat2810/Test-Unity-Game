using UnityEngine;
using UnityEngine.UI; // To use UI elements like Sliders
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    // === STATS VARIABLES ===
    [Header("Health Stats")]
    public float currentHealth = 75f;
    public float maxHealth = 100f;

    [Header("Stamina Stats")]
    public float currentStamina = 75f;
    public float maxStamina = 100f;

    [Header("Sanity Stats")]
    public float currentSanity = 75f;
    public float maxSanity = 100f;

    // bonus stats from equipment
    public int bonusHealth = 0;
    public int bonusDamage = 0; // Player combat will ask for this value

    // === UI REFERENCES ===
    [Header("UI Sliders")]
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider sanitySlider; 

    // === PLAYER STATE ===
    // This script now owns the player's state
    public enum PlayerState
    {
        Overworld,  // exploration status
        Combat      // fighting status
    }
    public PlayerState currentState;

    // KNOCKBACK VARIABLES
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private Coroutine knockbackCoroutine;


    // === START FUNCTION ===
    void Start()
    {
        // 1. Initialize Sliders' MAX values
        healthSlider.maxValue = maxHealth;
        staminaSlider.maxValue = maxStamina;
        sanitySlider.maxValue = maxSanity;

        // 2. Update Sliders' CURRENT values
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
        sanitySlider.value = currentSanity;


        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();

        // Start the game in Overworld state
        currentState = PlayerState.Overworld;
    }

    // === STATE CHANGING FUNCTIONS (NEW) ===
    // Functions to change the state are now here
    public void EnterCombatState()
    {
        currentState = PlayerState.Combat;
        Debug.Log("State changed to: COMBAT");
    }

    public void ExitCombatState()
    {
        currentState = PlayerState.Overworld;
        Debug.Log("State changed to: OVERWORLD");
    }


    // Called by EquipmentManager when we equip/unequip
    public void UpdateEquipmentStats(int healthBonus, int damageBonus)
    {
        bonusHealth = healthBonus;
        bonusDamage = damageBonus;
        
        // Update the max health immediately
        maxHealth = 100 + bonusHealth; // (Assuming 100 is base health)
        healthSlider.maxValue = maxHealth;
        
        // If current health is over max, clamp it
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // PlayerCombat will call this to calculate damage
    public float GetTotalDamage(float baseDamage)
    {
        return baseDamage + bonusDamage;
    }


    // === PUBLIC UTILITY FUNCTIONS ===

    // --- HEALTH FUNCTIONS ---
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;
        Debug.Log("Health remaining: " + currentHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;
        Debug.Log("Health restored: " + currentHealth);
    }

    // --- STAMINA FUNCTIONS ---
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaSlider.value = currentStamina;
        Debug.Log("Stamina remaining: " + currentStamina);
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaSlider.value = currentStamina;
        Debug.Log("Stamina restored: " + currentStamina);
    }

    // --- SANITY FUNCTIONS ---
    public void LoseSanity(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
        sanitySlider.value = currentSanity;
        Debug.Log("Sanity remaining: " + currentSanity);
    }

    public void RestoreSanity(float amount)
    {
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
        sanitySlider.value = currentSanity;
        Debug.Log("Sanity restored: " + currentSanity);
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        // If a knockback is already happening, stop it
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }
        // Start a new knockback coroutine
        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }   

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        // 1. Disable player's movement script (stun)
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }   

        // 2. Apply knockback force
        rb.linearVelocity = Vector2.zero; // Reset current velocity
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        // 3. Wait for the duration
        yield return new WaitForSeconds(duration);

        // 4. Re-enable player's movement script
        // We DELETE "rb.velocity = Vector2.zero;" to prevent getting stuck
        if (playerMovement != null)
            playerMovement.enabled = true;
        
        knockbackCoroutine = null;
    }
} 