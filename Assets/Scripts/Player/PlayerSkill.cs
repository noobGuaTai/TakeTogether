using Skill;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

class PlayerSkill : MonoBehaviour {
    public SkillTreeAsset skillTreeAsset;
    // only save locked skill
    public Dictionary<string, int> skillUnlockCounter = new Dictionary<string, int>();
    // only save can unlock skill
    public Dictionary<string, bool> canUnlockSkill = new Dictionary<string, bool>();
    
    public Dictionary<string, Dictionary<string, bool>> skillOut = new Dictionary<string, Dictionary<string, bool>>();
    public Dictionary<string, Dictionary<string, bool>> skillIn = new Dictionary<string, Dictionary<string, bool>>();

    void loadSkillTree()
    {
        if (skillTreeAsset == null)
        {
            Debug.LogError("PlayerSkill try to load null skill tree");
            return;
        }
        skillOut.Clear();
        skillIn.Clear();
        skillUnlockCounter.Clear();
        canUnlockSkill.Clear();


        Queue<string> queue = new Queue<string>();
        foreach(var rootKeyName in skillTreeAsset.rootNode.Keys)
            queue.Enqueue(rootKeyName);
        while (queue.Count > 0)
        {
            var root = queue.First();
            var rootNodeAsset = skillTreeAsset.nodes[root];
            queue.Dequeue();

            foreach (var initSkill in rootNodeAsset.outDegressNodes)
            {
                canUnlockSkill.Add(initSkill.typeName, true);
            }
        }

        // calculate skillOut & skillIn
        foreach (var skillPair in skillTreeAsset.nodes)
        {
            var keyName = skillPair.Key;
            var skillAsset = skillPair.Value;
            if (skillTreeAsset.rootNode.ContainsKey(keyName))
                continue;
            // in
            var skillName = skillAsset.typeName;
            skillIn.Add(skillName, new Dictionary<string, bool>());
            foreach (var nodeAssetIn in skillAsset.inDegreeNodes) 
                skillIn[skillName].Add(nodeAssetIn.typeName, true);
            // out
            skillOut.Add(skillName, new Dictionary<string, bool>());
            foreach (var nodeAssetOut in skillAsset.outDegressNodes)
                skillOut[skillName].Add(nodeAssetOut.typeName, true);
        }

        // calculate unlock counter
        foreach (var skillPair in skillIn)
        {
            if (canUnlockSkill.ContainsKey(skillPair.Key))
                continue;
            skillUnlockCounter.Add(skillPair.Key, skillPair.Value.Count);
        }
    }

    void Unlock(string skillName) {
        if (!canUnlockSkill.ContainsKey(skillName)) {
            Debug.LogWarning($"PlayerSkill: trying to unlock locked skill{skillName}");
            return;
        }

        // call OnUnlock function

        var skillManager = transform.Find("/SkillManager").GetComponent<SkillManager>();
        var skillBaseInstance = skillManager.skills[skillName];
        skillBaseInstance.OnUnlock();


        // update unlock counter and add new can unlock skills
        foreach (var outSkill in skillOut[skillName].Keys)
        {
            skillUnlockCounter[outSkill]--;
            if(skillUnlockCounter[outSkill] == 0)
            {
                skillUnlockCounter.Remove(outSkill);
                canUnlockSkill.Add(outSkill, true);
            }
        }

    }

    private void Awake()
    {
        loadSkillTree();
    }

    void test() { 
        foreach(var skill in canUnlockSkill) {
            Debug.Log(skill);
            Unlock(skill.Key);
        }
    }

    private void Start()
    {
        test();
    }
}