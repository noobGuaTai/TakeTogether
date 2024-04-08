using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Tool.Utils
{
    class Utils { 
        // path from Resources
        public static Dictionary<String, GameObject> getAllPrefab(string path)
        {
            var ret = new Dictionary<String, GameObject>();
            var prefix = "Assets/Resources/";
            var folderPath = prefix + path;
            string[] files =
                Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);
            foreach (var filePath in files)
            {
                string objName = Path.GetFileNameWithoutExtension(filePath);
                var s1 = filePath.Substring(prefix.Length);
                var s2 = s1.Substring(0, s1.Length - 7);
                GameObject prefab = Resources.Load<GameObject>(s2);
                ret[objName] = prefab;
            }
            return ret;
        }
    }
}
