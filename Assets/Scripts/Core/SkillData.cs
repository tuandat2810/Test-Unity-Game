using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Combat/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public float cooldownTime = 1f;
    public float staminaCost = 10f;
    public float damage = 5f;
    
    public enum SkillType { Punch, Kick, Block, Dodge }
    public SkillType skillType;

    // Additional skill properties can be added here
}


