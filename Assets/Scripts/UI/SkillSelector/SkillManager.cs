using Skill;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YooAsset;

public class SkillManager : MonoBehaviour
{
    public Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    public Dictionary<string, Sprite> skillIcons = new Dictionary<string, Sprite>();

    public IEnumerator LoadSkillIcons() {
        AssetInfo[] ass = YooAssets.GetAssetInfos("skillIcon");
        foreach (AssetInfo info in ass) {
            var ah = YooAssets.LoadSubAssetsAsync<Sprite>(info.AssetPath);
            if (ah == null)
                continue;
            yield return ah;
            Sprite icon = ah.GetSubAssetObjects<Sprite>()[0];
            if (icon == null)
                continue;
            skillIcons.Add(icon.name.Substring(9), icon);
        
        }
    }

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

        StartCoroutine(LoadSkillIcons());
    }
}
