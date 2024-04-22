using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeNodeAsset : ScriptableObject
{
    public Rect editorPosition = Rect.zero;

    public int skillLevel = 0;
    private string _keyName = "";
    public List<SkillTreeNodeAsset> inDegreeNodes = new List<SkillTreeNodeAsset>();
    public List<SkillTreeNodeAsset> outDegressNodes = new List<SkillTreeNodeAsset>();

    public SkillTreeAsset treeAsset;

    public string keyName
    {
        get { 
            return _keyName;
        }
        set {
            if (treeAsset != null && treeAsset.nodes.ContainsKey(_keyName))
            {
                treeAsset.nodes.Remove(_keyName);
                _keyName = treeAsset.GetUniqueName(value);
                treeAsset.nodes.Add(_keyName, this);
            }
            else {
                _keyName = value;
            }
        }
    }
    public string typeName
    {
        get { 
            return _keyName.Split(' ')[0];
        }
    }
    
}
