using UnityEngine;
using UnityEngine.UI; // To use UI elements like Sliders

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

    // === UI REFERENCES ===
    [Header("UI Sliders")]
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider sanitySlider;

    // === START FUNCTION ===

    void Start()
    {
        // Set current stats to max on game start
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentSanity = maxSanity;

        // Initialize the Sliders' max values
        // This is important if maxHealth isn't 100
        healthSlider.maxValue = maxHealth;
        staminaSlider.maxValue = maxStamina;
        sanitySlider.maxValue = maxSanity;

        // Update the Sliders' current values
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
        sanitySlider.value = currentSanity;
    }

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
