using UnityEngine;

public class LibraryZone : MonoBehaviour, IInteractable
{
    public float sanityToRestore = 10f;

    [SerializeField] private string promptMessage = "Read Book";

    public string InteractionPrompt => promptMessage;

    public void Interact(PlayerStats player)
    {
        if (player != null)
        {
            // Call the RestoreSanity function from PlayerStats
            player.RestoreSanity(sanityToRestore);

            Debug.Log("Player read a book. Sanity restored.");
        }
    }
}