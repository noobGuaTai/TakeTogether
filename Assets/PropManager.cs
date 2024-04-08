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
        propPrefabs = new Dictionary<string, GameObject>();
        var folderPath = "Assets/Resources/Prefabs/Props";
        var resPrefix = "Prefabs/Props/";
        string[] files =
            Directory.GetFiles(folderPath, "*.prefab");
        foreach (var filePath in files)
        {
            string propName = Path.GetFileNameWithoutExtension(filePath);
            GameObject prefab = Resources.Load<GameObject>(
                resPrefix + propName);
            propPrefabs[propName] = prefab;
        }
    }

    public void GenProp(string propName, Vector3 propPosition )
    {
        propName = "Prop" + propName;
        var flag = propPrefabs.ContainsKey(propName);
        if(!flag  ) {
            Debug.LogError("unknown prop name!");
            return;
        }
        var obj = propPrefabs[propName];
        var prop = Instantiate(obj);
        prop.transform.position = propPosition;
        prop.transform.parent = transform;
        prop.transform.localScale = new Vector3(1,1,1);
    }
}
