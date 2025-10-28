using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRadius = 1.0f;

    // Cache our own PlayerStats component
    private PlayerStats playerStats;

    [Header("Interaction Prompt UI")]
    public TextMeshProUGUI promptText;
    public Vector3 promptOffset = new Vector3(1f, 0.5f, 0); 

    private Camera mainCamera; 
    private IInteractable currentInteractable; 

    void Start()
    {
        // Get our own stats at the beginning
        playerStats = GetComponent<PlayerStats>();
        mainCamera = Camera.main; 
    
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        // === NEW STATE CHECK ===
        // Check the state from PlayerStats.cs
        if (playerStats.currentState == PlayerStats.PlayerState.Combat)
        {
            // If we are in COMBAT, do nothing. Hide the UI and stop.
            if (promptText != null && promptText.gameObject.activeSelf)
            {
                promptText.gameObject.SetActive(false);
            }
            currentInteractable = null; // Clear any target
            return; // Stop the Update() function here
        }

        // --- ALL LOGIC BELOW ONLY RUNS IF STATE IS "OVERWORLD" ---
        // 1. Find: Interactable object Around
        currentInteractable = FindNearbyInteractable();

        // 2. Show if found Interactable object
        if (currentInteractable != null)
        {
            // Update text
            string message = currentInteractable.InteractionPrompt;
            promptText.text = $"[F] {message}"; // Ex: "[F] Use Gym"

            // Text position
            Vector3 worldPos = transform.position + promptOffset;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
            promptText.transform.position = screenPos;

            // Show text
            promptText.gameObject.SetActive(true);

            // 3. Interact if user press F
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentInteractable.Interact(playerStats);
            }
        }
        else
        {
            // 4. HIde if found nothing
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }

    // Found interactable obj func
    private IInteractable FindNearbyInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        foreach (Collider2D collider in hits)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                return interactable; // Return First Object Interactable found
            }
        }
        return null; // Found nothing Interactable
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}