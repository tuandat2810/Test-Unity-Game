using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    // This function will be called by the animation
    public void DestroySelf()
    {
        // This is "bad" (slow). Later we will use an Object Pool.
        Destroy(gameObject);
    }

    // This function is called by the NPC
    public void SetText(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
    }
}
