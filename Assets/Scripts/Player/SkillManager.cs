using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Animator))]
public class SkillManager : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private PlayerStats playerStats;
    private Animator anim;

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

    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();    
        
        skill1Overlay.fillAmount = 0;
        skill1Text.text = "";
        skill2Overlay.fillAmount = 0;
        skill2Text.text = "";
    }

    void Update()
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

        // Skill 3 (Block) - Hold to block
        if (playerStats.currentState == PlayerStats.PlayerState.Combat)
        {
            // 1. When we PRESS the block key
            if (Input.GetKeyDown(blockKey))
            {
                playerStats.SetBlocking(true);
                anim.SetBool("isBlocking", true); // Tell animator to play block anim
            }
            
            // 2. When we RELEASE the block key
            if (Input.GetKeyUp(blockKey))
            {
                playerStats.SetBlocking(false);
                anim.SetBool("isBlocking", false); // Tell animator to stop
            }
        }
        else
        {
            // Failsafe: If we are not in combat, make sure we are not blocking
            if (playerStats.isBlocking)
            {
                playerStats.SetBlocking(false);
                anim.SetBool("isBlocking", false);
            }
        }

        // Handle Cooldowns
        HandleCooldowns();
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