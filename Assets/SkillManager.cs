using Skill;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    private void Awake()
    {
        skills.Clear();
        var types = TypeCache.GetTypesDerivedFrom<SkillBase>().ToList();
        foreach (var type in types) {
            var instance = (SkillBase)type.InvokeMember(
                "Instance", System.Reflection.BindingFlags.InvokeMethod, null, null, null);
            var skillName = type.Name.Substring(5);
            skills.Add(skillName, instance);
        }
        DontDestroyOnLoad(gameObject);
    }
}
