using Assets.Scripts.Tool.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mirror;

public class PropManager : NetworkBehaviour
{
    public Dictionary<string, GameObject> propPrefabs;
    private void Awake()
    {
        LoadPropPrefabs();

    }

    void LoadPropPrefabs()
    {
        propPrefabs = Assets.Scripts.Tool.Utils.Utils.getAllPrefab("Prefabs/Props/");
    }

    [Server]
    public void GenProp(string propName, Vector3 propPosition, bool drop = true)
    {
        propName = "Prop" + propName;
        var flag = propPrefabs.ContainsKey(propName);
        if(!flag  ) {
            Debug.LogError("unknown prop name!: " + propName);
            return;
        }
        var obj = propPrefabs[propName];
        var prop = Instantiate(obj);
        prop.transform.position = propPosition;
        prop.transform.localScale = new Vector3(1,1,1);
        var propComp = prop.GetComponent<PropBase>();
        propComp.Drop(propPosition);
        NetworkServer.Spawn(prop);
    }
}
