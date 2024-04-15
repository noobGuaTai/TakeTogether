using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

public class EffectManager : MonoBehaviour
{
    public Dictionary<string, GameObject> effectPrefabs;
    private void Awake()
    {
        StartCoroutine(LoadEffectPrefabs());
    }

    IEnumerator LoadEffectPrefabs()
    {
        effectPrefabs = new Dictionary<string, GameObject>();
        List<GameObject> prefabs = new List<GameObject>();

        AssetInfo[] ass = YooAssets.GetAssetInfos("effect");
        foreach (AssetInfo info in ass)
        {
            if (info.AssetPath.Contains(".prefab"))
            {
                AssetHandle ah = YooAssets.LoadAssetAsync<GameObject>(info.AssetPath);
                yield return ah;
                GameObject prefab = ah.AssetObject as GameObject;
                prefabs.Add(prefab);
            }
        }


        foreach (GameObject prefab in prefabs)
        {
            if (!effectPrefabs.ContainsKey(prefab.name))
            {
                effectPrefabs[prefab.name] = prefab;
            }
            else
            {
                Debug.LogWarning("Duplicate effect prefab name found in 'Prefabs/Effects': " + prefab.name);
            }
        }
    }

    public void GenEffect(string effectName, Vector3 propPosition, bool singlePlaye = true)
    {
        effectName = "Effect" + effectName;
        var flag = effectPrefabs.ContainsKey(effectName);
        if (!flag)
        {
            Debug.LogError("unknown prop name!");
            return;
        }
        var obj = effectPrefabs[effectName];
        var effectObj = Instantiate(obj);
        var effectComp = effectObj.GetComponent<EffectBase>();
        effectObj.transform.position = propPosition;
        var oldLocalScale = effectObj.transform.localScale;

        effectObj.transform.parent = transform;
        effectObj.transform.Rotate(0, 0, Random.Range(0, 360));
        effectObj.transform.localScale = oldLocalScale;
        effectComp.singlePlaye = singlePlaye;
    }
}
