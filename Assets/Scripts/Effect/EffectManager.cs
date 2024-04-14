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
        // GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("Prefabs/Effects");
        List<GameObject> prefabs = new List<GameObject>();
        AssetHandle effectBaseHandle = YooAssets.LoadAssetAsync<GameObject>("Assets/GameResources/Effect/Effects/EffectBase.prefab");
        AssetHandle effectCollect = YooAssets.LoadAssetAsync<GameObject>("Assets/GameResources/Effect/Effects/EffectCollect.prefab");
        AssetHandle effectHitSword_1Handle = YooAssets.LoadAssetAsync<GameObject>("Assets/GameResources/Effect/Effects/Hit/EffectHitSword_1.prefab");
        yield return effectBaseHandle;
        yield return effectCollect;
        yield return effectHitSword_1Handle;

        GameObject effectBasePrefab = effectBaseHandle.AssetObject as GameObject;
        prefabs.Add(effectBasePrefab);
        GameObject effectCollectPrefab = effectCollect.AssetObject as GameObject;
        prefabs.Add(effectCollectPrefab);
        GameObject effectHitSword_1Prefab = effectHitSword_1Handle.AssetObject as GameObject;
        prefabs.Add(effectHitSword_1Prefab);

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
