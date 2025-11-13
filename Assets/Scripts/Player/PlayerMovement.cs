using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // player speed
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 dominantInput;

    private Animator anim;
    private PlayerStats playerStats;

    public Vector2 LastFacingDirection { get; private set; }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();  

        rb.gravityScale = 0; // turnoff gravity

        dominantInput = new Vector2(1, 0); // Default facing right
        LastFacingDirection = dominantInput;
    }

    void Update()
    {
        // If we are blocking, stop all movement input
        if (playerStats.isBlocking)
        {
            moveInput = Vector2.zero;
            anim.SetFloat("Speed", 0); // Stop run animation
            return; // Skip the rest of the Update
        }

        // --- MOVEMENT INPUT ---
        float rawX = Input.GetAxisRaw("Horizontal");
        float rawY = Input.GetAxisRaw("Vertical");

        // --- ANIMATOR LOGIC IS ALWAYS ON ---
        moveInput = new Vector2(rawX, rawY).normalized;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            // Prioritize horizontal or vertical animation
            if (Mathf.Abs(rawX) > Mathf.Abs(rawY))
            {
                dominantInput = new Vector2(rawX, 0);
            }
            else
            {
                dominantInput = new Vector2(0, rawY);
            }

            LastFacingDirection = dominantInput.normalized;
        }

        anim.SetFloat("Speed", moveInput.sqrMagnitude);

        anim.SetFloat("MoveX", dominantInput.x);
        anim.SetFloat("MoveY", dominantInput.y);
    }

    void FixedUpdate()
    { // Move player
        rb.linearVelocity = moveInput * moveSpeed;
    }
}

