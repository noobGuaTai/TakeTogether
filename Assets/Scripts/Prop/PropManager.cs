using Assets.Scripts.Tool.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public Dictionary<string, GameObject> propPrefabs;
    private void Awake()
    {
        LoadPropPrefabs();

    }

    void LoadPropPrefabs()
    {
        propPrefabs = Utils.getAllPrefab("Prefabs/Props/");
    }

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
        prop.transform.parent = transform;
        prop.transform.localScale = new Vector3(1,1,1);
        var propComp = prop.GetComponent<PropBase>();
        propComp.Drop(propPosition);
    }
}
