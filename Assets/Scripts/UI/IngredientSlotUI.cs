using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI amountText;

    public void SetData(ItemData item, int count)
    {
        iconImage.sprite = item.icon;
        amountText.text = $"x{count}";
    }
}