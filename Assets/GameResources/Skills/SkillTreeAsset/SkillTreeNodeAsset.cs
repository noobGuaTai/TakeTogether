using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillTreeNodeAsset : ScriptableObject
{
    public Rect editorPosition = Rect.zero;

    public int skillLevel = 0;
    [SerializeField]
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

    public override int GetHashCode() {
        return _keyName.GetHashCode();
    }

    public override bool Equals(object obj) {
        return this == (SkillTreeNodeAsset)obj;
    }

    public static bool operator ==(SkillTreeNodeAsset a, SkillTreeNodeAsset b) {
        if (ReferenceEquals(a, b)) return true;
        else if (a is null || b is null) return false;
        else return a.keyName == b.keyName;
    }
    public static bool operator !=(SkillTreeNodeAsset a, SkillTreeNodeAsset b) {
        return !(a == b);
    }
}
