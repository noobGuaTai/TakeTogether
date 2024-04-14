using Assets.Scripts.Tool.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Dictionary<string, GameObject> effectPrefabs;
    private void Awake()
    {
        LoadEffectPrefabs();
    }

    void LoadEffectPrefabs()
    {
        // 这个方法在使用IL2cpp构建项目时，会出错
        // effectPrefabs = Utils.getAllPrefab("Prefabs/Effects");

        effectPrefabs = new Dictionary<string, GameObject>();
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("Prefabs/Effects");
        foreach (GameObject prefab in loadedPrefabs)
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
