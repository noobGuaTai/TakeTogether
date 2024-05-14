
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using System;
using System.Linq;
class GlobalEnv
{
    static private GlobalEnv _instance = null;

    static public GlobalEnv instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalEnv();
            }
            return _instance;
        }

    }
    GlobalEnv() { }

    public void Print(string text) { 
        Debug.Log(text);
    }
    public void PrintHello()
    {
        Debug.Log("Hello");
    }




    // 生成从0到n-1的随机排列
    public int[] GetRandPerm(int n) {
        System.Random rng = new System.Random();
        int[] array = Enumerable.Range(0, n).ToArray();

        // 使用Fisher-Yates洗牌算法来打乱数组
        for (int i = n - 1; i > 0; i--) {
            int swapIndex = rng.Next(i + 1);
            int temp = array[i];
            array[i] = array[swapIndex];
            array[swapIndex] = temp;
        }

        return array;
    }



    public bool EnableDebugKey = true;
    public bool EnableDebugOutput = true;

}