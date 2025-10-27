using UnityEngine;
using UnityEngine.UI; // To use UI elements like Sliders

public class PlayerStats : MonoBehaviour
{
    // === STATS VARIABLES ===
    [Header("Health Stats")]
    public float currentHealth = 75f; // Set default to 75
    public float maxHealth = 100f;

    [Header("Stamina Stats")]
    public float currentStamina = 75f; // Set default to 75
    public float maxStamina = 100f;

    [Header("Sanity Stats")]
    public float currentSanity = 75f; // Set default to 75
    public float maxSanity = 100f;

    // === UI REFERENCES ===
    [Header("UI Sliders")]
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider sanitySlider;

    // === START FUNCTION ===

    void Start()
    {
        // 1. Initialize the Sliders' MAX values
        healthSlider.maxValue = maxHealth;
        staminaSlider.maxValue = maxStamina;
        sanitySlider.maxValue = maxSanity;

        // 2. Update the Sliders' CURRENT values
        // (This will now read 75 from the variables above)
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
        sanitySlider.value = currentSanity;
    }

    // === PUBLIC UTILITY FUNCTIONS ===

    // --- HEALTH FUNCTIONS ---
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth; // <-- FIXED
        Debug.Log("Health remaining: " + currentHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth; // <-- FIXED
        Debug.Log("Health restored: " + currentHealth);
    }

    // --- STAMINA FUNCTIONS ---
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaSlider.value = currentStamina; // <-- FIXED
        Debug.Log("Stamina remaining: " + currentStamina);
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaSlider.value = currentStamina; // This one was already correct
        Debug.Log("Stamina restored: " + currentStamina);
    }

    // --- SANITY FUNCTIONS ---
    public void LoseSanity(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
        sanitySlider.value = currentSanity; // <-- FIXED
        Debug.Log("Sanity remaining: " + currentSanity);
    }

    public void RestoreSanity(float amount)
    {
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
        sanitySlider.value = currentSanity; // <-- FIXED
        Debug.Log("Sanity restored: " + currentSanity);
    }
}