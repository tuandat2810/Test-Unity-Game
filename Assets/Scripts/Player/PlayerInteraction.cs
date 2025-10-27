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
        // 1. TÌM KIẾM: Luôn tìm vật thể ở gần
        currentInteractable = FindNearbyInteractable();

        // 2. HIỂN THỊ UI (nếu tìm thấy)
        if (currentInteractable != null)
        {
            // Cập nhật nội dung text
            string message = currentInteractable.InteractionPrompt;
            promptText.text = $"[F] {message}"; // Ví dụ: "[F] Use Gym"

            // Cập nhật vị trí text
            Vector3 worldPos = transform.position + promptOffset;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
            promptText.transform.position = screenPos;

            // Hiển thị text
            promptText.gameObject.SetActive(true);

            // 3. HÀNH ĐỘNG (nếu nhấn phím)
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentInteractable.Interact(playerStats);
            }
        }
        else
        {
            // 4. ẨN UI (nếu không tìm thấy)
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }

    // Hàm tìm vật thể
    private IInteractable FindNearbyInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        foreach (Collider2D collider in hits)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                return interactable; // Trả về vật thể đầu tiên tìm thấy
            }
        }
        return null; // Không tìm thấy gì
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}