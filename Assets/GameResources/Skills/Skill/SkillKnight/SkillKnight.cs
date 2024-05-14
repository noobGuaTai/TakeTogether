using Skill;
using UnityEngine;

public class SkillBladeRush : Skill.SkillBase
{
    public override void OnUnlock()
    {
        Debug.Log("!!!!!SkillFireDush unlock!!!!!");
    }

    public static SkillBladeRush Instance() { 
        return new SkillBladeRush();
    }
}

public class SkillBladeRushLighting : Skill.SkillBase
{
    public override void OnUnlock() {
        Debug.Log("!!!!!SkillFireDush unlock!!!!!");
    }

    public static SkillBladeRushLighting Instance() {
        return new SkillBladeRushLighting();
    }
}

public class SkillBladeRushLightingAmount : Skill.SkillBase
{
    public override void OnUnlock() {
        Debug.Log("!!!!!SkillFireDush unlock!!!!!");
    }

    public static SkillBladeRushLightingAmount Instance() {
        return new SkillBladeRushLightingAmount();
    }
}

public class SkillStrength : Skill.SkillBase
{
    public override void OnUnlock() {
        Debug.Log("!!!!!SkillFireDush unlock!!!!!");
    }

    public static SkillStrength Instance() {
        return new SkillStrength();
    }
}