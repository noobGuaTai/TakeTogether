using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Tool.Utils
{
    class Utils { 
        // path from Resources
        // public static Dictionary<String, GameObject> getAllPrefab(string path)
        // {
        //     var ret = new Dictionary<String, GameObject>();
        //     var prefix = "Assets/Resources/";
        //     var folderPath = prefix + path;
        //     string[] files =
        //         Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);
        //     foreach (var filePath in files)
        //     {
        //         string objName = Path.GetFileNameWithoutExtension(filePath);
        //         var s1 = filePath.Substring(prefix.Length);
        //         var s2 = s1.Substring(0, s1.Length - 7);
        //         GameObject prefab = Resources.Load<GameObject>(s2);
        //         ret[objName] = prefab;
        //     }
        //     return ret;
        // }


        static public void ExitRange(Dictionary<Collider2D, bool> inRange, Collider2D collision)
        {
            if (inRange.ContainsKey(collision))
                inRange.Remove(collision);
        }

        public static void UpdateRange(Dictionary<Collider2D, bool> inRange) =>
            UpdateRange(inRange, (Collider2D _) => false);

        public static void UpdateRange(Dictionary<Collider2D, bool> inRange, Func<Collider2D, bool> process)
        {
            var removeList = new List<Collider2D>();
            var keyList = new List<Collider2D>(inRange.Keys);
            for (int i = 0; i < keyList.Count; i++)
            {
                var obj = keyList[i];
                if (obj.IsDestroyed() || process(obj))
                    inRange.Remove(obj);
            }
        }

        public static void EnterRange(Dictionary<Collider2D, bool> inRange, Collider2D collision) 
            => EnterRange(inRange, collision, (Collider2D _) => true);
        public static void EnterRange(Dictionary<Collider2D, bool> inRange, Collider2D collision, Func<Collider2D, bool> condition)
        {
            if(condition(collision))
                 inRange.Add(collision, true);
        }
    }
}
