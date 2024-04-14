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
    private void Awake()
    {
        StartCoroutine(LoadPropPrefabs());

    }

    IEnumerator LoadPropPrefabs()
    {
        propPrefabs = new Dictionary<string, GameObject>();
        // GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("Prefabs/Props/");
        List<GameObject> prefabs = new List<GameObject>();
        AssetHandle propBaseHandle = YooAssets.LoadAssetAsync<GameObject>("Assets/GameResources/Enemy/Prop/CoinPrefab/PropBase.prefab");
        AssetHandle PropCoin_1Handle = YooAssets.LoadAssetAsync<GameObject>("Assets/GameResources/Enemy/Prop/CoinPrefab/Coin/PropCoin_1.prefab");
        AssetHandle PropCoin_5Handle = YooAssets.LoadAssetAsync<GameObject>("Assets/GameResources/Enemy/Prop/CoinPrefab/Coin/PropCoin_5.prefab");

        yield return propBaseHandle;
        yield return PropCoin_1Handle;
        yield return PropCoin_5Handle;

        GameObject propBasePrefab = propBaseHandle.AssetObject as GameObject;
        prefabs.Add(propBasePrefab);
        GameObject propCoin_1Prefab = PropCoin_1Handle.AssetObject as GameObject;
        prefabs.Add(propCoin_1Prefab);
        GameObject propCoin_5Prefab = PropCoin_5Handle.AssetObject as GameObject;
        prefabs.Add(propCoin_5Prefab);


        foreach (GameObject prefab in prefabs)
        {
            if (!propPrefabs.ContainsKey(prefab.name))
            {
                propPrefabs[prefab.name] = prefab;
                print(prefab.name);
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
        propName = "Prop" + propName + ".prefab";
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
