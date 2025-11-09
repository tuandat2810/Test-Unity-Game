using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStatsUI : MonoBehaviour
{
    [Header("Data Sources (Assign Player)")]
    public PlayerStats playerStats;
    public EquipmentManager equipmentManager;

    [Header("Stat Text Fields (Right Panel)")]
    public TextMeshProUGUI healthValueText;
    public TextMeshProUGUI staminaValueText;
    public TextMeshProUGUI sanityValueText;
    public TextMeshProUGUI damageValueText; // For total damage
    // (Add more texts as needed, e.g., "PlayerStats_TitleText")

    [Header("Equipment Slots (Left Panel)")]
    public Image weaponSlotIcon;
    public Image weaponPlaceholder;

    public Image helmetSlotIcon;
    public Image helmetPlaceholder;

    public Image chestSlotIcon;
    public Image chestPlaceholder;

    public Image bootsSlotIcon;
    public Image bootsPlaceholder;
    // (Add more slots as needed, e.g., "helmetSlotIcon")

    // OnEnable is called automatically every time
    // this GameObject (CharacterScreen) is set to Active.
    void OnEnable()
    {
        // Fail-safe check
        if (playerStats == null || equipmentManager == null)
        {
            Debug.LogError("CharacterStatsUI is missing references to Player!");
            return;
        }
        
        // Update all UI elements
        UpdateStatsDisplay();
        UpdateEquipmentDisplay();
    }

    public void UpdateStatsDisplay()
    {
        healthValueText.text = $"{playerStats.currentHealth.ToString("F0")} / {playerStats.maxHealth.ToString("F0")}";
        staminaValueText.text = $"{playerStats.currentStamina.ToString("F0")} / {playerStats.maxStamina.ToString("F0")}";
        sanityValueText.text = $"{playerStats.currentSanity.ToString("F0")} / {playerStats.maxSanity.ToString("F0")}";
        
        float baseDamage = 5f; // Base damage can be defined or fetched from PlayerStats
        damageValueText.text = playerStats.GetTotalDamage(baseDamage).ToString("F0");
        
        // (Update more stats as needed)
    }

    // This function updates all the slot icons
    public void UpdateEquipmentDisplay()
    {
        // Get the equipped weapon
        ItemData weapon = equipmentManager.GetEquippedItem(ItemData.EquipmentSlot.Weapon);
        if (weapon != null)
        {
            weaponSlotIcon.sprite = weapon.icon;
            weaponSlotIcon.enabled = true; // Show the icon
            weaponPlaceholder.enabled = false; // Hide placeholder
        }
        else
        {
            weaponSlotIcon.enabled = false; // Hide if no weapon is equipped
            weaponPlaceholder.enabled = true; // Show placeholder
        }

        // Get the equipped chest armor
        ItemData chest = equipmentManager.GetEquippedItem(ItemData.EquipmentSlot.Chest);
        if (chest != null)
        {
            chestSlotIcon.sprite = chest.icon;
            chestSlotIcon.enabled = true;
            chestPlaceholder.enabled = false; // Hide placeholder
        }
        else
        {
            chestSlotIcon.enabled = false;
            chestPlaceholder.enabled = true; // Show placeholder
        }

        // Get the equipped helmet
        ItemData helmet = equipmentManager.GetEquippedItem(ItemData.EquipmentSlot.Helmet);
        if (helmet != null)
        {
            helmetSlotIcon.sprite = helmet.icon;
            helmetSlotIcon.enabled = true;
            helmetPlaceholder.enabled = false; // Hide placeholder
        }
        else
        {
            helmetSlotIcon.enabled = false;
            helmetPlaceholder.enabled = true; // Show placeholder
        }

        // Get the equipped boots
        ItemData boots = equipmentManager.GetEquippedItem(ItemData.EquipmentSlot.Boots);
        if (boots != null)
        {
            bootsSlotIcon.sprite = boots.icon;
            bootsSlotIcon.enabled = true;
            bootsPlaceholder.enabled = false; // Hide placeholder
        }
        else
        {
            bootsSlotIcon.enabled = false;
            bootsPlaceholder.enabled = true; // Show placeholder
        }

        
        // (Repeat for Helmet, Boots, etc.)
    }
}
