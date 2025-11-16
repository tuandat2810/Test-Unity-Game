using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Animator))]    
[RequireComponent(typeof(PlayerMovement))] 
[RequireComponent(typeof(SpriteRenderer))]
public class SkillManager : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private PlayerStats playerStats;
    private Animator anim;
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    [Header("Skill 1 (Punch)")]
    public SkillData skill1Data;
    public KeyCode skill1Key = KeyCode.J;
    public Image skill1Overlay;
    public TextMeshProUGUI skill1Text;
    private float skill1Timer = 0f; 

    [Header("Skill 2 (Kick)")]
    public SkillData skill2Data;
    public KeyCode skill2Key = KeyCode.K;
    public Image skill2Overlay;
    public TextMeshProUGUI skill2Text;
    private float skill2Timer = 0f;

    [Header("Skill 3 (Block)")]
    public KeyCode blockKey = KeyCode.Mouse1;
    public GameObject blockHighlight;

    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();    
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        skill1Overlay.fillAmount = 0;
        skill1Text.text = "";
        skill2Overlay.fillAmount = 0;
        skill2Text.text = "";

        // Ensure highlight is off at the start
        if (blockHighlight != null)
            blockHighlight.SetActive(false);
    }

    void Update()
    {
        HandleSkillInputs(); // Handle J and K keys
        HandleBlockInput();  // Handle Right Mouse Button
        HandleCooldowns();   // Update the UI
    }

    private void HandleSkillInputs()
    {
        // Skill 1 (J)
        if (Input.GetKeyDown(skill1Key) && skill1Timer <= 0)
        {
            playerCombat.PerformSkill(skill1Data);
            skill1Timer = skill1Data.cooldownTime; 
        }

        // Skill 2 (K)
        if (Input.GetKeyDown(skill2Key) && skill2Timer <= 0)
        {
            playerCombat.PerformSkill(skill2Data);
            skill2Timer = skill2Data.cooldownTime;
        }
    }

    private void HandleBlockInput()
    {
        // Logic for holding the block key
        if (playerStats.currentState == PlayerStats.PlayerState.Combat)
        {
            // 1. When we PRESS the block key
            if (Input.GetKeyDown(blockKey))
            {
                playerStats.SetBlocking(true);
                anim.SetBool("isBlocking", true);

                if (blockHighlight != null)
                    blockHighlight.SetActive(true); 

                // Set flip direction
                Vector2 direction = playerMovement.LastFacingDirection;
                if (direction.x < -0.1f)
                    spriteRenderer.flipX = true; // Facing Left
                else if (direction.x > 0.1f)
                    spriteRenderer.flipX = false; // Facing Right
            }
            
            // 2. When we RELEASE the block key
            if (Input.GetKeyUp(blockKey))
            {
                playerStats.SetBlocking(false);
                anim.SetBool("isBlocking", false);
                spriteRenderer.flipX = false; // Reset flip

                if (blockHighlight != null)
                    blockHighlight.SetActive(false);
            }
        }
        else
        {
            // Failsafe: If we are not in combat, make sure we are not blocking
            if (playerStats.isBlocking)
            {
                playerStats.SetBlocking(false);
                anim.SetBool("isBlocking", false);
                spriteRenderer.flipX = false;
            }

            if (blockHighlight != null)
                    blockHighlight.SetActive(false);
        }
    }

    private void HandleCooldowns()
    {
        // Skill 1
        if (skill1Timer > 0)
        {
            // Cooling down
            skill1Timer -= Time.deltaTime;
            skill1Overlay.fillAmount = skill1Timer / skill1Data.cooldownTime;
            skill1Text.text = skill1Timer.ToString("F1"); // "2.5"
        }
        else
        {
            // Cooldown complete
            skill1Overlay.fillAmount = 0;
            skill1Text.text = "";
        }

        // Skill 2
        if (skill2Timer > 0)
        {
            skill2Timer -= Time.deltaTime;
            skill2Overlay.fillAmount = skill2Timer / skill2Data.cooldownTime;
            skill2Text.text = skill2Timer.ToString("F1");
        }
        else
        {
            skill2Overlay.fillAmount = 0;
            skill2Text.text = "";
        }
    }
}