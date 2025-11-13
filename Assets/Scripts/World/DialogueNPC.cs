using UnityEngine;

// This NPC's only job is to talk.
// It implements IInteractable so PlayerInteraction can find it.
public class DialogueNPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    // Assign the starting node of your conversation here
    public DialogueNode startNode; 

    // This is the message for the [F] prompt (e.g., "[F] Talk")
    [SerializeField] private string promptMessage = "Talk";

    // --- IInteractable Implementation ---

    // This property sends the prompt text to PlayerInteraction
    public string InteractionPrompt => promptMessage;

    // This function is called by PlayerInteraction when 'F' is pressed
    public void Interact(PlayerStats player)
    {
        // Find the singleton DialogueManager and tell it to start
        if (DialogueManager.Instance != null && startNode != null)
        {
            DialogueManager.Instance.StartDialogue(startNode);
        }
    }
}