using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRadius = 1.0f;

    // Cache our own PlayerStats component
    private PlayerStats playerStats;

    void Start()
    {
        // Get our own stats at the beginning
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

            foreach (Collider2D collider in hits)
            {
                IInteractable interactable = collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    // Pass our stats component (playerStats) to the object
                    interactable.Interact(playerStats);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}