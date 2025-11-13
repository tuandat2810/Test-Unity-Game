using UnityEngine;

// [System.Serializable] means this class can be
// viewed and edited in the Inspector (inside another script).
// It is NOT a ScriptableObject or a MonoBehaviour.
[System.Serializable]
public class PlayerResponse
{
    [TextArea(2, 5)]
    public string responseText; // The text of the player's response. (e.g., "Yes, I will help you.")

    // Reference to the next DialogueNode that follows this response.
    public DialogueNode nextNode;
}
