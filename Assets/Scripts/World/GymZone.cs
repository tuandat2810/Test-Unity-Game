using UnityEngine;

public class GymZone : MonoBehaviour, IInteractable
{
    public float staminaToRestore = 10f;

    [SerializeField] private string promptMessage = "Use Gym";

    public string InteractionPrompt => promptMessage;

    // The function signature now matches the interface
    public void Interact(PlayerStats player) // <-- Receives player stats directly
    {
        Debug.Log("Interacted with Gym!");

        // No need to FindObjectOfType anymore!
        if (player != null)
        {
            player.RestoreStamina(staminaToRestore);
        }
    }
}