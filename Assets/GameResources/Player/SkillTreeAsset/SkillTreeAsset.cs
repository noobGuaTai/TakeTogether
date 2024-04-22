using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class SkillTreeAsset : ScriptableObject
{
    public enum NodeType { 
        SKILL,
        ROOT
    }

    private Dictionary<string,SkillTreeNodeAsset> _nodes = new Dictionary<string, SkillTreeNodeAsset>();
    public Dictionary<string, bool> rootNode = new Dictionary<string, bool>();

    [DoNotSerialize]
    private bool hasInit = false;

    public Dictionary<string, SkillTreeNodeAsset> nodes
    {
        get {
            init();
            return _nodes;
        }
    }

    private void OnEnable()
    {
        init(true);
    }

    void init(bool force = false) {
        if (hasInit && !force) { return; }
        hasInit = true;
        var path = AssetDatabase.GetAssetPath(this);
        var objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
        foreach (var obj in objects)
        {
            var skillTreeNodeAsset = obj as SkillTreeNodeAsset;
            if (skillTreeNodeAsset != null)
            {
                if (nodes.ContainsKey(skillTreeNodeAsset.keyName)) {
                    AssetDatabase.RemoveObjectFromAsset(skillTreeNodeAsset);
                    AssetDatabase.SaveAssets();
                    continue;
                }
                nodes.Add(skillTreeNodeAsset.keyName, skillTreeNodeAsset);
                if(skillTreeNodeAsset.typeName == "#ROOT")
                    rootNode.Add(skillTreeNodeAsset.keyName, skillTreeNodeAsset);
            }
            
        }
        
    }

    public string GetUniqueName(String name)
    {
        var samenameCount = 1;
        var modifiedSkillName = name + $" - ({samenameCount})";
        while (nodes.ContainsKey(modifiedSkillName))
        {
            samenameCount++;
            modifiedSkillName = name + $" - ({samenameCount})";
        }
        return modifiedSkillName;
    }

    public SkillTreeNodeAsset AddNode(string skillName = "[Undefined]", NodeType nodeType=NodeType.SKILL) {
        var node = ScriptableObject.CreateInstance<SkillTreeNodeAsset>();

        var modifiedSkillName = GetUniqueName(skillName);
        node.treeAsset = this;
        node.keyName = modifiedSkillName;
        node.name = modifiedSkillName;
        
        nodes.Add(modifiedSkillName, node);
        if (nodeType == NodeType.ROOT)
            rootNode.Add(modifiedSkillName, node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void RemoveNode(SkillTreeNodeAsset node) {
        nodes.Remove(node.keyName);

        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void changeNodeSkillName(SkillTreeNodeAsset node, string newSkillName) {
        RemoveNode(node);
        var newNode = AddNode(newSkillName);
        newNode.editorPosition = node.editorPosition;
    }
}
