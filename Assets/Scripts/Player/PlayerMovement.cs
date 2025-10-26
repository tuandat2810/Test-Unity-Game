using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // player speed
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 dominantInput;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; // turnoff gravity
        }

        dominantInput = new Vector2(0, -1);
    }

    void Update()
    {
        float rawX = Input.GetAxisRaw("Horizontal");
        float rawY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(rawX, rawY).normalized;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            // W + D -> go D
            if (Mathf.Abs(rawX) > Mathf.Abs(rawY))
            {
                dominantInput = new Vector2(rawX, 0);
            }
            // 
            else
            {
                dominantInput = new Vector2(0, rawY);
            }
        }

        anim.SetFloat("Speed", moveInput.sqrMagnitude);

        anim.SetFloat("MoveX", dominantInput.x);
        anim.SetFloat("MoveY", dominantInput.y);
    }

    void FixedUpdate()
    { // Move player
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime); }
    }

