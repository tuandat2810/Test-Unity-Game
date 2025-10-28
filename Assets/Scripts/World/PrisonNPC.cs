using UnityEngine;

public class PrisonNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "Rival Prisoner";

    public string InteractionPrompt => $"Challenge {npcName}";

    public void Interact(PlayerStats player)
    {
        // Check if player is not null
        if (player != null)
        {
            // === THIS IS THE MISSING LINE ===
            // Tell the PlayerStats script to change the state
            player.EnterCombatState();
        }

        // debug log only at this moment
        Debug.Log($"Player challenged {npcName}!");
    }
}
