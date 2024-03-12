using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理命名空间

public class StartGame : MonoBehaviour
{
    private bool isPlayerInside = false; // 用于检测玩家是否在物体内部

    void Update()
    {
        // 检查是否在物体内部且按下了"Attack"键
        if (isPlayerInside && Input.GetButtonDown("Attack")) // 确保在Input Manager中有一个名为"Attack"的按钮设置
        {
            LoadNewScene(); // 调用加载新场景的函数
        }
    }

    void OnTriggerEnter2D(Collider2D other) // 注意是Collider2D
    {
        Debug.Log(other.tag);
        // 检测进入碰撞的是不是玩家
        if (other.CompareTag("Player")) // 确保你的玩家对象有一个"Player"的标签
        {
            isPlayerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) // 注意是Collider2D
    {
        // 检测离开碰撞的是不是玩家
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    void LoadNewScene()
    {
        SceneManager.LoadScene("Level"); // 替换"Level"为你的目标场景名
    }
}
