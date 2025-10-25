using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // === STATS VARIABLES ===
    // Using [Header] to organize the Inspector

    [Header("Health Stats")]
    public float currentHealth;
    public float maxHealth = 100f;

    [Header("Stamina Stats")]
    public float currentStamina;
    public float maxStamina = 100f;

    [Header("Sanity Stats")]
    public float currentSanity;
    public float maxSanity = 100f;


    // === PUBLIC UTILITY FUNCTIONS ===
    // Other scripts (like Gym, Combat) will call these functions

    // --- HEALTH FUNCTIONS ---
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        // Ensure health never drops below 0
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log("Health remaining: " + currentHealth); // Log to console for testing
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        // Ensure health never exceeds the max value
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log("Health restored: " + currentHealth);
    }

    // --- STAMINA FUNCTIONS ---
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        Debug.Log("Stamina remaining: " + currentStamina);
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        Debug.Log("Stamina restored: " + currentStamina);
    }

    // --- SANITY FUNCTIONS ---
    public void LoseSanity(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
        Debug.Log("Sanity remaining: " + currentSanity);
    }

    public void RestoreSanity(float amount)
    {
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
        Debug.Log("Sanity restored: " + currentSanity);
    }
}
