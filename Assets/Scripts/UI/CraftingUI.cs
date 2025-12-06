using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUI : MonoBehaviour
{
    [Header("Data")]
    public List<CraftingRecipe> allRecipes; // Drag your recipes here manually

    [Header("UI References")]
    public Transform listContainer; // The RecipeListPanel
    public GameObject slotPrefab;   // The RecipeSlot_Prefab

    [Header("Details Panel")]
    public Image resultIcon;
    public TextMeshProUGUI recipeNameText;
    public Transform ingredientsGrid;
    public GameObject ingredientSlotPrefab;
    public Button craftButton;

    private CraftingRecipe selectedRecipe;
    private CraftingManager craftingManager;

    void Start()
    {
        craftingManager = FindObjectOfType<CraftingManager>();

        // Hide screen on start
        gameObject.SetActive(false);

        // Generate the list
        PopulateRecipeList();

        // Assign Craft Button
        if (craftButton != null)
            craftButton.onClick.AddListener(OnCraftButtonDown);
    }

    void PopulateRecipeList()
    {
        // Clear old slots
        foreach (Transform child in listContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new slots
        foreach (CraftingRecipe recipe in allRecipes)
        {
            GameObject slotGO = Instantiate(slotPrefab, listContainer);
            CraftingSlot slotScript = slotGO.GetComponent<CraftingSlot>();
            slotScript.Setup(recipe, this);
        }
    }

    // Called by CraftingSlot when clicked
    public void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;

        // Update Details Panel
        if (resultIcon != null)
        {
            resultIcon.sprite = recipe.outputItem.icon;
            resultIcon.enabled = true;
        }

        if (recipeNameText != null)
            recipeNameText.text = recipe.recipeName;

        // List ingredients
        foreach (Transform child in ingredientsGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (CraftingRecipe.Ingredient ing in recipe.ingredients)
        {
            GameObject slotGO = Instantiate(ingredientSlotPrefab, ingredientsGrid);

            IngredientSlotUI slotScript = slotGO.GetComponent<IngredientSlotUI>();
            if (slotScript != null)
            {
                slotScript.SetData(ing.item, ing.count);
            }
        }
    }

    void OnCraftButtonDown()
    {
        if (selectedRecipe != null && craftingManager != null)
        {
            craftingManager.Craft(selectedRecipe);
            // (Optional: Update UI or play sound)
        }
    }
}