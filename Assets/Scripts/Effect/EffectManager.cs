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
        effectPrefabs = Utils.getAllPrefab("Prefabs/Effects");
    }

    public void GenEffect(string effectName, Vector3 propPosition, bool singlePlaye=true)
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
        effectObj.transform.parent = transform;
        effectObj.transform.localScale = new Vector3(1, 1, 1);
        effectComp.singlePlaye = singlePlaye;
    }
}
