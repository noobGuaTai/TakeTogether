using Assets.Scripts.Tool.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mirror;
using YooAsset;

public class PropManager : NetworkBehaviour
{
    public Dictionary<string, GameObject> propPrefabs;
    private void Start()
    {
        StartCoroutine(LoadPropPrefabs());
    }

    IEnumerator LoadPropPrefabs()
    {
        propPrefabs = new Dictionary<string, GameObject>();
        List<GameObject> prefabs = new List<GameObject>();
        AssetInfo[] ass = YooAssets.GetAssetInfos("prop");
        foreach (AssetInfo info in ass)
        {
            if(info.AssetPath.Contains(".prefab"))
            {
                AssetHandle ah = YooAssets.LoadAssetAsync<GameObject>(info.AssetPath);
                yield return ah;
                GameObject prefab = ah.AssetObject as GameObject;
                prefabs.Add(prefab);
            }
            
        }

        foreach (GameObject prefab in prefabs)
        {
            if (!propPrefabs.ContainsKey(prefab.name))
            {
                propPrefabs[prefab.name] = prefab;
            }
            else
            {
                Debug.LogWarning("Duplicate effect prefab name found in 'Prefabs/Props': " + prefab.name);
            }
        }
    }

    [Server]
    public void GenProp(string propName, Vector3 propPosition, bool drop = true)
    {
        propName = "Prop" + propName;
        var flag = propPrefabs.ContainsKey(propName);
        if (!flag)
        {
            Debug.LogError("unknown prop name!: " + propName);
            return;
        }
        var obj = propPrefabs[propName];
        var prop = Instantiate(obj);
        prop.transform.position = propPosition;
        prop.transform.localScale = new Vector3(1, 1, 1);
        var propComp = prop.GetComponent<PropBase>();
        propComp.Drop(propPosition);
        NetworkServer.Spawn(prop);
    }
}
