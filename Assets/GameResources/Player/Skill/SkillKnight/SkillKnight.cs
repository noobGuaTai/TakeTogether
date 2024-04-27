using Skill;
using UnityEngine;

public class SkillFireDush : Skill.SkillBase
{
    public override void OnUnlock()
    {
        Debug.Log("!!!!!SkillFireDush unlock!!!!!");
    }

    public static SkillFireDush Instance() { 
        return new SkillFireDush();
    }
}