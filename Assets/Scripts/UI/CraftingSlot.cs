using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSlot : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    private CraftingRecipe myRecipe;
    private CraftingUI uiManager; // Reference back to the main UI

    // Setup function called when created
    public void Setup(CraftingRecipe recipe, CraftingUI ui)
    {
        myRecipe = recipe;
        uiManager = ui;

        if (recipe != null)
        {
            iconImage.sprite = recipe.outputItem.icon; // Use result icon
            nameText.text = recipe.recipeName;
        }
    }

    // Assign this to the Button's OnClick in the Prefab
    public void OnClick()
    {
        if (uiManager != null)
        {
            uiManager.SelectRecipe(myRecipe);
        }
    }
}